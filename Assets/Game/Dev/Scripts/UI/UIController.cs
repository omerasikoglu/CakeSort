using System.Collections.Generic;
using System.Linq;
using CakeSort.World;
using TMPro;
using UnityEngine;
using VContainer;

namespace CakeSort.UI{

  public class UIController : MonoBehaviour{

    [Inject] GridManager gridManager;

    [SerializeField] Transform successVisual;
    [SerializeField] Transform failVisual;

    [SerializeField] TMP_Text remainingMoveCount;

    HashSet<Transform> visualSet;

    void Awake(){
      visualSet = new() { successVisual, failVisual };
    }

    void OnEnable(){
      gridManager.OnUpdateRemainingMoveCount += OnUpdateRemainingMoveCount;
      gridManager.OnLevelEnded += OnLevelEnded;
    } 
    void OnDisable(){
      gridManager.OnUpdateRemainingMoveCount -= OnUpdateRemainingMoveCount;
      gridManager.OnLevelEnded -= OnLevelEnded;
    }

    void OnUpdateRemainingMoveCount(int remainingMoveCount){
      this.remainingMoveCount.SetText(remainingMoveCount.ToString());
    }

    void OnLevelEnded(GridManager.LevelStatus levelStatus){
      Transform correctVisual = levelStatus switch {
        GridManager.LevelStatus.Succeed => successVisual,
        GridManager.LevelStatus.Failed  => failVisual,
        _                               => null,
      };
      OpenVisual(correctVisual);
    }

    void OpenVisual(Transform visual){
      visualSet.Where(o => o != visual).ToList().ForEach(o => o.gameObject.SetActive(false));
      if (visual is not null){
        visual.gameObject.SetActive(true);
      }
    }

  }

}