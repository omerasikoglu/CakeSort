using UnityEngine;

namespace Utils.Extension{

  public static class ListExtentions{
    public static void ForEveryChild(this Transform parent, System.Action<Transform> action){
      for (var i = parent.childCount - 1; i >= 0; i--){
        action(parent.GetChild(i));
      }
    }
  }

}