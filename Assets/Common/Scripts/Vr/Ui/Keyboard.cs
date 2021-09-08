using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Vr.Ui.Controls
{
  public class Keyboard : MonoBehaviour
  {
    public Button[] Buttons;
    public KbdKey[] ButtonMaps;
    public string[] ShiftedLabels;
    public GameObject[] MetaControls;
    public float[] Sizes;

    [NonSerialized] public bool SendKeyStrokes = true;
    public Action<KbdKey> OnKeyPressed;

    public void ClearModKeys(bool includeCaps = false)
    {
      if (includeCaps) { _mod_caps = false; }
      _mod_shift = _mod_caps;
      _mod_control = _mod_alt = false;
      RefreshShiftLabels();
      RefreshModKeyStates();
    }

    public void SetMetaControlVisibility(bool visible)
    {
      foreach (var metaControl in MetaControls)
      {
        metaControl.SetActive(visible);
      }
    }

    public void OnHideButton()
    {
      gameObject.SetActive(false);
    }

    public void OnSizeSelectionChanged(Dropdown src)
    {
      transform.localScale = Vector3.one * Sizes[src.Index];
    }

    public void OnKeyboardKeyPressed(Button key)
    {
      var buttonMap = ButtonMaps[_mapButtonToIndex[key]];
      if (SendKeyStrokes)
      {
        if (_shiftButtons.Contains(key))
        {
          _mod_shift = !_mod_shift;
          RefreshShiftLabels();
          RefreshModKeyStates();
        }
        else if (_controlButtons.Contains(key))
        {
          _mod_control = !_mod_control;
          RefreshModKeyStates();
        }
        else if (_altButtons.Contains(key))
        {
          _mod_alt = !_mod_alt;
          RefreshModKeyStates();
        }
        else if (_capsButtons.Contains(key))
        {
          _mod_caps = !_mod_caps;
          _mod_shift = _mod_caps;
          RefreshShiftLabels();
          RefreshModKeyStates();
        }
        else
        {
          if (_mod_shift) { OsHook_Keyboard.SetKeyState(KbdKey.Shift, true); }
          if (_mod_control) { OsHook_Keyboard.SetKeyState(KbdKey.Control, true); }
          if (_mod_alt) { OsHook_Keyboard.SetKeyState(KbdKey.Alt, true); }
          OsHook_Keyboard.SetKeyState(buttonMap, true);
          OsHook_Keyboard.SetKeyState(buttonMap, false);
          if (_mod_shift) { OsHook_Keyboard.SetKeyState(KbdKey.Shift, false); }
          if (_mod_control) { OsHook_Keyboard.SetKeyState(KbdKey.Control, false); }
          if (_mod_alt) { OsHook_Keyboard.SetKeyState(KbdKey.Alt, false); }
          ClearModKeys();
        }
      }
      OnKeyPressed?.Invoke(buttonMap);
    }

    private Dictionary<Button, uint> _mapButtonToIndex = new Dictionary<Button, uint>();
    private string[] _originalLabels;
    private List<Button> _shiftButtons = new List<Button>();
    private List<Button> _controlButtons = new List<Button>();
    private List<Button> _altButtons = new List<Button>();
    private List<Button> _capsButtons = new List<Button>();
    private bool _mod_shift, _mod_control, _mod_alt, _mod_caps;

    private void Start()
    {
      _originalLabels = new string[Buttons.Length];
      for (uint i = 0; i < Buttons.Length; i++)
      {
        _mapButtonToIndex.Add(Buttons[i], i);
        _originalLabels[i] = Buttons[i].Label.text;
        if (ButtonMaps[i] == KbdKey.Shift || ButtonMaps[i] == KbdKey.Left_Shift ||
            ButtonMaps[i] == KbdKey.Right_Shift)
        {
          _shiftButtons.Add(Buttons[i]);
        }
        if (ButtonMaps[i] == KbdKey.Control || ButtonMaps[i] == KbdKey.Left_Control ||
            ButtonMaps[i] == KbdKey.Right_Control)
        {
          _controlButtons.Add(Buttons[i]);
        }
        if (ButtonMaps[i] == KbdKey.Alt || ButtonMaps[i] == KbdKey.Left_Alt ||
            ButtonMaps[i] == KbdKey.Right_Alt)
        {
          _altButtons.Add(Buttons[i]);
        }
        if (ButtonMaps[i] == KbdKey.Capslock)
        {
          _capsButtons.Add(Buttons[i]);
        }
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

    private void RefreshShiftLabels()
    {
      if (_mod_shift)
      {
        for (var i = 0; i < Buttons.Length; i++)
        {
          if (ShiftedLabels[i] != "")
          {
            Buttons[i].Label.text = ShiftedLabels[i];
          }
        }
      }
      else
      {
        for (var i = 0; i < Buttons.Length; i++)
        {
          Buttons[i].Label.text = _originalLabels[i];
        }
      }
    }

    private void RefreshModKeyStates()
    {
      foreach (var key in _shiftButtons)
      {
        key.Label.text =
          _mod_shift ? key.Label.text.ToUpper() : _originalLabels[_mapButtonToIndex[key]];
      }
      foreach (var key in _controlButtons)
      {
        key.Label.text =
          _mod_control ? key.Label.text.ToUpper() : _originalLabels[_mapButtonToIndex[key]];
      }
      foreach (var key in _altButtons)
      {
        key.Label.text =
          _mod_alt ? key.Label.text.ToUpper() : _originalLabels[_mapButtonToIndex[key]];
      }
      foreach (var key in _capsButtons)
      {
        key.Label.text =
          _mod_caps ? key.Label.text.ToUpper() : _originalLabels[_mapButtonToIndex[key]];
      }
    }
  }
}
