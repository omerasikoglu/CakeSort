using System.Linq;
using UnityEngine;
using Utils.Extension;

namespace CakeSort.World{

  public class CakeSlice : MonoBehaviour{

    public CakeSliceSettings Settings{get; set;}

    public CakeType CakeType{get; private set;}
    
    void Awake(){
      // CakeType = Settings.type;
    }
  }

}