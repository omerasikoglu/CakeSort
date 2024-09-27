using System.Linq;
using UnityEngine;
using Utils.Extension;

namespace CakeSort.World{

  public class Cake : MonoBehaviour{

    public CakeSettings Settings{get; set;}

    public Transform[] cakeSliceTransformArray;

    void Awake(){
      cakeSliceTransformArray = transform.GetComponentsOnlyInChildren<MeshRenderer>().Select(o => o.transform).ToArray();
    }

    void OnEnable(){
      ResetChildrenPosition();
    }

    void OnDisable(){
      ResetChildrenPosition();
    }
    
    void ResetChildrenPosition() => cakeSliceTransformArray.ForEach(child => child.transform.position = default);

  }

}