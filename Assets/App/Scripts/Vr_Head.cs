using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vr_Head : MonoBehaviour
{
  public Vr_Hand[] Hands;

  private App_Input _input;

  private void Start()
  {
    _input = new App_Input();
    _input.Enable();
  }

  private void OnPreRender()
  {
    transform.localPosition = _input.VrHeadTracking.Position.ReadValue<Vector3>();
    transform.localRotation = _input.VrHeadTracking.Rotation.ReadValue<Quaternion>();
    foreach (var Hand in Hands)
    {
      Hand.OnPreRender();
    }
  }
}
