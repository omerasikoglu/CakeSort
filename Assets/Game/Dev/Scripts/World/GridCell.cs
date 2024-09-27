using System;
using UnityEngine;

namespace CakeSort.World{

  [Serializable] public class GridCellData{
    public Axis     Axis;
    public Vector3  WorldPosition;
    public bool     IsEmpty  = true; // TODO: remove | ADD: Plate CurrentPlate
    public CakeType cakeType = CakeType.Empty;
  }

  public class GridCell : MonoBehaviour{

    bool isPlateHovered;

    MeshRenderer canPutPlateLight;

    Plate currentPlate; // occupied plate

    void Awake(){
      canPutPlateLight = GetComponentInChildren<MeshRenderer>();
      TurnOffLight();
    }

    void OnTriggerEnter(Collider other){
      if (currentPlate != null) return;

      other.TryGetComponent(out IDrag drag);

      if (drag == null) return;
      if (isPlateHovered) return;

      drag.OnHoverInGridCell(this);
      TurnOnLight();
    }

    void OnTriggerExit(Collider other){
      if (currentPlate != null) return;

      other.TryGetComponent(out IDrag drag);

      if (drag == null) return;

      drag.OnHoverOutGridCell(this);
      isPlateHovered = false;
      TurnOffLight();
    }

    public void AddPlateToCell(Plate plate){
      currentPlate = plate;
      TurnOffLight();
    }

    void TurnOnLight(){
      isPlateHovered           = true;
      canPutPlateLight.enabled = true;
    }

    public void TurnOffLight(){
      isPlateHovered           = false;
      canPutPlateLight.enabled = false;
    }

    public void RemovePlate() => currentPlate = null;

  }

}