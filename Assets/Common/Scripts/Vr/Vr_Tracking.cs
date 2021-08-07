using UnityEngine;

public class Vr_Tracking : MonoBehaviour
{
  public Transform LeftHand;
  public Transform RightHand;

  private App_Input _input;
  private Vector3 _leftPosition;
  private Vector3 _rightPosition;
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

    var now = Time.time;
    var newLeftPosition = _input.VrLeftHandTracking.Position.ReadValue<Vector3>();
    var newRightPosition = _input.VrRightHandTracking.Position.ReadValue<Vector3>();

    LeftHand.localPosition = newLeftPosition;
    LeftHand.localRotation = _input.VrLeftHandTracking.Rotation.ReadValue<Quaternion>();

    RightHand.localPosition = newRightPosition;
    RightHand.localRotation = _input.VrRightHandTracking.Rotation.ReadValue<Quaternion>();

    // Handle hiding disabled controllers
    if (newLeftPosition != _leftPosition)
    {
      _leftPosition = newLeftPosition;
      _lastLeftControllerMoveTime = now;
      LeftHand.gameObject.SetActive(true);
    }
    else if ((now - _lastLeftControllerMoveTime) > _const_hideDisabledControllerDelay)
    {
      LeftHand.gameObject.SetActive(false);
      _lastLeftControllerMoveTime = float.MaxValue; // Prevent repeat calls to this code block
    }
    if (newRightPosition != _rightPosition)
    {
      _rightPosition = newRightPosition;
      _lastRightControllerMoveTime = now;
      RightHand.gameObject.SetActive(true);
    }
    else if ((now - _lastRightControllerMoveTime) > _const_hideDisabledControllerDelay)
    {
      RightHand.gameObject.SetActive(false);
      _lastRightControllerMoveTime = float.MaxValue; // Prevent repeat calls to this code block
    }
  }
}
