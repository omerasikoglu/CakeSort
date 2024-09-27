using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CakeSort.World{

  public class GridCreator : IInitializable{

    public event Action<GridCellData[]> OnGridCompleted; // gridCell

  #region Data
    const float X_INTERVAL = 0.2f;
    const float Y_HEIGHT   = 0f;
    const float Z_INTERVAL = 0.2f;
    
    readonly Vector2Int GRID_WIDTH_LENGTH = new(4, 5);
  #endregion

    Vector2Int gridWidthLength;

    GridCellData[] gridCellArray; // = grid

    public GridCreator(){
      gridWidthLength = GRID_WIDTH_LENGTH;
    }

    public void Initialize(){

      gridCellArray = new GridCellData[gridWidthLength.x * gridWidthLength.y];

      int counter = 0;

      for (int z = 0; z < gridWidthLength.y; z++){
        for (int x = 0; x < gridWidthLength.x; x++){

          Axis axis          = new(x, z);
          var  worldPosition = GetPosition(x, z);
          gridCellArray[counter] = new GridCellData {
            Axis           = axis,
            WorldPosition  = worldPosition,
            OccupyingPlate = null,
            GridCell       = null
          };

        #if UNITY_EDITOR
          Debug.DrawLine(
            GetPosition(x, z) + Vector3.left * .05f,
            GetPosition(x, z) + Vector3.right * .05f,
            Color.cyan,
            1000);
        #endif

          counter++;
        }
      }

      OnGridCompleted?.Invoke(gridCellArray);

    }

    Vector3 GetPosition(int x, int z){
      return new Vector3(x * X_INTERVAL, Y_HEIGHT, z * Z_INTERVAL);
    }

  }

}