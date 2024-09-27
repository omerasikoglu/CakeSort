using UnityEngine;
using VContainer;

namespace CakeSort.World{

  [RequireComponent(typeof(Plate), typeof(BoxCollider))]
  public class DragObject : MonoBehaviour, IDrag{
    
    Vector3 startMovePos; // counter waiting position

    BoxCollider boxCollider;
    GridCell    currentGridCell;
    Plate       attachedPlate;

    bool isDraggable = true;

    void Awake(){
      attachedPlate = GetComponent<Plate>();
      boxCollider   = GetComponent<BoxCollider>();
      startMovePos  = transform.position;
    }

  #region IDrag
    public bool IsDraggable() => isDraggable;

    public void OnStartDrag(){
      // play sfx
    }

    public void OnEndDrag(){
      if (currentGridCell == null){ // not inside any grid cell
        transform.position = startMovePos;
      }
      else{ // put it inside the grid cell
        transform.position = currentGridCell.transform.position;
        currentGridCell.AddPlateToCell(attachedPlate);
        boxCollider.enabled = false;
        isDraggable         = false;
        transform.SetParent(currentGridCell.transform);
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