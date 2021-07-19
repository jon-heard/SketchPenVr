using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Menu_Settings : Ui_Menu
{
  [SerializeField] private Ui_Menu _menu_setControls;
  [SerializeField] private Ui_Menu _menu_backdrops;
  [SerializeField] private Ui_Menu _menu_gripAdjust;
  [SerializeField] private Button _button_PressureLength;
  [SerializeField] private Dropdown _dropdown_handedness;
  [SerializeField] private Dropdown _dropdown_pressureLength;

  public override bool Show(Button source = null)
  {
    _dropdown_handedness.Index = (uint)(App_Details.Instance.IsLeftHanded ? 0 : 1);
    _dropdown_pressureLength.gameObject.SetActive(false);
    _dropdown_pressureLength.Index = App_Details.Instance.PressureLengthIndex;
    _button_PressureLength.State = Button.ButtonState.NotLockedDown;
    return base.Show(source);
  }

  public void OnPressureLengthButton(Button source)
  {
    _dropdown_pressureLength.gameObject.SetActive(true);
    _button_PressureLength.State = Button.ButtonState.LockedDown;
  }

  public void OnSetControlsButton(Button source)
  {
    _menu_setControls.Show(source);
  }

  public void OnBackdropsButton(Button source)
  {
    _menu_backdrops.Show(source);
  }

  public void OnGripAdjustButton(Button source)
  {
    _menu_gripAdjust.Show(source);
  }

  public void PressureLengthChanged()
  {
    App_Details.Instance.PressureLengthIndex = _dropdown_pressureLength.Index;
  }

  public void OnHandednessChanged()
  {
    App_Details.Instance.IsLeftHanded = (_dropdown_handedness.Index == 0);
    Hide();
  }

  protected override void Start()
  {
    base.Start();
    _dropdown_pressureLength.SetList(App_Details.Instance.PressureLengthTitles);
    _dropdown_handedness.SetList(new List<string> { "Left handed", "Right handed" });
  }
}
