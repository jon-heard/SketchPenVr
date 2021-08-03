using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Menu_Settings : Ui_Menu
{
  [Serializable]
  public struct BackgroundInfo
  {
    public string Title;
    public string Filename;
  }

  [SerializeField] private Ui_Menu _menu_setControls;
  [SerializeField] private Ui_Menu _menu_gripAdjust;
  [SerializeField] private Button _button_pressureLength;
  [SerializeField] private Button _button_background;
  [SerializeField] private Button _button_rumble;
  [SerializeField] private Button _button_handedness;
  [SerializeField] private Dropdown _dropdown_pressureLength;
  [SerializeField] private Dropdown _dropdown_background;
  [SerializeField] private Dropdown _dropdown_rumble;
  [SerializeField] private Dropdown _dropdown_handedness;
  [SerializeField] private BackgroundInfo[] _backgrounds;


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

    var backgroundFile = App_Details.Instance.Background;
    _dropdown_background.Index = 0;
    for (uint i = 0; i < _backgrounds.Length; i++)
    {
      if (_backgrounds[i].Filename == backgroundFile)
      {
        _dropdown_background.Index = i;
        break;
      }
    }
    _dropdown_background.gameObject.SetActive(false);
    _button_background.State = Button.ButtonState.NotLockedDown;

    return base.Show(source);
  }

  public void OnPressureLengthButton()
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

  public void OnBackgroundButton()
  {
    _dropdown_background.gameObject.SetActive(true);
    _button_background.State = Button.ButtonState.LockedDown;
  }

  public void BackgroundChanged()
  {
    App_Details.Instance.Background = _backgrounds[_dropdown_background.Index].Filename;
  }

  public void OnGripAdjustButton(Button source)
  {
    _menu_gripAdjust.Show(source);
  }

  public void OnRumbleButton()
  {
    _dropdown_rumble.gameObject.SetActive(true);
    _button_rumble.State = Button.ButtonState.LockedDown;
  }

  public void RumbleChanged()
  {
    App_Details.Instance.RumbleStrength = (App_Details.RumbleStrengthType)_dropdown_rumble.Index;
    Hide();
  }

  public void OnHandednessButton()
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
    // Background dropdown
    var backgroundTitles = new List<string>();
    foreach (var background in _backgrounds)
    {
      backgroundTitles.Add(background.Title);
    }
    _dropdown_background.SetList(backgroundTitles);
  }
}
