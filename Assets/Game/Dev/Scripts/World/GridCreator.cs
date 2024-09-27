using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CakeSort.World{

  [Serializable] public class GridCell{
    public Axis     Axis;
    public Vector3  WorldPosition;
    public bool     IsEmpty  = true;
    public CakeType cakeType = CakeType.Empty;
  }

  [Serializable] public struct Axis{ // grid cell position
    public int x;
    public int z;

    public Axis(int x, int z){
      this.x = x;
      this.z = z;
    }
  }

  public class GridCreator : IInitializable{

    public event Action<GridCell[]> OnGridCompleted; // gridCell

  #region Data
    const float X_INTERVAL = 0.2f;
    const float Y_HEIGHT   = 0f;
    const float Z_INTERVAL = 0.2f;
  #endregion

    Vector2Int gridWidthLength;

    GridCell[] gridCellArray; // = grid

    public GridCreator(Vector2Int gridWidthLength){
      this.gridWidthLength = gridWidthLength;
    }

    public void Initialize(){

      gridCellArray = new GridCell[gridWidthLength.x * gridWidthLength.y];

      int counter = 0;

      for (int z = 0; z < gridWidthLength.y; z++){
        for (int x = 0; x < gridWidthLength.x; x++){

          Axis axis          = new(x, z);
          var  worldPosition = GetPosition(x, z);
          gridCellArray[counter] = new GridCell { Axis = axis, WorldPosition = worldPosition };

          Debug.DrawLine(
            GetPosition(x, z) + Vector3.left * .05f,
            GetPosition(x, z) + Vector3.right * .05f,
            Color.cyan,
            1000);

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