#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using CakeSort.World;

namespace CakeSort.Editor{

#if UNITY_EDITOR

  [CustomEditor(typeof(GridManager))]
  public class TestGrid : UnityEditor.Editor{
    public override void OnInspectorGUI(){
      GridManager gridManager = (GridManager)target;

      DrawDefaultInspector();
      
      if (GUILayout.Button("LevelFailEnded")){
        gridManager.LevelFailEnded();
      }
      if (GUILayout.Button("LevelSuccessEnded")){
        gridManager.LevelSuccessEnded();
      }
      
    }
  }

#endif

}