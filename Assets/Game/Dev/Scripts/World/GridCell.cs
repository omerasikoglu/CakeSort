using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CakeSort.World{

  [Serializable] public class GridCellData{
    public Axis        Axis;
    public Vector3     WorldPosition;
    public GridManager GridManager;
    public GridCell    GridCell;
    
    public Plate       OccupyingPlate; // if null => empty
  }

  public class GridCell : MonoBehaviour{

    MeshRenderer emptyCellLight; // if cell is empty light enabled

    public GridCellData gridCellData;

    bool isPlateHovered;
    // public bool IsOccupied{get; private set;}

  #region Unity functions
    void Awake(){
      emptyCellLight = GetComponentInChildren<MeshRenderer>();
      TurnOffLight();
    }
  #endregion

  #region Collision
    void OnTriggerEnter(Collider other){
      if (gridCellData.OccupyingPlate != null) return;
      if (gridCellData.OccupyingPlate) return;

      other.TryGetComponent(out IDrag drag);

      if (drag == null) return;
      if (isPlateHovered) return;

      drag.OnHoverInGridCell(this);
      TurnOnLight();
    }

    void OnTriggerExit(Collider other){
      if (gridCellData.OccupyingPlate != null) return;
      if (gridCellData.OccupyingPlate) return;

      other.TryGetComponent(out IDrag drag);

      if (drag == null) return;

      drag.OnHoverOutGridCell(this);
      isPlateHovered = false;
      TurnOffLight();
    }
  #endregion

  #region Assign
    public void AssignGridCellData(GridCellData gridCellData){
      this.gridCellData = gridCellData;
    }
  #endregion

  #region Set
    public void AddPlateToCell(Plate plate){
      gridCellData.OccupyingPlate = plate;
      gridCellData.GridCell       = this;
      gridCellData.OccupyingPlate.AssignGridCell(this);
      gridCellData.GridManager.UpdateGrid(gridCellData);

      plate.AssignGridCell(this);
      
      TurnOffLight();
      // IsOccupied = true;
    }

    public void RemovePlateFromCell(Plate plate){ // plate ascend
      
      // gridCellData.OccupyingPlate.AssignGridCell(null);
      gridCellData.OccupyingPlate = null;
      // IsOccupied                  = false;
      TurnOffLight();
      Destroy(plate.gameObject);
      // gridCellData.GridManager.UpdateGrid(gridCellData);

    }
  #endregion

    void TurnOnLight(){
      isPlateHovered         = true;
      emptyCellLight.enabled = true;
    }

    public void TurnOffLight(){
      isPlateHovered         = false;
      emptyCellLight.enabled = false;
    }

    // public void SetIsOccupied(bool isOccupied) => IsOccupied = isOccupied;

  }

}