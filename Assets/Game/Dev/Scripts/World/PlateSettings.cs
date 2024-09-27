using UnityEngine;

namespace CakeSort.World{

  [CreateAssetMenu(menuName = "Scriptables/Plate")]
  public class PlateSettings :ScriptableObject{

    public GameObject prefab;
    
    public CakeSliceSettings[] cakeSliceSettings;

    public Plate Create(Transform parent){
      GameObject go       = Instantiate(prefab, parent);
      go.SetActive(true);
      go.name               = prefab.name;
      // go.transform.position = parent.position;

      Plate plate = go.GetComponent<Plate>();
      // Plate plate = go.AddComponent<Plate>();
      plate.Settings = this;

      return plate;
    }
  }

}