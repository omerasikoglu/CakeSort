using UnityEngine;

namespace CakeSort.World{

  [CreateAssetMenu(menuName = "Scriptables/Cake Container")]
  public class CakeContainer : ScriptableObject{
    public CakeSettings[] cakeArray;
  }

}