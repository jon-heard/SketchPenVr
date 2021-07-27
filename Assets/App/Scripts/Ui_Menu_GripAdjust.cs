using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using UnityEngine;

public class Ui_Menu_GripAdjust : Ui_Menu
{
  [SerializeField] private Button[] _buttons;

  public override bool Show(Button source = null)
  {
    var result = base.Show(source);
    App_Functions.Instance.SetFullUiLock(true);
    foreach (var button in _buttons)
    {
      button.Locker.SetLock(App_Details.LOCK__ALL_UI, false);
    }
    Controller.IsInGripAdjust = true;
    return result;
  }

  public override bool Hide()
  {
    if (!gameObject.activeSelf) { return true; }
    App_Functions.Instance.SetFullUiLock(false);
    return base.Hide();
  }

  public void OnAcceptButton()
  {
    Controller.AcceptGripAdjust();
    Hide();
  }
  public void OnCancelButton()
  {
    Controller.CancelGripAdjust();
    Hide();
  }

  public void OnResetButton()
  {
    Controller.ResetGripAdjust();
    Hide();
  }

  protected override void Start()
  {
    base.Start();
    var t = transform.localPosition;
    t.y = 0;
    transform.localPosition = t;
  }
}
