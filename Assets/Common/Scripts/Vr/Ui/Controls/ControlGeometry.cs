using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class ControlGeometry : MonoBehaviour
  {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public Control MyControl { get; private set; }

    private void Start()
    {
      MyControl = transform.parent.GetComponent<Control>();
    }
#endif
  }
}
