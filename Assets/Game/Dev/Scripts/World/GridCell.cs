using System;
using UnityEngine;

namespace CakeSort.World{

  [Serializable] public class GridCellData{
    public Axis        Axis;
    public Vector3     WorldPosition;
    public GridManager GridManager;
    public GridCell    GridCell;

    public Plate OccupyingPlate; // if null = empty
  }

  public class GridCell : MonoBehaviour{

    MeshRenderer emptyCellLight; // if cell is empty light enabled

    bool isPlateHovered;

    public GridCellData GridCellData{get; private set;}

  #region Unity functions
    void Awake(){
      emptyCellLight = GetComponentInChildren<MeshRenderer>();
      TurnOffLight();
    }
  #endregion

  #region Collision
    void OnTriggerEnter(Collider other){
      if (GridCellData.OccupyingPlate != null) return;
      if (GridCellData.OccupyingPlate) return;

      other.TryGetComponent(out IDrag drag);

      if (drag == null) return;
      if (isPlateHovered) return;

      drag.OnHoverInGridCell(this);
      TurnOnLight();
    }

    void OnTriggerExit(Collider other){
      if (GridCellData.OccupyingPlate != null) return;
      if (GridCellData.OccupyingPlate) return;

      other.TryGetComponent(out IDrag drag);

      if (drag == null) return;

      drag.OnHoverOutGridCell(this);
      isPlateHovered = false;
      TurnOffLight();
    }
  #endregion

  #region Assign
    public void AssignGridCellData(GridCellData gridCellData){
      GridCellData = gridCellData;
    }
  #endregion

    public void AddPlateToCell(Plate plate){
      GridCellData.OccupyingPlate = plate;
      GridCellData.GridCell       = this;
      GridCellData.OccupyingPlate.AssignGridCell(this);
      GridCellData.GridManager.UpdateGrid(GridCellData);

      plate.AssignGridCell(this);

      TurnOffLight();
      // IsOccupied = true;
    }

    public void RemovePlateFromCell(Plate plate){ // plate ascend

      // gridCellData.OccupyingPlate.AssignGridCell(null);
      GridCellData.OccupyingPlate = null;
      // IsOccupied                  = false;
      TurnOffLight();
      Destroy(plate.gameObject);
      // gridCellData.GridManager.UpdateGrid(gridCellData);

    }

    void TurnOnLight(){
      isPlateHovered         = true;
      emptyCellLight.enabled = true;
    }

    public void TurnOffLight(){
      isPlateHovered         = false;
      emptyCellLight.enabled = false;
    }
  }

}