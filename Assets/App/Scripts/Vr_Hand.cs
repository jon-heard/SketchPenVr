using UnityEngine;

public class Vr_Hand : MonoBehaviour
{
  public bool IsLeft = false;

  public void OnPreRender()
  {
    if (IsLeft)
    {
      transform.localPosition = _input.VrLeftHandTracking.Position.ReadValue<Vector3>();
      transform.localRotation = _input.VrLeftHandTracking.Rotation.ReadValue<Quaternion>();
    }
    else
    {
      transform.localPosition = _input.VrRightHandTracking.Position.ReadValue<Vector3>();
      transform.localRotation = _input.VrRightHandTracking.Rotation.ReadValue<Quaternion>();
    }
  }

  private App_Input _input;

  private void Start()
  {
    _input = new App_Input();
    _input.Enable();
  }
}
