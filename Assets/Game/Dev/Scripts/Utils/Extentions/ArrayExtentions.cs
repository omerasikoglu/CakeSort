using System;
using System.Collections.Generic;

namespace Utils.Extension{

  public static class ArrayExtentions{
    public static void ForEach<T>(this IEnumerable<T> array, Action<T> action){
      foreach (T child in array){
        action(child);
      }
    }

    public static void ForEach<T>(this IEnumerable<T> array, Action action){
      foreach (T child in array){ action(); }
    }
  }

}