using UnityEngine;
using UnityEngine.SceneManagement;

namespace CakeSort.Systems{

  public class LevelLoader{
    AsyncOperation currentLevelLoadOperation;

    string          currentLevelSceneName = string.Empty;
    readonly string startingLevelName     = "Level01";
    readonly int    startingLevelNumber   = 1;

    public int  CurrentLevel  {get; private set;}
    public bool IsLevelCreated{get; private set;}

    public LevelLoader(int levelNumber = 1){
      CurrentLevel = levelNumber;
      LoadLevel(levelNumber, false);
    }

    public void LoadLevel(int levelNumber, bool shouldIncreaseCurrentLevel = true){
      if (IsLevelCreated) return;
      if (currentLevelLoadOperation != null) return;

      string levelSceneName = "Level" + levelNumber.ToString("D2");

      if (SceneManager.GetSceneByName(levelSceneName).IsValid()){
        IsLevelCreated = true;
        return;
      }

      if (IsValidLevel(levelSceneName)){
        if (shouldIncreaseCurrentLevel) CurrentLevel++;
      }
      else{
        levelSceneName = startingLevelName;
        CurrentLevel   = startingLevelNumber;
      }

      currentLevelLoadOperation = SceneManager.LoadSceneAsync(levelSceneName, LoadSceneMode.Additive);

      currentLevelLoadOperation.completed += op => {
        IsLevelCreated            = true;
        currentLevelSceneName     = levelSceneName;
        currentLevelLoadOperation = null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelSceneName));

        var mainScene = SceneManager.GetSceneAt(0).name;
        Scene targetScene = SceneManager.GetSceneByName(mainScene);
        SceneManager.SetActiveScene(targetScene);
      };
    }

    bool IsAlreadyInBuildScenes(string levelSceneName){
      for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++){
        string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        // if (sceneName == levelSceneName) return true;
      }

      return false;
    }

    bool IsValidLevel(string levelSceneName){
      return SceneUtility.GetBuildIndexByScenePath(levelSceneName) != -1;
    }

    public void LevelCompleted(){
      UnloadCurrentLevel();
      LoadNextLevel();
    }

    void UnloadCurrentLevel(){
      if (!IsLevelCreated) return;

      SceneManager.UnloadSceneAsync(currentLevelSceneName);
      IsLevelCreated            = false;
      currentLevelSceneName     = string.Empty;
      currentLevelLoadOperation = null;
    }

    void LoadNextLevel(){
      LoadLevel(CurrentLevel + 1);
    }
  }

}