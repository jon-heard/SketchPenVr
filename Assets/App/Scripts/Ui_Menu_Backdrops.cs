using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using UnityEngine;

public class Ui_Menu_Backdrops : Ui_Menu
{
  [SerializeField] private string[] _filenames;
  [SerializeField] private Button[] _backdropButtons;

  protected override void Start()
  {
    base.Start();
    var t = transform.localPosition;
    t.y = 0;
    transform.localPosition = t;
  }

  public void OnBackdropSelected(Button source)
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
