using UnityEngine;
using UnityEngine.Events;

namespace Common.Vr.Ui.Controls
{
  public class ClickReactor : Control
  {
    [SerializeField] private UnityEvent OnClick;

    protected void OnClickedEventListener(Control focus)
    {
      if (focus == this)
      {
        OnClick.Invoke();
      }
    }

    private void OnEnable()
    {
      Control.OnControlClicked += OnClickedEventListener;
    }
    private void OnDisable()
    {
      Control.OnControlClicked -= OnClickedEventListener;
    }
  }
}
