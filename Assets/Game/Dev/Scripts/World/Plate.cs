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

  public class Plate : MonoBehaviour{
    public PlateSettings Settings{get; set;}

    public event Action OnPlateFull;

   public Transform[] cakeSliceTransformArray;

    readonly List<Cake> cakeSliceList = new(SLICE_AMOUNT);
    const    int        SLICE_AMOUNT  = 6;

  #region Variables
    [SerializeField] CakeContainer cakeContainer;

    Dictionary<int, CakeType> slotTypeDic; // slot index - cake type

    const int MAX_SLICE_SLOT = 6;
  #endregion

  #region Unity functions
    void Awake(){ }
  #endregion

    public void CreateCakeSlices(){
      if (cakeSliceList.Any()){ 
        cakeSliceList.ForEach(o => Destroy(o.gameObject)); 
        cakeSliceList.Clear();
      }

      for (int i = 0; i < cakeSliceList.Count; i++){
        
        if (Random.value < 0.5f){ // empty slice
          cakeSliceList[i] = null;
          continue;
        }

        var randomIndex = Random.Range(0, cakeContainer.cakeArray.Length);
        var cake        = cakeContainer.cakeArray[randomIndex].Create();
        cake.gameObject.SetActive(true);
        
        cakeSliceList[i] = cake;
      }

      // cake.transform.position = t.position;

      // cakeSliceList.Add(cake);
    }

  #region Logic
    public bool IsPlateContainsOnlyOneTypeOfCake(){
      // var nonEmptyCakeTypes = cakeSliceTransformArray.Where(o => o != CakeType.Empty).Distinct();

      // return nonEmptyCakeTypes.Count() == 1;
      return default;
    }

    public void AddSliceToPlate(CakeType type){
      for (int i = 0; i < MAX_SLICE_SLOT; i++){
        // if (cakeSliceTransformArray[i] != CakeType.Empty) continue;

        slotTypeDic[i] = type;
        // Add slice of cake visual
      }
    }

    bool CheckIsPlateFullWithSameType(){

      if (slotTypeDic.Count != MAX_SLICE_SLOT) return false;

      return true;
      // var a = slotTypeDic[CakeType.Chocolate];

    }

    GameObject GetCakePrefab(CakeType cakeType){
      return cakeType switch {
        // CakeType.Strawberry => cakeSlicePrefabArray[1],
        // CakeType.Chocolate  => cakeSlicePrefabArray[2],
        // CakeType.Lemon      => cakeSlicePrefabArray[3],
        // CakeType.Matcha     => cakeSlicePrefabArray[4],
        // _                   => cakeSlicePrefabArray[0]
      };
    }

  #region obsolete
    void InitDic(){
      slotTypeDic = new(MAX_SLICE_SLOT);
      for (int i = 0; i < MAX_SLICE_SLOT; i++){
        slotTypeDic[i] = CakeType.Empty;
      }
    }
  #endregion
  #endregion
  }


}