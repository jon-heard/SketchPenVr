using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using UnityEngine;

public class Ui_Menu_SetControls : Ui_Menu
{
  public override bool Show(Button source = null)
  {
    Ui_Control_ControlSetter.ClearFocus();
    _oldControlSettings = JsonUtility.ToJson(App_Details.Instance.MyControllerMappings);
    return base.Show(source);
  }

  public override bool Hide()
  {
    Ui_Control_ControlSetter.ClearFocus();
    if (JsonUtility.ToJson(App_Details.Instance.MyControllerMappings) != _oldControlSettings)
    {
      App_Details.Instance.SaveControllerMappings();
    }
    return base.Hide();
  }

  public void OnMenuClick()
  {
    Ui_Control_ControlSetter.ClearFocus();
  }

  public void OnResetButton()
  {
    Ui_Control_ControlSetter.ClearFocus();
    App_Details.Instance.MyControllerMappings.SetupDefault();
    Hide();
  }

  private string _oldControlSettings;
}
