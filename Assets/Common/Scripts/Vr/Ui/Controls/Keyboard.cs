using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Keyboard : MonoBehaviour
  {
    public Button[] Buttons;
    public KbdKey[] ButtonMaps;

    public Action<KbdKey> OnKeyPressed;

    public void OnKeyboardKeyPressed(Button key)
    {
      OnKeyPressed?.Invoke(_map_buttonToKey[key]);
    }

    private Dictionary<Button, KbdKey> _map_buttonToKey = new Dictionary<Button, KbdKey>();

    private void Start()
    {
      for (var i = 0; i < Buttons.Length; i++)
      {
        _map_buttonToKey.Add(Buttons[i], ButtonMaps[i]);
      }
      gameObject.SetActive(false);
    }

    private void OnDisable()
    {
      foreach (var button in Buttons)
      {
        button.State = Button.ButtonState.Idle;
      }
    }
  }
}
