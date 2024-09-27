using System;
using CakeSort.Input;
using CakeSort.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CakeSort.World{

  public class DragAndDrop : MonoBehaviour{
    [SerializeField] float  mouseDragSpeed = 0.1f;
    [SerializeField] Camera mainCam;

    MobileActionMap mobileActionMap;

    Vector3 velocity = Vector3.zero;

    bool isDragging;
    
    const float MAX_RAY_DISTANCE = 100f;
    const float DRAG_Y_HEIGHT    = 0.1f;

  #region Unity functions
    void Awake(){
      mainCam ??= Camera.main;
    }

    void OnEnable(){
      mobileActionMap = new();
      mobileActionMap.Enable();
      mobileActionMap.CoreLoopMap.Touch.performed += TouchPerformed;
    }

    void OnDisable(){
      mobileActionMap.CoreLoopMap.Touch.performed -= TouchPerformed;
      mobileActionMap.Disable();
    }
  #endregion

    void Update(){
      Debug.Log($"<color=green>{ mobileActionMap.CoreLoopMap.TouchContact.ReadValue<float>()}</color>");
    }

  #region Callbacks
    void TouchPerformed(InputAction.CallbackContext ctx){
      if (isDragging) return;

      Ray ray = mainCam.ScreenPointToRay(ctx.ReadValue<Vector2>());

      GameObject closestObject = Physics.Raycast(ray, out RaycastHit hit) ? hit.collider.gameObject : null;

      if (closestObject == null) return;
      if (!closestObject.CompareTag(Keys.Tag.DRAGGABLE)) return;
      if (closestObject.layer != LayerMask.NameToLayer(Keys.Layer.DRAGGABLE)) return;
      if (closestObject.GetComponent<IDrag>() == null) return;

      DragUpdate(closestObject).Forget();
    }

    async UniTask DragUpdate(GameObject clickedObject){
      clickedObject.TryGetComponent<IDrag>(out var dragObject);

      isDragging = true;
      
      dragObject?.OnStartDrag();
      
      while (HaveTouchContact()){
        Ray ray = mainCam.ScreenPointToRay(mobileActionMap.CoreLoopMap.Touch.ReadValue<Vector2>());

        Vector3 targetPosition = ray.GetPoint(MAX_RAY_DISTANCE);

        targetPosition.y = DRAG_Y_HEIGHT;

        clickedObject.transform.position = Vector3.SmoothDamp(
          clickedObject.transform.position,
          targetPosition,
          ref velocity,
          mouseDragSpeed);
        await UniTask.Yield();

      }

      isDragging = false;
      dragObject?.OnEndDrag();
    }
  #endregion

    bool HaveTouchContact() => mobileActionMap.CoreLoopMap.TouchContact.ReadValue<float>() > 0f;

  }

}