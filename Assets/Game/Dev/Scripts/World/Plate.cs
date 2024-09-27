using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CakeSort.World{

  [Serializable] public class CakeData{
    public CakeType       type;
    public MeshRenderer[] cakeModel;
  }

  [SelectionBase][RequireComponent(typeof(DragObject))]
  public class Plate : MonoBehaviour{
    public PlateSettings Settings{get; set;}

  #region Variables
    [SerializeField] CakeSliceSettings[] cakeSliceSettings; // prefabs
    [SerializeField] Transform[]         sliceSlotTransformArray;
    [SerializeField] Transform           cakeSlicesRoot;

    // Dictionary<int, CakeType> sliceSlotCakeTypeDic; // slot index - cake type

    List<CakeSlice> sliceList; // if cake slice = null => Empty slice slot

    const int MAX_SLICE_SLOT = 6;
    const int MAX_CAKE_TYPE  = 4;
  #endregion

    void Awake(){
      sliceList = new(MAX_SLICE_SLOT);
    }

  #region Cake Slice
    public void FillPlateWithSlices(Transform parent){
      if (sliceList.Any()) sliceList.Clear();

      for (int slotIndex = 0; slotIndex < MAX_SLICE_SLOT; slotIndex++){

        // if (Random.value < 0.5f){ // empty slice
        //   sliceList[slotIndex] = null;
        //   continue;
        // }

        CreateSlice(slotIndex);

      }
    }

    void CreateSlice(int slotIndex/*, Transform parent*/){
      var maxCakeType   = cakeSliceSettings.Length;
      var cakeTypeIndex = Random.Range(0, maxCakeType);

      var cakeSlice = cakeSliceSettings[cakeTypeIndex].Create(slotIndex, cakeSlicesRoot);
      cakeSlice.transform.position = sliceSlotTransformArray[slotIndex].position;
      cakeSlice.gameObject.SetActive(true);

      // var cakeTypePrefab = cakePrefabs.cakeSliceArray[cakeTypeIndex].fullCakePrefab.GetComponent<CakeSlice>().cakeSliceTransformArray[slotIndex];

      // var sliceTransform = Instantiate(cakeTypePrefab, parent, true);
      // cakeSlicePrefab.transform.position = sliceSlotTransformArray[slotIndex].transform.position;
      // sliceList[slotIndex] = cake;
    }

    // public void CreateCakeSlices(){
    //
    //   // for (int i = 0; i < platePlaceArray.Length; i++){
    //   //   var plate = plateSettings.Create();
    //   //   plate.transform.position = platePlaceArray[i].position;
    //   //   plate.gameObject.SetActive(true);
    //   //
    //   //   plate.FillPlateWithSlices();
    //   // }
    //
    //
    // }

    // cake.transform.position = t.position;

    // sliceList.Add(cake);
    // }
  #endregion

  #region Logic // TODO:
    // public bool IsPlateContainsOnlyOneTypeOfCake(){
    //   // var nonEmptyCakeTypes = cakeSliceTransformArray.Where(o => o != CakeType.Empty).Distinct();
    //
    //   // return nonEmptyCakeTypes.Count() == 1;
    //   return default;
    // }

// public void AddSliceToPlate(CakeType type){
    //   for (int i = 0; i < MAX_SLICE_SLOT; i++){
    //     // if (cakeSliceTransformArray[i] != CakeType.Empty) continue;
    //
    //     sliceSlotCakeTypeDic[i] = type;
    //     // Add slice of cake visual
    //   }
    // }
    //
    // bool CheckIsPlateFullWithSameType(){
    //
    //   if (sliceSlotCakeTypeDic.Count != MAX_SLICE_SLOT) return false;
    //
    //   return true;
    //   // var a = sliceSlotCakeTypeDic[CakeType.Chocolate];
    //
    // }

    // GameObject GetCakePrefab(CakeType cakeType){
    // return cakeType switch {
    // CakeType.Strawberry => cakeSlicePrefabArray[1],
    // CakeType.Chocolate  => cakeSlicePrefabArray[2],
    // CakeType.Lemon      => cakeSlicePrefabArray[3],
    // CakeType.Matcha     => cakeSlicePrefabArray[4],
    // _                   => cakeSlicePrefabArray[0]
    //   };

  #region obsolete
    // void InitDic(){
    //   sliceSlotCakeTypeDic = new(MAX_SLICE_SLOT);
    //   for (int i = 0; i < MAX_SLICE_SLOT; i++){
    //     sliceSlotCakeTypeDic[i] = CakeType.Empty;
    //   }
    // }
  #endregion
  #endregion

  }

}