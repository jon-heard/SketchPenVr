using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.Vr.Ui.Controls
{
  public class UiInputHandler_Mouse : UiInputHandler
  {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public IEnumerator Update(
      Camera camera, InputAction mousePosition, InputAction mouseButton)
    {
      RaycastHit hit;
      while (true)
      {
        yield return null;
        var ray = camera.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
        var rayHit = Physics.Raycast(ray, out hit);
        UpdatePointer(
          rayHit ? hit.collider.GetComponent<ControlGeometry>()?.MyControl : null,
          mouseButton.ReadValue<float>() == 1.0f,
          rayHit ? hit.point : Vector3.zero,
          true);
      }
    }
#endif
  }
}
