using UnityEngine;

namespace CakeSort.World{

  [CreateAssetMenu(menuName = "Scriptables/Plate")]
  public class PlateSettings :ScriptableObject{

    public GameObject prefab;

    public Plate CreatePlate(){
      GameObject go = Instantiate(prefab);
      go.SetActive(true);
      go.name = prefab.name;

      Plate plate = prefab.GetComponent<Plate>();
      plate.Settings = this;

      return plate;
    }
  }

}