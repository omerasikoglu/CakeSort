using UnityEngine;

namespace CakeSort.World{

  public enum CakeType{
    Empty      = 0,
    Strawberry = 1, // red
    Chocolate  = 2, // brown
    Lemon      = 3, // yellow
    Matcha     = 4, // green
  }

  [CreateAssetMenu(menuName = "Scriptables/Cake")]
  public class CakeSettings : ScriptableObject{
    public CakeType    type;
    public GameObject  prefab;
    
    public Transform[] CakeSliceArray{get; private set;}

    public Transform GetSlice(int sliceIndex) => prefab.GetComponent<Cake>().cakeSliceTransformArray[sliceIndex];
    
    public Cake Create(){
      GameObject go = Instantiate(prefab);
      go.SetActive(true);
      go.name = prefab.name;

      Cake cake = go.AddComponent<Cake>();
      cake.Settings = this;

      return cake;
    }
  }

}