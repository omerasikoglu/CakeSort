using System;
using CakeSort.World;
using UnityEngine;

namespace CakeSort.World{

  [RequireComponent(typeof(Plate))]
  public class DragObject : MonoBehaviour, IDrag{
    
    Vector3 startMovePos; // counter waiting position

    Plate attachedPlate;

    void Awake(){
      attachedPlate = GetComponent<Plate>();
      startMovePos  = transform.position;
    }

  #region IDrag
    GridCell currentGridCell;

    public void OnStartDrag(){
      // play sfx
    }

    public void OnEndDrag(){
      if (currentGridCell == null){ // not inside any grid cell
        transform.position = startMovePos;
      }
      else{
        transform.position = currentGridCell.transform.position;
        currentGridCell.AddPlateToCell(attachedPlate);
      }

      currentGridCell = null;
      // play sfx
    }

    public void OnHoverInGridCell(GridCell gridCell){
      if (currentGridCell != null){
        currentGridCell.TurnOffLight();
      }

      currentGridCell = gridCell;
    }

    public void OnHoverOutGridCell(GridCell hoverOutGridCell){
      if (currentGridCell != hoverOutGridCell){ // already hover in another grid cell
        return;
      }
      else{ // not inside any grid cell
        currentGridCell = null;
      }
    }
  #endregion

  }

}