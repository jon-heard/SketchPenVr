using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using Common.Vr.Ui.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Menu_Main : Ui_Menu
{
  [SerializeField] private Ui_Menu Menu_Settings;
  [SerializeField] private Ui_Menu Menu_RLPlaneAlign;
  [SerializeField] private Dropdown _dropdown_lock;
  [SerializeField] private Button _button_lock;
  [SerializeField] private Transform _dimmer;

  public override bool Show(Button source = null)
  {
    _dropdown_lock.gameObject.SetActive(false);
    _dropdown_lock.Index = (uint)App_Functions.Instance.MyScreen.LockType;
    _button_lock.State = Button.ButtonState.NotLockedDown;
    return base.Show(source);
  }

  public void OnMenuButton()
  {
    this.Toggle();
  }

  public void OnSettingsButton(Button source)
  {
    Menu_Settings.Show(source);
  }

  public void OnLockButton(Button source)
  {
    _dropdown_lock.gameObject.SetActive(true);
    _button_lock.State = Button.ButtonState.LockedDown;
  }

  public void OnLockChanged()
  {
    App_Functions.Instance.MyScreen.LockType = (Screen.ScreenLockType)_dropdown_lock.Index;
    Hide();
  }

  public void OnRLPlaneAlignButton(Button source)
  {
    Menu_RLPlaneAlign.Show(source);
  }

  public void OnQuitButton(Button source)
  {
    Confirm.ShowOnButtonParent(source, "Confirm quitting\nSketchPenVr", (confirmed) =>
    {
      Hide();
      if (confirmed)
      {
        Debug.Log("Quitting");
        Application.Quit();
      }
    });
  }

  protected override void Start()
  {
    base.Start();
    _dropdown_lock.SetList(new List<string>(Enum.GetNames(typeof(Screen.ScreenLockType))));
    StartCoroutine(PostStart());
  }
  private IEnumerator PostStart()
  {
    yield return new WaitForEndOfFrame();

    var screen = App_Functions.Instance.ScreenRenderer.transform;
    _dimmer.position = screen.position + new Vector3(0.0f, 0.0f, -0.0005f);
    _dimmer.rotation = screen.rotation;
    _dimmer.localScale = screen.lossyScale;

    Hide();
  }
}
