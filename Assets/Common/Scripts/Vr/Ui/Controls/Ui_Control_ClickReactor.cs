using UnityEngine;
using UnityEngine.Events;

public class Ui_Control_ClickReactor : Ui_Control
{
  [SerializeField] private UnityEvent OnClick;

  protected override void DoClickInternal()
  {
    OnClick.Invoke();
  }
}
