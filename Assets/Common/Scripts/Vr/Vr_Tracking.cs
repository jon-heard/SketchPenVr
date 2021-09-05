using UnityEngine;
using UnityEngine.Events;

namespace Common.Vr
{
  public class Vr_Tracking : SingletonComponent<Vr_Tracking>
  {
    public Transform LeftHand;
    public Transform RightHand;
    public UnityEvent OnTrackingUpdated;

    private App_Input _input;
    private float _lastLeftControllerMoveTime;
    private float _lastRightControllerMoveTime;
    private float _const_hideDisabledControllerDelay;

    private void Start()
    {
      _input = new App_Input();
      _input.Enable();
      _const_hideDisabledControllerDelay = App_Details.Instance.MyCommonDetails.DELAY_HIDE_DISABLED_CONTROLLER;
    }

    private void OnPreRender()
    {
      transform.localPosition = _input.VrHeadTracking.Position.ReadValue<Vector3>();
      transform.localRotation = _input.VrHeadTracking.Rotation.ReadValue<Quaternion>();

      var newLeftPosition = _input.VrLeftHandTracking.Position.ReadValue<Vector3>();
      var newRightPosition = _input.VrRightHandTracking.Position.ReadValue<Vector3>();
      var newLeftRotation = _input.VrLeftHandTracking.Rotation.ReadValue<Quaternion>();
      var newRightRotation = _input.VrRightHandTracking.Rotation.ReadValue<Quaternion>();

      var now = Time.time;
      var controllerTrackingMoved = false;

      // Handle hiding disabled controllers
      if (newLeftPosition != LeftHand.localPosition)
      {
        LeftHand.localPosition = newLeftPosition;
        controllerTrackingMoved = true;
        LeftHand.gameObject.SetActive(true);
        _lastLeftControllerMoveTime = now;
      }
      else if ((now - _lastLeftControllerMoveTime) > _const_hideDisabledControllerDelay)
      {
        LeftHand.gameObject.SetActive(false);
        _lastLeftControllerMoveTime = float.MaxValue; // Prevent repeat calls to this code block
      }

      if (newRightPosition != RightHand.localPosition)
      {
        RightHand.localPosition = newRightPosition;
        controllerTrackingMoved = true;
        RightHand.gameObject.SetActive(true);
        _lastRightControllerMoveTime = now;
      }
      else if ((now - _lastRightControllerMoveTime) > _const_hideDisabledControllerDelay)
      {
        RightHand.gameObject.SetActive(false);
        _lastRightControllerMoveTime = float.MaxValue; // Prevent repeat calls to this code block
      }

      if (newLeftRotation != LeftHand.localRotation)
      {
        LeftHand.localRotation = newLeftRotation;
        controllerTrackingMoved = true;
      }

      if (newRightRotation != RightHand.localRotation)
      {
        RightHand.localRotation = newRightRotation;
        controllerTrackingMoved = true;
      }

      if (controllerTrackingMoved)
      {
        OnTrackingUpdated.Invoke();
      }
    }
  }
}
