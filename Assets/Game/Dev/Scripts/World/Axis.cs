using System;

namespace CakeSort.World{

  [Serializable] public struct Axis{
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

    // grid cell position
    public int x;
    public int z;

    public Axis(int x, int z){
      this.x = x;
      this.z = z;
    }

    public static bool operator ==(Axis operand1, Axis operand2){
      return operand1.x == operand2.x && operand1.z == operand2.z;
    }

    public static bool operator !=(Axis operand1, Axis operand2){
      return !(operand1 == operand2);
    }
  }

}