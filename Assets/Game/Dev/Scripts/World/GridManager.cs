using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Extension;
using VContainer;

namespace CakeSort.World{

  public class GridManager : MonoBehaviour{

    [Inject] readonly GridCreator gridCreator;

    [SerializeField] GridCell gridCellPrefab;

    GridCellInfo[] mainGrid; // cast

    Dictionary<GridCellInfo, Plate> gridCellPlateDic; // not positions | 01,02,03,04,10,11,12,13..

  #region Observer
    void OnEnable(){
      gridCreator.OnGridCompleted += OnGridCompleted;
    }

    void OnDisable(){
      gridCreator.OnGridCompleted -= OnGridCompleted;
    }

    void OnGridCompleted(GridCellInfo[] gridCellInfoArray){
      InitDic(gridCellInfoArray);

      this.mainGrid = gridCellInfoArray;

      foreach (GridCellInfo gridCellInfo in gridCellInfoArray){
        CreateGridCell(gridCellInfo);
      }

    }

    void InitDic(IEnumerable<GridCellInfo> grid){
      gridCellPlateDic = new();
      foreach (GridCellInfo cell in grid){
        gridCellPlateDic[cell] = null;
      }

      foreach (var key in gridCellPlateDic.Keys){
        // Debug.Log($"<color=green>{key.Axis.x}{key.Axis.z}</color>");
      }
    }

    void CreateGridCell(GridCellInfo gridCellInfo){
      var spawnPos = gridCellInfo.WorldPosition;

      var gridCell = Instantiate(gridCellPrefab, spawnPos, Quaternion.identity, transform);

      gridCellInfo.GridManager = this;
      gridCellInfo.GridCell    = gridCell;

      // gridCell.SetGridManager(this);
      gridCell.SetGridCellInfo(gridCellInfo);
    }
  #endregion

  #region Logic
    public void UpdateGrid(GridCellInfo updatedGridCellInfo){
      mainGrid = mainGrid.Select(o => o == updatedGridCellInfo ? updatedGridCellInfo : o).ToArray();

      LookAtNeighbours(updatedGridCellInfo);
    }

    void LookAtNeighbours(GridCellInfo currentCellInfo){

      var viableCells = GetViableAdjacentGridCells(currentCellInfo).ToList();
      if (!viableCells.Any()) return;

      var viableNonPlateOccupiedCells = viableCells.Where(o => o.OccupyingPlate is null).ToList();
      if (!viableNonPlateOccupiedCells.Any()) return;

      var currentPlate = currentCellInfo.OccupyingPlate;
      if (!currentPlate.HaveAnySlice()){
        // currentCellInfo.Axis
        return;
      }

      IsCurrentPlateHaveOneDifferentTypeOfSlice(currentCellInfo, viableNonPlateOccupiedCells, viableCells);

    }

    static bool IsCurrentPlateHaveAnySlice(GridCellInfo currentGridCell){
      return currentGridCell.OccupyingPlate.GetSliceSlotDic().Count > 0;
    }

    void IsCurrentPlateHaveOneDifferentTypeOfSlice(GridCellInfo currentGridCell, List<GridCellInfo> viablePlateCells, List<GridCellInfo> viableGridCells){
      if (IsCurrentPlateOnlyHaveOneType(viablePlateCells)){ // Neighbours come to here

        var currentDic = currentGridCell.OccupyingPlate.GetSliceSlotDic();

        if (!CheckIsAnyEmptyPlateSlot(currentGridCell)) return;

        var currentPlateSliceType = currentDic.Values.First(o => o != null).CakeType;

        foreach (GridCellInfo gridCellInfo in viableGridCells){

          var targetPlate      = gridCellInfo.OccupyingPlate;
          var targetSliceArray = targetPlate.GetSliceSlotDic();

          CakeSlice targetSlice;
          int       targetSlotIndex = -1;

          for (int i = 0; i < targetSliceArray.Count; i++){
            if (targetSliceArray[i].CakeType != currentPlateSliceType) return;
            targetSlice     = targetSliceArray[i];
            targetSlotIndex = i;
          }

          targetPlate.RemoveCakeSlice(targetSlotIndex);
        }
      }

      else{
        // TODO: ASCEND EMPTY PLATE
      }
    }

    bool CheckIsAnyEmptyPlateSlot(GridCellInfo currentGridCell){
      return currentGridCell.OccupyingPlate.GetSliceSlotDic().Any(o => o.Value is null);
    }

    bool IsCurrentPlateOnlyHaveOneType(IEnumerable<GridCellInfo> viableGridCells){
      return viableGridCells.Distinct().Count() == 1;
    }

    IEnumerable<GridCellInfo> GetViableAdjacentGridCells(GridCellInfo currentGridCell){
      var currentX = currentGridCell.Axis.x;
      var currentZ = currentGridCell.Axis.z;

      var targetLeftAdjacent  = currentX - 1;
      var targetRightAdjacent = currentX + 1;
      var targetUpAdjacent    = currentZ + 1;
      var targetDownAdjacent  = currentZ - 1;

      Axis potantialRightNeighbour = new(targetRightAdjacent, currentZ);
      Axis potantialLeftNeighbour  = new(targetLeftAdjacent, currentZ);
      Axis potantialUpNeighbour    = new(currentX, targetUpAdjacent);
      Axis potantialDownNeighbour  = new(currentX, targetDownAdjacent);

      List<Axis> potantialNeighbourList = new() { potantialLeftNeighbour, potantialRightNeighbour, potantialUpNeighbour, potantialDownNeighbour };

      return mainGrid.Where(o =>
        potantialNeighbourList.Contains(o.Axis) &&
        o.OccupyingPlate == null);
    }

    public void HasNeighbourCellsHaveSameTypeCake(){ }

    void GetPlateEmptySlotCount(){ }
  #endregion

  #region Test
    public void TestGridCellCoords(){
      foreach (GridCellInfo cell in mainGrid){
        // Debug.Log($"<color=green>{cell.Axis.x} | {cell.Axis.z}</color>");
        Debug.Log($"<color=green>{cell.WorldPosition}</color>");
      }
    }
  #endregion

  }

}