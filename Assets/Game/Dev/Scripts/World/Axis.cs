using System;

namespace CakeSort.World{

  [Serializable] public struct Axis{

  #region Comparision
    public static bool operator ==(Axis operand1, Axis operand2){
      return operand1.x == operand2.x && operand1.z == operand2.z;
    }

    public static bool operator !=(Axis operand1, Axis operand2){
      return !(operand1 == operand2);
    }
    
    public bool Equals(Axis other){
      return x == other.x && z == other.z;
    }

    public override bool Equals(object obj){
      return obj is Axis other && Equals(other);
    }

    public override int GetHashCode(){
      unchecked{
        return (x * 397) ^ z;
      }
    }
  #endregion
    
    public int x;
    public int z;

    public Axis(int x, int z){ // grid cell position
      this.x = x;
      this.z = z;
    }
  }

}