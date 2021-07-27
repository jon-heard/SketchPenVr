using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Menu_Settings : Ui_Menu
{
  [SerializeField] private Ui_Menu _menu_setControls;
  [SerializeField] private Ui_Menu _menu_backdrops;
  [SerializeField] private Ui_Menu _menu_gripAdjust;
  [SerializeField] private Button _button_pressureLength;
  [SerializeField] private Button _button_rumble;
  [SerializeField] private Button _button_handedness;
  [SerializeField] private Dropdown _dropdown_pressureLength;
  [SerializeField] private Dropdown _dropdown_rumble;
  [SerializeField] private Dropdown _dropdown_handedness;

  public override bool Show(Button source = null)
  {
    _dropdown_pressureLength.Index = App_Details.Instance.PressureLengthIndex;
    _dropdown_pressureLength.gameObject.SetActive(false);
    _button_pressureLength.State = Button.ButtonState.NotLockedDown;

    _dropdown_rumble.Index = (uint)App_Details.Instance.RumbleStrength;
    _dropdown_rumble.gameObject.SetActive(false);
    _button_rumble.State = Button.ButtonState.NotLockedDown;
    if (Controller.PrimaryController._myXrDevice == null)
    {
      _button_rumble.State = Button.ButtonState.Disabled;
    }

    _dropdown_handedness.Index = (uint)(App_Details.Instance.IsLeftHanded ? 0 : 1);
    _dropdown_handedness.gameObject.SetActive(false);
    _button_handedness.State = Button.ButtonState.NotLockedDown;

    return base.Show(source);
  }

  public void OnPressureLengthButton(Button source)
  {
    _dropdown_pressureLength.gameObject.SetActive(true);
    _button_pressureLength.State = Button.ButtonState.LockedDown;
  }

  public void PressureLengthChanged()
  {
    App_Details.Instance.PressureLengthIndex = _dropdown_pressureLength.Index;
    Hide();
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

  public void OnRumbleButton(Button source)
  {
    _dropdown_rumble.gameObject.SetActive(true);
    _button_rumble.State = Button.ButtonState.LockedDown;
  }

  public void RumbleChanged()
  {
    App_Details.Instance.RumbleStrength = (App_Details.RumbleStrengthType)_dropdown_rumble.Index;
    Hide();
  }

  public void OnHandednessButton(Button source)
  {
    _dropdown_handedness.gameObject.SetActive(true);
    _button_handedness.State = Button.ButtonState.LockedDown;
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
    _dropdown_handedness.SetList(new List<string> { "Lefty", "Righty" });
    _dropdown_rumble.SetList(
      new List<string>(Enum.GetNames(typeof(App_Details.RumbleStrengthType))));
  }
}
