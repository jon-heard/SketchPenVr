using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using Common.Vr.Ui.Popups;
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

  [SerializeField] private Ui_Menu _menu_setPanelSizes;
  [SerializeField] private Ui_Menu _menu_setControls;
  [SerializeField] private Ui_Menu _menu_gripAdjust;
  [SerializeField] private Button _button_background;
  [SerializeField] private Button _button_pressureCurve;
  [SerializeField] private Button _button_pressureLength;
  [SerializeField] private Button _button_volume;
  [SerializeField] private Button _button_penPhysics;
  [SerializeField] private Button _button_haptics;
  [SerializeField] private Button _button_handedness;
  [SerializeField] private Dropdown _dropdown_background;
  [SerializeField] private Dropdown _dropdown_pressureCurve;
  [SerializeField] private Dropdown _dropdown_pressureLength;
  [SerializeField] private Dropdown _dropdown_volume;
  [SerializeField] private Dropdown _dropdown_penPhysics;
  [SerializeField] private Dropdown _dropdown_haptics;
  [SerializeField] private Dropdown _dropdown_handedness;
  [SerializeField] private BackgroundInfo[] _backgrounds;


  public override bool Show(Button source = null)
  {
    // Background
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

    // Pressure curve
    _dropdown_pressureCurve.Index = App_Details.Instance.PressureCurveIndex;
    _dropdown_pressureCurve.gameObject.SetActive(false);
    _button_pressureCurve.State = Button.ButtonState.NotLockedDown;

    // Pressure length
    _dropdown_pressureLength.Index = App_Details.Instance.PressureLengthIndex;
    _dropdown_pressureLength.gameObject.SetActive(false);
    _button_pressureLength.State = Button.ButtonState.NotLockedDown;

    // Volume
    _dropdown_volume.Index = App_Details.Instance.VolumeIndex;
    _dropdown_volume.gameObject.SetActive(false);
    _button_volume.State = Button.ButtonState.NotLockedDown;

    // Pen physics
    _dropdown_penPhysics.Index = (uint)App_Details.Instance.PenPhysics;
    _dropdown_penPhysics.gameObject.SetActive(false);
    _button_penPhysics.State = Button.ButtonState.NotLockedDown;

    // Haptics
    _dropdown_haptics.Index = (uint)App_Details.Instance.HapticsStrength;
    _dropdown_haptics.gameObject.SetActive(false);
    _button_haptics.State = Button.ButtonState.NotLockedDown;
#if !UNITY_EDITOR
    if (Controller.PrimaryController.MyXrDevice == null)
    {
      _button_haptics.State = Button.ButtonState.Disabled;
    }
#endif

    // Handedness
    _dropdown_handedness.Index = (uint)(App_Details.Instance.IsLeftHanded ? 0 : 1);
    _dropdown_handedness.gameObject.SetActive(false);
    _button_handedness.State = Button.ButtonState.NotLockedDown;

    return base.Show(source);
  }

  // Background
  public void OnBackgroundButton()
  {
    _dropdown_background.gameObject.SetActive(true);
    _button_background.State = Button.ButtonState.LockedDown;
  }
  public void BackgroundChanged()
  {
    App_Details.Instance.Background = _backgrounds[_dropdown_background.Index].Filename;
  }

  // Sizes
  public void OnSetPanelSizes(Button source)
  {
    _menu_setPanelSizes.Show(source);
  }

  // Controls
  public void OnSetControlsButton(Button source)
  {
    _menu_setControls.Show(source);
  }

  // Grip adjust
  public void OnGripAdjustButton(Button source)
  {
    _menu_gripAdjust.Show(source);
  }

  // Pressure curve
  public void OnPressureCurveButton()
  {
    _dropdown_pressureCurve.gameObject.SetActive(true);
    _button_pressureCurve.State = Button.ButtonState.LockedDown;
  }
  public void PressureCurveChanged()
  {
    App_Details.Instance.PressureCurveIndex = _dropdown_pressureCurve.Index;
    Hide();
  }

  // Pressure length
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

  // Volume
  public void OnVolumeButton()
  {
    _dropdown_volume.gameObject.SetActive(true);
    _button_volume.State = Button.ButtonState.LockedDown;
  }
  public void VolumeChanged()
  {
    App_Details.Instance.VolumeIndex = _dropdown_volume.Index;
    Hide();
  }

  // Pen physics
  public void OnPenPhysicsButton()
  {
    _dropdown_penPhysics.gameObject.SetActive(true);
    _button_penPhysics.State = Button.ButtonState.LockedDown;
  }
  public void OnPenPhysicsChanged()
  {
    App_Details.Instance.PenPhysics = (App_Details.PenPhysicsType)_dropdown_penPhysics.Index;
    Hide();
  }

  // Haptics
  public void OnHapticsButton()
  {
    _dropdown_haptics.gameObject.SetActive(true);
    _button_haptics.State = Button.ButtonState.LockedDown;
  }
  public void OnHapticsChanged()
  {
    App_Details.Instance.HapticsStrength = (App_Details.HapticsStrengthType)_dropdown_haptics.Index;
    Hide();
  }

  // Handedness
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

  // Reset settings
  public void OnResetSettingsButton(Button source)
  {
    Confirm.ShowOnButtonParent(source, "Confirm resetting all\nsettings to defaults.", (confirmed) =>
    {
      if (confirmed)
      {
        Hide();
        App_Details.Instance.ResetSettings();
      }
    });
  }


  protected override void Start()
  {
    base.Start();

    // Background dropdown
    var backgroundTitles = new List<string>();
    foreach (var background in _backgrounds)
    {
      backgroundTitles.Add(background.Title);
    }
    _dropdown_background.SetList(backgroundTitles);
    // Pen physics
    _dropdown_penPhysics.SetList(new List<string>(Enum.GetNames(typeof(App_Details.PenPhysicsType))));
    // Haptics
    _dropdown_haptics.SetList(
      new List<string>(Enum.GetNames(typeof(App_Details.HapticsStrengthType))));
  }
}
