using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CakeSort.World{

  public enum LevelStatus{
    Reset, Failed, Succeed
  }

  public class GridManager : MonoBehaviour{

    [Inject] readonly GridCreator gridCreator;

    public event Action              OnGridCreated;
    public event Action<LevelStatus> OnLevelEnded;       // Fail, Success, Reset
    public event Action<int>         OnPlateAddedToGrid; // remaining move count

  #region Variables
    [SerializeField] GridCell gridCellPrefab;

    // logic
    GridCellData[] mainGrid;
    GridCellData   currentCellData;
    List<Plate>    neighbourPlateList = new();

    int remainingMoveCount;

    // data
    const int MAX_MOVE_COUNT_FOR_WIN = 40;
  #endregion

  #region Properties
    Plate CurrentPlate => currentCellData?.OccupyingPlate;

    CakeType       GetYourFirstCakeType    => CurrentPlate.GetFirstCakeType();
    List<CakeType> GetAllDistinctCakeTypes => CurrentPlate.GetAllDistinctCakeTypes();

    bool HaveYouAnySlice             => CurrentPlate.HaveAnySlice();
    bool HaveYouOnlyOneDifferentType => CurrentPlate.HaveOnlyOneDistinctType();
    bool HaveEmptySlot               => CurrentPlate.HaveEmptySlot();
    bool IsPlateFull                 => CurrentPlate.IsPlateFull();
  #endregion

  #region Observer
    void OnEnable(){
      gridCreator.OnGridCompleted += OnGridCompleted;
    }

    void OnDisable(){
      gridCreator.OnGridCompleted -= OnGridCompleted;
    }

    void OnGridCompleted(GridCellData[] mainGrid){
      this.mainGrid = mainGrid;

      foreach (GridCellData gridCellInfo in mainGrid){
        CreateGridCell(gridCellInfo);
      }

      // init
      remainingMoveCount = MAX_MOVE_COUNT_FOR_WIN;
      OnPlateAddedToGrid?.Invoke(remainingMoveCount);
      OnGridCreated?.Invoke();
    }

    void CreateGridCell(GridCellData gridCellData){
      var spawnPos = gridCellData.WorldPosition;

      var gridCell = Instantiate(gridCellPrefab, spawnPos, Quaternion.identity, transform);

      gridCellData.GridManager = this;
      gridCellData.GridCell    = gridCell;
      gridCell.AssignGridCellData(gridCellData);
    }
  #endregion

  #region Logic // UML link is: Assets/Game/Dev
    public void UpdateGrid(GridCellData currentCellData){
      this.currentCellData = currentCellData;

      OnPlateAddedToGrid?.Invoke(--remainingMoveCount);

      UpdateMainGrid(this.currentCellData);

      var viableCells = GetViableAdjacentGridCells(this.currentCellData).ToList();
      if (!viableCells.Any()) return;

      UpdatePlates();

      CheckIsGridFull();
      CheckMoveCount();
    }

    void UpdatePlates(){

      if (!HaveYouAnySlice) return;

      CheckHaveYouEmptySlot();
    }

    void CheckHaveYouEmptySlot(){

      if (HaveYouOnlyOneDifferentType){
        IsPlateFullWithSameType(out bool isYourPlateFull);
        if (isYourPlateFull) return;

        LookingForNeighboursDistinct();
      }
      else{ // you have more than 1 types

        LookingForNeighbours();

      }
    }

    void LookingForNeighboursDistinct(){
      CakeType lookingCakeType = GetYourFirstCakeType;

      if (!neighbourPlateList.Any()){
        return;
      }

      foreach (var neighbourPlate in neighbourPlateList){
        for (int neighbourSlotIndex = 0; neighbourSlotIndex < 6; neighbourSlotIndex++){
          if (neighbourPlate.SlotIndexSliceDic[neighbourSlotIndex]?.CakeType != lookingCakeType) continue;
          AddSliceFromNeighbour(neighbourPlate, neighbourSlotIndex);
          return;
        }
      }
    }

    void LookingForNeighbours(){ // you are not distinct
      if (!neighbourPlateList.Any()){
        return;
      }

      List<CakeType> yourCakeTypeList = GetAllDistinctCakeTypes;

      if (neighbourPlateList.SelectMany(o => o.SlotIndexSliceDic.Values).Any(q => yourCakeTypeList.Any()) == false){
        return;
      }

      foreach (var yourCakeType in yourCakeTypeList){
        foreach (var neighbourCakeSlice in neighbourPlateList.SelectMany(o => o.SlotIndexSliceDic.Values).Where(q => q is not null)){
          if (neighbourCakeSlice == null) continue;
          if (neighbourCakeSlice.Plate == null) continue;
          if (yourCakeType != neighbourCakeSlice.CakeType) continue;

          var targetPlate = neighbourCakeSlice.Plate;

          if (targetPlate.IsPlateFull()) continue;

          var yourRemovedSliceSlotIndex = CurrentPlate.GetLastSlotIndexOfCakeType(yourCakeType);

          AddSliceToNeighbour(targetPlate, yourRemovedSliceSlotIndex);

          return;
        }
      }

    }

    void AddSliceToNeighbour(Plate neighbourPlate, int yourRemovedSliceSlotIndex){
      var neighbourEmptySlot = neighbourPlate.GetFirstEmptySlotIndex();
      var cakeSlice          = CurrentPlate.SlotIndexSliceDic[yourRemovedSliceSlotIndex];

      CurrentPlate.RemoveCakeSlice(yourRemovedSliceSlotIndex);
      neighbourPlate.AddCakeSlice(neighbourEmptySlot, cakeSlice);

      if (neighbourPlate.IsPlateFull()) neighbourPlate.AscendEmptyPlate();

      UpdatePlates();

    }

    void AddSliceFromNeighbour(Plate neighbourPlate, int neighbourSlotIndex){

      int yourfirstEmptySlotIndex = CurrentPlate.SlotIndexSliceDic.
        Where(o => o.Value is null).
        Select(kvp => kvp.Key).
        FirstOrDefault();

      var neighbourCakeSlice = neighbourPlate.SlotIndexSliceDic[neighbourSlotIndex];

      CurrentPlate.AddCakeSlice(yourfirstEmptySlotIndex, neighbourCakeSlice);

      neighbourPlate.RemoveCakeSlice(neighbourSlotIndex);

      if (neighbourPlate.IsPlateEmpty()){
        var neighbourGridCellData = neighbourPlate.OccupiedGridCell.GridCellData;

        neighbourGridCellData.OccupyingPlate = null;
        UpdateMainGrid(neighbourGridCellData);
        neighbourPlate.AscendEmptyPlate();
      }

      CheckHaveYouEmptySlot();
    }

    // ------------------------

    void IsPlateFullWithSameType(out bool isPlateFull){

      isPlateFull = IsPlateFull;

      if (!isPlateFull) return;

      CurrentPlate.AscendFullPlate();
      UpdateMainGrid(currentCellData);
    }

    void UpdateMainGrid(GridCellData gridCellData){ // only update current cell data
      mainGrid = mainGrid.Select(o => o.Axis == gridCellData.Axis ? gridCellData : o).ToArray();
    }

    IEnumerable<GridCellData> GetViableAdjacentGridCells(GridCellData currentGridCell){
      var currentX = currentGridCell.Axis.x;
      var currentZ = currentGridCell.Axis.z;

      var targetLeftAdjacent  = currentX - 1;
      var targetRightAdjacent = currentX + 1;
      var targetUpAdjacent    = currentZ + 1;
      var targetDownAdjacent  = currentZ - 1;

      Axis potantialLeftNeighbour  = new(targetLeftAdjacent, currentZ);
      Axis potantialRightNeighbour = new(targetRightAdjacent, currentZ);
      Axis potantialUpNeighbour    = new(currentX, targetUpAdjacent);
      Axis potantialDownNeighbour  = new(currentX, targetDownAdjacent);

      List<Axis> potantialNeighbourList = new() { potantialLeftNeighbour, potantialRightNeighbour, potantialUpNeighbour, potantialDownNeighbour };

      var viableGridCells = mainGrid.Where(o => potantialNeighbourList.Contains(o.Axis));
      // var viableNonPlateOccupiedCells = viableGridCells.Where(o => o.OccupyingPlate is null).ToList();
      var viablePlateOccupiedCells = viableGridCells.Where(o => o.OccupyingPlate is not null).ToList();

      neighbourPlateList = viablePlateOccupiedCells.Where(
        o => o.OccupyingPlate is not null).Select(o => o.OccupyingPlate).ToList();

      return viablePlateOccupiedCells;
    }
  #endregion

  #region Grid Status
    void CheckIsGridFull(){
      bool isFull = mainGrid.All(o => o.OccupyingPlate != null);
      if (isFull){ // game ends
        OnLevelEnded?.Invoke(LevelStatus.Failed);
      }
    }

    void CheckMoveCount(){
      if (remainingMoveCount <= 0){
        OnLevelEnded?.Invoke(LevelStatus.Succeed);
      }
    }

    public void ResetGrid(){
      remainingMoveCount = MAX_MOVE_COUNT_FOR_WIN;
      OnLevelEnded?.Invoke(LevelStatus.Reset);
      OnPlateAddedToGrid?.Invoke(remainingMoveCount);
      OnGridCreated?.Invoke();

      foreach (var cellData in mainGrid){
        if (cellData.OccupyingPlate is null) continue;
        cellData.GridCell.RemovePlateFromCell(cellData.OccupyingPlate);
        UpdateMainGrid(cellData);
      }
    }
  #endregion

  #region Test
    public void LevelSuccessEnded(){
      OnLevelEnded?.Invoke(LevelStatus.Succeed);
    }

    public void LevelFailEnded(){
      OnLevelEnded?.Invoke(LevelStatus.Failed);
    }
  #endregion

  }

}