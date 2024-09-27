using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils.Extension;
using Random = UnityEngine.Random;

namespace CakeSort.World{

  [SelectionBase][RequireComponent(typeof(DragObject))]
  public class Plate : MonoBehaviour{
    public PlateSettings Settings{get; set;}

  #region Variables
    [SerializeField] Transform[] sliceSlotTransformArray;
    [SerializeField] Transform   cakeSlicesRoot;

    // List<CakeSlice> sliceList = Enumerable.Repeat<CakeSlice>(null, MAX_SLICE_SLOT).ToList(); // if cake slice = null => Empty slice slot

    Dictionary<int, CakeSlice> slotIndexSliceDic;

    const int MAX_SLICE_SLOT = 6;
  #endregion

    void Awake(){
      slotIndexSliceDic = new();
      for (int i = 0; i < MAX_SLICE_SLOT; i++){
        slotIndexSliceDic[i] = null;
      }
    }

  #region Create functions
    public void FillPlateWithSlices(){
      for (int slotIndex = 0; slotIndex < MAX_SLICE_SLOT; slotIndex++){

        bool  allSlicesAreEmptyUntilLastSlice = 
          slotIndexSliceDic.Values.All(o => o is null) && slotIndex == MAX_SLICE_SLOT - 1;
        float emptySliceRatio                 = 0.75f;

        if (Random.value < emptySliceRatio && !allSlicesAreEmptyUntilLastSlice){ // empty slice
          slotIndexSliceDic[slotIndex] = null;
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
      
      slotIndexSliceDic[slotIndex] = cakeSlice;
    }
  #endregion

  #region Logic
    public void TryPutCakeSliceToEmptySlot(CakeSlice cakeSlice){
      if (!CheckPlateContainsOnlyOneTypeOfCake()) return;

      AddCakeSliceToEmptySlot(cakeSlice);

      CheckIsPlateFullWithSameType();
    }

    bool CheckPlateContainsOnlyOneTypeOfCake(){
      var nonEmptyCakeTypes = slotIndexSliceDic.Values.
        Where(o => o.CakeType != CakeType.Empty).Distinct();
      return nonEmptyCakeTypes.Count() == 1;
    }

    void AddCakeSliceToEmptySlot(CakeSlice cakeSlice){
      var slotIndex = cakeSlice.Settings.SliceSlotIndex;

      if (slotIndexSliceDic[slotIndex] != null) return;

      // cakeSlice.transform.SetParent();
    }

    void CheckIsPlateFullWithSameType(){

      // if (sliceSlotCakeTypeDic.Count != MAX_SLICE_SLOT) return false;

      // var a = sliceSlotCakeTypeDic[CakeType.Chocolate];
    }

    public void RemoveCakeSliceFromPlate(){ }

    void PlateIsFull(){ }

    void PlateIsEmpty(){ }

    public void IsPlateFull() { }
    public void IsPlateEmpty(){ }

    public bool HaveAnySlice(){

      // foreach (CakeSlice slice in sliceList){
      //   if (slice == null){
      //     Debug.Log($"<color=green>{"null"}</color>");
      //   }
      //   else{
      //     Debug.Log($"<color=green>{slice.CakeType}</color>");
      //   }
      // }

      foreach (CakeSlice slice in slotIndexSliceDic.Values){
        if (slice is null) continue;
        Debug.Log($"<color=green>{slice.CakeType}</color>");
      }

      return default;
    }
  #endregion

    public Dictionary<int, CakeSlice> GetSliceSlotDic() => slotIndexSliceDic;

    public void RemoveCakeSlice(int slotIndex){ }

    public void Dbg(){
      for (int index = 0; index < slotIndexSliceDic.Count; index++){
        CakeSlice slice = slotIndexSliceDic[index];
        Debug.Log($"<color=green>{(slice != null ? slice.CakeType : (CakeType?)null)} | {index}</color>");
      }
    }
  }

}