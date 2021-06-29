using UnityEngine;

public class Ui_Menu_SetControls : Ui_Menu
{
  public override bool Show(Ui_Control_Button source = null)
  {
    Ui_Control_ControlSetter.ClearFocus();
    return base.Show(source);
  }

  public override bool Hide()
  {
    Ui_Control_ControlSetter.ClearFocus();
    App_Details.Instance.SaveControllerMappings();
    return base.Hide();
  }

  public void OnMenuClick()
  {
    Ui_Control_ControlSetter.ClearFocus();
  }
}
