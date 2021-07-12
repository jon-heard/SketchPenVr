using Common.Vr.Ui.Controls;
using Common.Vr.Ui.Popups;
using System.Collections;
using UnityEngine;

public class Ui_Menu_Main : Common.Vr.Ui.Ui_Menu
{
  [SerializeField] private Common.Vr.Ui.Ui_Menu Menu_Settings;

  public void OnMenuButton()
  {
    this.Toggle();
  }

  public void OnSettingsButton(Button source)
  {
    Menu_Settings.Show(source);
  }

  public void OnQuitButton(Button source)
  {
    Ui_Popup_Confirm.ShowOnButtonParent(source, "Confirm quitting\nSketchPenVr", (confirmed) =>
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
