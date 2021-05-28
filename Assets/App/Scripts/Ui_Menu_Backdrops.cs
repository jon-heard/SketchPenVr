using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Menu_Backdrops : Ui_Menu
{
  [SerializeField] private string[] _filenames;
  [SerializeField] private Ui_Control_Button[] _backdropButtons;

  protected override void Start()
  {
    base.Start();
    var t = transform.localPosition;
    t.y = 0;
    transform.localPosition = t;
  }

  public void OnBackdropSelected(Ui_Control_Button source)
  {
    for (var i = 0; i < _backdropButtons.Length; i++)
    {
      if (_backdropButtons[i] == source)
      {
        App_Details.Instance.Backdrop = _filenames[i];
        break;
      }
    }
    Hide();
  }
}
