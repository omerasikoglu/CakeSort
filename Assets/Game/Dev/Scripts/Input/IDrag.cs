using CakeSort.World;

namespace CakeSort.World{

  interface IDrag{
    bool IsDraggable();
    void OnStartDrag();
    void OnEndDrag();
    void OnHoverInGridCell(GridCell gridCell);
    void OnHoverOutGridCell(GridCell gridCell);
  }

}