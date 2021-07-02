using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.Vr.Ui.Controls
{
  public class InputHandler_Mouse : InputHandler
  {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

    public IEnumerator Update(
      Camera camera, InputAction mousePosition, InputAction mouseButton)
    {
      while (true)
      {
        yield return null;

        // Mouse position
        var ray = camera.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
          PointerHovered(hit.collider.GetComponent<ControlGeometry>()?.MyControl);
        }
        else
        {
          PointerHovered(null);
        }

        // Mouse down
        PointerDown(mouseButton.ReadValue<float>() == 1.0f);
      }
    }
#endif
  }
}
