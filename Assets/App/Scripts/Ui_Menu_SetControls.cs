using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using UnityEngine;

public class Ui_Menu_SetControls : Ui_Menu
{
  [SerializeField] private Dropdown _dropdown_controllerSelect;
  [SerializeField] private GameObject[] _controllerMappings;

  public override bool Show(Button source = null)
  {
    Ui_Control_ControlSetter.ClearFocus();
    _oldControlSettings = JsonUtility.ToJson(App_Details.Instance.MyControllerMappings);
    _dropdown_controllerSelect.Index = 0;
    foreach (var controllerMapping in _controllerMappings)
    {
      controllerMapping.SetActive(false);
    }
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

  public void OnControllerSelected()
  {
    for (var i = 0; i < _controllerMappings.Length; i++)
    {
      _controllerMappings[i].SetActive(i == _dropdown_controllerSelect.Index-1);
    }
  }

  private string _oldControlSettings;

  protected override void Start()
  {
    base.Start();
    var t = transform.localPosition;
    t.x = 0;
    transform.localPosition = t;
  }
}
