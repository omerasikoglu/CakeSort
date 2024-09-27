using CakeSort.Input;
using CakeSort.UI;
using CakeSort.World;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CakeSort.Systems{

  public class Scope : LifetimeScope{

  #region Data // @formatter:off
    const int START_LEVEL_FROM = 1;
  #endregion

    protected override void Configure(IContainerBuilder builder){
      base.Configure(builder);

      builder.RegisterEntryPoint<GameManager>(); // need LevelLoader, GridCreator, GridManager

      builder.Register<LevelLoader>(Lifetime.Scoped).WithParameter(START_LEVEL_FROM);  // for GameManager
      builder.Register<GridCreator>(Lifetime.Scoped); // for GameManager, GridManager

      builder.RegisterComponentInHierarchy<GridManager>().UnderTransform(transform);  // for GameManager, need GridCreator
      builder.RegisterComponentInHierarchy<UIController>().UnderTransform(transform); // need GridManager
      builder.RegisterComponentInHierarchy<Counter>(); // need GridManager
    }
  }

}