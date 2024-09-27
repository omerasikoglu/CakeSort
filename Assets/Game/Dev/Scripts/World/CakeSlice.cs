using UnityEngine;

namespace CakeSort.World{

  public class CakeSlice : MonoBehaviour{

    public CakeSliceSettings Settings{get; set;}

    public CakeType CakeType{get; private set;}

    public int CakeSliceSlot{get; private set;}

    public Plate Plate;

    void Start(){
      CakeType = Settings.type;
      CakeSliceSlot = Settings.SliceSlotIndex;
    }

    public void SetSliceSlot(int newSlot){
      CakeSliceSlot = newSlot;
    }  
    public void SetPlate(Plate plate){
      Plate = plate;
    }
  }

}