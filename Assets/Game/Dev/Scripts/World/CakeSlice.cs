using UnityEngine;

namespace CakeSort.World{

  public class CakeSlice : MonoBehaviour{

    public CakeSliceSettings Settings{get; set;}

    public CakeType CakeType     {get; private set;}
    public Plate    Plate        {get; private set;}
    public int      CakeSliceSlot{get; private set;}

    void Start(){
      CakeType      = Settings.type;
      CakeSliceSlot = Settings.SliceSlotIndex;
    }

    public void AssignPlate(Plate plate){
      Plate = plate;
    }

    public void SetSliceSlot(int newSlot){
      CakeSliceSlot = newSlot;
    }

  }

}