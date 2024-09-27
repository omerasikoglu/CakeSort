using UnityEngine;

namespace Utils.Extension{

  public static class TransformExtentions{
    public static T[] GetComponentsOnlyInChildren<T>(this Transform transform) where T : Object{
      var  allComponents         = transform.GetComponentsInChildren<T>();
      bool isParentHaveComponent = transform.TryGetComponent(out T component);
      return isParentHaveComponent ? allComponents[1..] : allComponents;
    }
  }

}