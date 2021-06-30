using UnityEngine;
using UnityEngine.Events;

namespace Common.Vr.Ui.Controls
{
  public class ClickReactor : Control
  {
    [SerializeField] private UnityEvent OnClick;

    protected override void DoClickInternal()
    {
      OnClick.Invoke();
    }
  }
}
