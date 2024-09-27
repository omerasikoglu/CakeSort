namespace CakeSort.Utils{

  public struct Keys{

    public struct Input{
      public const string HOLD    = "Hold";
      public const string RELEASE = "Release";

      public struct ActionMap{
        public const string PLAYER_ACTION_MAP = "PlayerActionMap";
        public const string UI_ACTION_MAP     = "UIActionMap";
      }

      public const string HORIZONTAL = "Horizontal";
      public const string VERTICAL   = "Vertical";
    }

    public struct Editor{
      public const string BASE_DATA  = "Base Data";
      public const string CHILD_DATA = "Child Data";
    }

    public struct Layer{
      public const string CAKE      = "Cake";
      public const string DRAGGABLE = "Draggable";
    }

    public struct Tag{
      public const string CAKE      = "Cake";
      public const string DRAGGABLE = "Draggable";
    }

    public struct Sfx{
      public const string HOLD = "CakeHold";

      public static readonly string[] MERGE_ALL = { MERGE_1, MERGE_2, MERGE_3, MERGE_4, MERGE_5 };

      public const string MERGE_1 = "CakeMerge1";
      public const string MERGE_2 = "CakeMerge2";
      public const string MERGE_3 = "CakeMerge3";
      public const string MERGE_4 = "CakeMerge4";
      public const string MERGE_5 = "CakeMerge5";

      public const string FAIL    = "Fail";
      public const string SUCCESS = "Success";

    }
  }

}