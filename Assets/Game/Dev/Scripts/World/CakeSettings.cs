using UnityEngine;

namespace CakeSort.World{
  
  public enum CakeType{
    Strawberry, // red
    Chocolate,  // brown
    Lemon,      // yellow
    Matcha      // green
  }

  [CreateAssetMenu(menuName = "Scriptables/Cake")]
  public class CakeSettings : ScriptableObject{
    public GameObject prefab;
    public CakeType   type;

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