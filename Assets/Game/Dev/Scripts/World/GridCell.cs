using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CakeSort.World{

  [Serializable] public class GridCellInfo{
    public Axis     Axis;
    public Vector3  WorldPosition;
    public GridManager GridManager;
    public GridCell GridCell;
    public Plate    OccupyingPlate; // if null => empty
  }

  public class GridCell : MonoBehaviour{

    MeshRenderer        emptyCellLight; // if cell is empty light enabled
   
    // GridManager         gridManager;
    // GridCell            gridCell;
    public GridCellInfo gridCellInfo;

    bool isPlateHovered;
    bool isOccupied; // with plate

  #region Unity functions
    void Awake(){
      emptyCellLight = GetComponentInChildren<MeshRenderer>();
      TurnOffLight();
    }
  #endregion

  #region Cast
    // public void SetGridManager(GridManager gridManager){
    //   this.gridManager = gridManager;
    // }
    // public void SetGridCell(GridCell gridCell){
    //   this.gridCell = gridCell;
    // }

    public void SetGridCellInfo(GridCellInfo gridCellInfo){
      this.gridCellInfo = gridCellInfo;
    }
  #endregion

  #region Collision
    void OnTriggerEnter(Collider other){
      if (gridCellInfo.OccupyingPlate != null) return;
      if (isOccupied) return;

      other.TryGetComponent(out IDrag drag);

      if (drag == null) return;
      if (isPlateHovered) return;

      drag.OnHoverInGridCell(this);
      TurnOnLight();
    }

    void OnTriggerExit(Collider other){
      if (gridCellInfo.OccupyingPlate != null) return;
      if (isOccupied) return;

      other.TryGetComponent(out IDrag drag);

      if (drag == null) return;

      drag.OnHoverOutGridCell(this);
      isPlateHovered = false;
      TurnOffLight();
    }
  #endregion

    public void AddPlateToCell(Plate plate){
      gridCellInfo.OccupyingPlate = plate;
      TurnOffLight();
      isOccupied = true;

      gridCellInfo.GridManager.UpdateGrid(gridCellInfo);
    }

    public async UniTaskVoid RemovePlateFromCell(){ // Empty plate ascend
      gridCellInfo.OccupyingPlate = null;
      isOccupied                  = false;

      await UniTask.WaitForSeconds(1f);
      Debug.Log($"<color=red>{"plate removed"}</color>");
      gridCellInfo.GridManager.UpdateGrid(gridCellInfo);

    }

    void TurnOnLight(){
      isPlateHovered         = true;
      emptyCellLight.enabled = true;
    }

    public void TurnOffLight(){
      isPlateHovered         = false;
      emptyCellLight.enabled = false;
    }

    public bool IsOccupied() => isOccupied;

  }

}