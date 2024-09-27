using UnityEngine;

namespace CakeSort.World{

  public enum CakeType{
    Empty      = 0,
    Strawberry = 1, // red
    Chocolate  = 2, // brown
    Lemon      = 3, // yellow
    Matcha     = 4, // green
  }

  [CreateAssetMenu(menuName = "Scriptables/Cake Slice")]
  public class CakeSliceSettings : ScriptableObject{
    public CakeType     type;
    public GameObject[] CakeSlicePrefabArray;

    public int SliceSlotIndex{get; private set;}
    
    public CakeSlice Create(int sliceSlotIndex, Transform parent){

      GameObject targetPrefab = CakeSlicePrefabArray[sliceSlotIndex];
      
      GameObject go   = Instantiate(targetPrefab, parent);
      go.SetActive(true);
      go.name = targetPrefab.name;

      CakeSlice cakeSlice = go.AddComponent<CakeSlice>();
      SliceSlotIndex     = sliceSlotIndex;
      cakeSlice.Settings = this;

      return cakeSlice;
    }
  }

}