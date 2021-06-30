using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class ControlGeometry : MonoBehaviour
  {
#if UNITY_EDITOR
    public void OnMouseDown()
    {
      transform.parent.GetComponent<Control>().DoClick();
    }
#endif
  }
}
