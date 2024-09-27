using CakeSort.Input;
using CakeSort.World;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CakeSort.Systems{

  public class Scope : LifetimeScope{

  #region Data // @formatter:off
    readonly Vector2Int GRID_WIDTH_LENGTH = new(1, 2);
    const int START_LEVEL_FROM = 1;
  #endregion // @formatter:on

    protected override void Configure(IContainerBuilder builder){
      base.Configure(builder);

      builder.RegisterEntryPoint<GameManager>(); // need LevelLoader, GridCreator

      builder.Register<LevelLoader>(Lifetime.Singleton).WithParameter(START_LEVEL_FROM);  // for GameManager
      builder.Register<GridCreator>(Lifetime.Singleton).WithParameter(GRID_WIDTH_LENGTH); // for GameManager

      builder.RegisterComponentInHierarchy<PlayerInputManager>(); // for PlateController
      builder.RegisterComponentInHierarchy<AudioManager>();       // for Plate

    }
  }

}