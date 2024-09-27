using UnityEngine;

namespace CakeSort.World{

  public class Counter : MonoBehaviour{

    [SerializeField] Transform[] platePlaceArray;

    [SerializeField] PlateSettings plateSettings;

    public void CreatePlate(int repeatAmount = -1){

      repeatAmount = repeatAmount == -1 ? platePlaceArray.Length : repeatAmount;
      
      for (int i = 0; i < repeatAmount; i++){
        var plate = plateSettings.CreatePlate();
        plate.transform.position = platePlaceArray[i].position;
        plate.gameObject.SetActive(true);
      }
      
      // Create Cake Slices
    }

  }

}