using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using UnityEngine;

public class Ui_Menu_Settings : Ui_Menu
{
  [SerializeField] private Ui_Menu _menu_setControls;
  [SerializeField] private Ui_Menu _menu_backdrops;
  [SerializeField] private Ui_Menu _menu_gripAdjust;
  [SerializeField] private GameObject _button_handedness_left;
  [SerializeField] private GameObject _button_handedness_right;

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
    return base.Show(source);
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

  public void OnHandednessButton()
  {
    App_Details.Instance.IsLeftHanded = !App_Details.Instance.IsLeftHanded;
    Hide();
  }
}
