using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace CakeSort.Systems{

  public class GameManager : IInitializable{
    readonly LevelLoader levelLoader;

    [Inject] public GameManager(LevelLoader levelLoader){
      this.levelLoader = levelLoader;
    }

    public async void Initialize(){
      await UniTask.WaitUntil(() => levelLoader.IsLevelCreated);
    }
  }

}