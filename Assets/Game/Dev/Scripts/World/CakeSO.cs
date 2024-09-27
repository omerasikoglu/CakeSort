using UnityEngine;

namespace CakeSort.World{

  [CreateAssetMenu(menuName = "Scriptables/Cake Slice Container")]
  public class CakeSO : ScriptableObject{
    [SerializeField] public CakeSliceSettings[] cakeSliceArray;
  }

}