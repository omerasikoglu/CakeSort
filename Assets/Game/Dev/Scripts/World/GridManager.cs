using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Utils.Extension;
using VContainer;

namespace CakeSort.World{

  public class GridManager : MonoBehaviour{ //@formatter:off

    [Inject] readonly GridCreator gridCreator;
    
    [SerializeField] GridCell gridCellPrefab;

    GridCellData[] gridCellArray;
    
    Dictionary<Axis, Plate> gridCellPlateDic;

    float xInterval; float yHeight; float zInterval;  // @formatter:on

  #region Unity functions
    void OnEnable(){
      gridCreator.OnGridCompleted += OnGridCompleted;
    }

    void OnDisable(){
      gridCreator.OnGridCompleted -= OnGridCompleted;
    }
  #endregion

  #region Observer
    void OnGridCompleted(GridCellData[] gridCellArray){
      InitDic(gridCellArray);

      this.gridCellArray = gridCellArray;

      gridCellArray.ForEach(o => CreateColliders(o.WorldPosition));

      void CreateColliders(Vector3 pos) => Instantiate(gridCellPrefab, pos, Quaternion.identity, transform);

    }
  #endregion

    void InitDic(IEnumerable<GridCellData> grid){
      gridCellPlateDic = new();
      foreach (GridCellData cell in grid){
        gridCellPlateDic[cell.Axis] = null;
      }
    }

    bool IsCellEmpty(Axis axis){
      if (gridCellArray.Any(cell => cell.Axis.Equals(axis))){
        return true;
      }

      return gridCellPlateDic.TryGetValue(axis, out var plate);
    }

  #region Test
    public void TestGridCellCoords(){
      foreach (GridCellData cell in gridCellArray){
        // Debug.Log($"<color=green>{cell.Axis.x} | {cell.Axis.z}</color>");
        Debug.Log($"<color=green>{cell.WorldPosition}</color>");
      }
    }
  #endregion

  }

}