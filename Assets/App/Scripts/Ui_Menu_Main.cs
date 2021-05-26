using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Menu_Main : Ui_Menu
{
  [SerializeField] private Ui_Menu Menu_Settings;

  public void OnMenuButton()
  {
    this.Toggle();
  }

  public void OnSettingsButton(Ui_Control_Button source)
  {
    Menu_Settings.Show(source);
  }

  public void OnQuitButton(Ui_Control_Button source)
  {
    Ui_Popup_Confirm.ShowOnButtonParent(source, "Confirm quitting\nSketchpadVr", (confirmed) =>
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
    StartCoroutine(PostStart());
  }
  private IEnumerator PostStart()
  {
    yield return new WaitForEndOfFrame();
    Hide();
  }
}
