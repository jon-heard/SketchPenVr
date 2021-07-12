using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using UnityEngine;

public class Ui_Menu_Settings : Ui_Menu
{
  [SerializeField] private Ui_Menu _menu_setControls;
  [SerializeField] private Ui_Menu _menu_backdrops;
  [SerializeField] private Ui_Menu _menu_gripAdjust;
  [SerializeField] private Button _button_PressureLength;
  [SerializeField] private GameObject _button_handedness_left;
  [SerializeField] private GameObject _button_handedness_right;
  [SerializeField] private Dropdown _dropdown_pressureLength;

  public override bool Show(Button source = null)
  {
    if (App_Details.Instance.IsLeftHanded)
    {
      _button_handedness_left.SetActive(true);
      _button_handedness_right.SetActive(false);
    }
    else
    {
      _button_handedness_left.SetActive(false);
      _button_handedness_right.SetActive(true);
    }
    _dropdown_pressureLength.gameObject.SetActive(false);
    _button_PressureLength.State = Button.ButtonState.NotLockedDown;
    return base.Show(source);
  }

  public override bool Hide()
  {
    return base.Hide();
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

  public void OnHandednessButton()
  {
    App_Details.Instance.IsLeftHanded = !App_Details.Instance.IsLeftHanded;
    Hide();
  }

  protected override void Start()
  {
    base.Start();
    _dropdown_pressureLength.SetList(App_Details.Instance.PressureLengthTitles);
    _dropdown_pressureLength.Index = App_Details.Instance.PressureLengthIndex;
  }
}
