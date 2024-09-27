using CakeSort.World;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace CakeSort.Systems{

  public class GameManager : IInitializable{
    readonly LevelLoader levelLoader;
    readonly GridCreator gridCreator;

    [Inject] public GameManager(LevelLoader levelLoader, GridCreator gridCreator){
      this.levelLoader = levelLoader;
      this.gridCreator = gridCreator;
    }

    public async void Initialize(){
      await UniTask.WaitUntil(() => levelLoader.IsLevelCreated);
      gridCreator.Initialize();
    }
  }

}