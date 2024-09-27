using CakeSort.Input;
using CakeSort.World;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CakeSort.Systems{

  public class Scope : LifetimeScope{

  #region Data // @formatter:off
    readonly Vector2Int GRID_WIDTH_LENGTH = new(4, 5);
    const int START_LEVEL_FROM = 1;
  #endregion // @formatter:on

    protected override void Configure(IContainerBuilder builder){
      base.Configure(builder);

      builder.RegisterEntryPoint<GameManager>(); // need LevelLoader, GridCreator

      builder.Register<LevelLoader>(Lifetime.Scoped).WithParameter(START_LEVEL_FROM);  // for GameManager
      builder.Register<GridCreator>(Lifetime.Scoped).WithParameter(GRID_WIDTH_LENGTH); // for GameManager, GridManager
      
      // builder.RegisterComponentOnNewGameObject<GridManager>(Lifetime.Scoped).UnderTransform(transform); // for GameManager, need GridCreator
      builder.RegisterComponentInHierarchy<GridManager>().UnderTransform(transform); // for GameManager, need GridCreator

      builder.RegisterComponentInHierarchy<PlayerInputManager>();
      builder.RegisterComponentInHierarchy<AudioManager>(); // for Plate

    }
  }

}