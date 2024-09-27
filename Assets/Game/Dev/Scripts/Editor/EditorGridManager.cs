#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using CakeSort.World;

namespace CakeSort.Editor{

#if UNITY_EDITOR

  [CustomEditor(typeof(GridManager))]
  public class EditorGridManager : UnityEditor.Editor{
    public override void OnInspectorGUI(){
      GridManager gridManager = (GridManager)target;

      DrawDefaultInspector();

      if (GUILayout.Button("Test Grid Cell Coords")){
        gridManager.TestGridCellCoords();
      }
    }
  }

  [CustomEditor(typeof(Counter))]
  public class TestCounter : UnityEditor.Editor{
    public override void OnInspectorGUI(){
      Counter counter = (Counter)target;

      DrawDefaultInspector();

      if (GUILayout.Button("Test Create 3 Plate")){
        counter.CreatePlate();
      }
    }
  }

#endif

}