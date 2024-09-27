using System.Linq;
using UnityEngine;
using Utils.Extension;

namespace CakeSort.World{

  public class CakeSlice : MonoBehaviour{

    public CakeSliceSettings Settings{get; set;}

    public CakeType CakeType{get; private set;}

    public int CakeSliceSlot{get; private set;}

    void Start(){
      CakeType = Settings.type;
      CakeSliceSlot = Settings.SliceSlotIndex;
    }
  }

}