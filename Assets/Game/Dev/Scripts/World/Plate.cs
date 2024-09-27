using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CakeSort.World{

  [SelectionBase][RequireComponent(typeof(DragObject))]
  public class Plate : MonoBehaviour{
    public PlateSettings Settings{get; set;}

    [SerializeField] Transform[] sliceSlotTransformArray;
    [SerializeField] Transform   cakeSlicesRoot;

    public Dictionary<int, CakeSlice> SlotIndexSliceDic{get; private set;}
    public GridCell                   OccupiedGridCell {get; private set;}

    const int MAX_SLICE_SLOT = 6;

    void Awake(){
      SlotIndexSliceDic = new();
      for (int i = 0; i < MAX_SLICE_SLOT; i++){
        SlotIndexSliceDic[i] = null;
      }
    }

    public void AssignGridCell(GridCell gridCell){
      OccupiedGridCell = gridCell;
    }

  #region Create
    public void FillPlateWithSlices(){
      for (int slotIndex = 0; slotIndex < MAX_SLICE_SLOT; slotIndex++){

        bool allSlicesAreEmptyUntilLastSlice =
          SlotIndexSliceDic.Values.All(o => o is null) && slotIndex == MAX_SLICE_SLOT - 1;

        float emptySliceRatio = 0.75f;
        if (Random.value < emptySliceRatio && !allSlicesAreEmptyUntilLastSlice){ // empty slice
          SlotIndexSliceDic[slotIndex] = null;
          continue;
        }

        CreateSlice(slotIndex);
      }
    }

    void CreateSlice(int slotIndex){
      var maxCakeType   = Settings.cakeSliceSettings.Length;
      var cakeTypeIndex = Random.Range(0, maxCakeType);

      var cakeSlice = Settings.cakeSliceSettings[cakeTypeIndex].Create(slotIndex, cakeSlicesRoot);
      cakeSlice.transform.position = sliceSlotTransformArray[slotIndex].position;
      cakeSlice.gameObject.SetActive(true);

      SlotIndexSliceDic[slotIndex] = cakeSlice;
      cakeSlice.AssignPlate(this);
    }
  #endregion

  #region Get
    public bool HaveAnySlice() => SlotIndexSliceDic.Values.Any(o => o is not null);

    public bool HaveOnlyOneDistinctType() => GetDistinctCakeType() == 1;

    public bool HaveEmptySlot() => SlotIndexSliceDic.Values.Any(o => o is null);

    public CakeType GetFirstCakeType() => SlotIndexSliceDic.Values.First(o => o is not null).CakeType;

    public List<CakeType> GetAllDistinctCakeTypes() => 
      SlotIndexSliceDic.Values.Where(q => q is not null).Select(o => o.CakeType).Distinct().ToList();

    public int GetDistinctCakeType() => SlotIndexSliceDic.Values.Where(
      o => o is not null && o.CakeType is not CakeType.Empty).Select(o => o.CakeType).Distinct().Count();

    public bool IsPlateFull(){
      return !SlotIndexSliceDic.Values.Any(o => o is null);
    }

    public bool IsPlateEmpty() => SlotIndexSliceDic.Values.All(o => o is null);

    public IEnumerable<int> GetEmptySlotIndexes(){
      return SlotIndexSliceDic.
        Where(o => o.Value is null || o.Value.CakeType is CakeType.Empty).
        Select(o => o.Key).ToList();
    }

    public int GetFirstEmptySlotIndex() => GetEmptySlotIndexes().First();

    public int GetLastSlotIndexOfCakeType( CakeType cakeType ){
      return SlotIndexSliceDic.
        Where(o => o.Value is not null && o.Value.CakeType == cakeType).
        Select(o => o.Key).Last(); 
    }
    
  #endregion

  #region Set
    public void AddCakeSlice(int slotIndex, CakeSlice cakeSlice){

      cakeSlice.transform.position         = sliceSlotTransformArray[slotIndex].transform.position;
      cakeSlice.transform.localEulerAngles = new(0f, 60f * slotIndex, 0f);

      cakeSlice.SetSliceSlot(slotIndex);
      cakeSlice.AssignPlate(this);
      cakeSlice.transform.SetParent(cakeSlicesRoot);

      SlotIndexSliceDic[slotIndex] = cakeSlice;
    }

    public void RemoveCakeSlice(int slotIndex){
      SlotIndexSliceDic[slotIndex] = null;
    }

    public void AscendEmptyPlate(){
      OccupiedGridCell.RemovePlateFromCell(this);
    }

    public void AscendFullPlate(){
      OccupiedGridCell.RemovePlateFromCell(this);

    }
  #endregion

  }

}