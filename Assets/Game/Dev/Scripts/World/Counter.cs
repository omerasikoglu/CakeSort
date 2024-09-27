using UnityEngine;
using VContainer;

namespace CakeSort.World{

  public class Counter : MonoBehaviour{

    [Inject] GridManager gridManager;
    
    [SerializeField] Transform[] plateRootTransformArray;

    [SerializeField] PlateSettings plateSettings;

    public void CreatePlate(int repeatAmount = -1){
      foreach (Transform plateRoot in plateRootTransformArray){
        var parent = plateRoot.transform;

        var plate = plateSettings.Create(parent);
        plate.transform.position = plateRoot.position;
        plate.gameObject.SetActive(true);

        plate.FillPlateWithSlices();
      }
    }

  }

}