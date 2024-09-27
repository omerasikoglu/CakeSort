using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CakeSort.World{

  public class Counter : MonoBehaviour{

    [SerializeField] Transform[] platePlaceArray;

    [SerializeField] PlateSettings plateSettings;

    List<Plate> createdPlateList = new();

    public void CreatePlate(int repeatAmount = -1){
      // repeatAmount = repeatAmount == -1 ? platePlaceArray.Length : repeatAmount;

      for (int i = 0; i < platePlaceArray.Length; i++){

        var parent = platePlaceArray[i].transform;

        var plate = plateSettings.Create(parent);
        plate.transform.position = platePlaceArray[i].position;
        plate.gameObject.SetActive(true);

        createdPlateList.Add(plate);
      }

      // foreach (Plate plate in createdPlateList){
      //   plate.FillPlateWithSlices(plate.transform);
      // }
    }

  }

}