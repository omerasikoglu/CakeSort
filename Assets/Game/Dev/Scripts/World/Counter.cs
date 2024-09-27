using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CakeSort.World{

  public class Counter : MonoBehaviour{

    [Inject] GridManager gridManager;

    [SerializeField] Transform[]   plateRootTransformArray;
    [SerializeField] PlateSettings plateSettings;

    bool isCounterEmpty;
    int  moveCounter = -1;

    readonly HashSet<Plate> plateSet = new();

    void OnEnable(){
      gridManager.OnGridCreated      += GridCreated;
      gridManager.OnPlateAddedToGrid += MovePerformed;
    }

    void OnDisable(){
      gridManager.OnGridCreated      -= GridCreated;
      gridManager.OnPlateAddedToGrid -= MovePerformed;
    }

    void MovePerformed(int remainingMoveCount){
      moveCounter++;
      isCounterEmpty = moveCounter % 3 == 0 && moveCounter != 0;

      if (isCounterEmpty){
        CreatePlates();
      }
    }

    void GridCreated(){
      ResetCounter();
      CreatePlates();
    }

    void ResetCounter(){
      moveCounter = 0;

      foreach (Plate plate in plateSet.Where(o => o.OccupiedGridCell == null)){
        Destroy(plate.gameObject);
      }

      plateSet.Clear();
    }

    void CreatePlates(){
      foreach (Transform plateRoot in plateRootTransformArray){
        var parent = plateRoot.transform;

        var plate = plateSettings.Create(parent);
        plate.transform.position = plateRoot.position;
        plate.gameObject.SetActive(true);
        plateSet.Add(plate);

        plate.FillPlateWithSlices();
      }
    }

  }

}