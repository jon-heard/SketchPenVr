using Common;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Textbox : Control
  {
    [Header("Wiring")]
    public Renderer Geometry;
    public TextMesh Label;

    public string Text
    {
      get { return _text; }
      set
      {
        if (value == _text) { return; }
        _text = value;
        Label.text = _text + "<material=1>|</material>";
        var leftChar = 0;
        while (Label.GetTextWidth() >
            Geometry.transform.localScale.x - App_Details.Instance.MyCommonDetails.TEXTBOX_TEXT_X_OFFSET)
        {
          leftChar++;
          Label.text = "..." + _text.Substring(leftChar) + "<material=1>|</material>";
        }
      }
    }
    private string _text;

    public void Paste()
    {
      Text += GUIUtility.systemCopyBuffer;
    }

    private float _lastBlink;
    private float _const_blinkSpeed;
    private Material _blinkMaterial;

    private static Textbox _focusTextbox;
    private static string _focusTextboxInitialText;

    protected override void Awake()
    {
      base.Awake();
      Label.anchor = TextAnchor.MiddleLeft;
      var p = Label.transform.localPosition;
      p.x = -Geometry.transform.localScale.x * 0.5f + App_Details.Instance.MyCommonDetails.TEXTBOX_TEXT_X_OFFSET;
      Label.transform.localPosition = p;
      _const_blinkSpeed = App_Details.Instance.MyCommonDetails.TEXTBOX_CARET_BLINKSPEED;
      _blinkMaterial = Label.GetComponent<Renderer>().materials[1];
      Text = "";
    }

    private void OnEnable()
    {
      Control.OnControlClicked += OnClickedEventListener;
    }

    private void OnDisable()
    {
      Control.OnControlClicked -= OnClickedEventListener;
    }

    private void OnDestroy()
    {
      if (_focusTextbox != null)
      {
        _focusTextbox = null;
        UnhookWindowsKeyboard();
      }
    }

    protected void OnClickedEventListener(Control focus)
    {
      if (focus == this)
      {
        if (_focusTextbox != this)
        {
          _focusTextbox = this;
          StartCoroutine(VrKeyboardCoroutine());
          StartCoroutine(CaretBlinkCoroutine());
          HookWindowsKeyboard();
        }
        _focusTextboxInitialText = _focusTextbox.Text;
      }
      else if (_focusTextbox == this)
      {
        _focusTextbox = null;
        StopAllCoroutines();
        UnhookWindowsKeyboard();
        var blinkColor = _blinkMaterial.color;
        blinkColor.a = 0.0f;
        _blinkMaterial.color = blinkColor;
      }
    }

    private IEnumerator VrKeyboardCoroutine()
    {
      TouchScreenKeyboard.hideInput = false;
      var vrKeyboard = TouchScreenKeyboard.Open(Text, TouchScreenKeyboardType.Default, false);
      if (vrKeyboard == null) { yield break; }
      while (vrKeyboard.active)
      {
        yield return null;
        if (vrKeyboard.text != Text)
        {
          Text = vrKeyboard.text;
        }
      }
    }

    private IEnumerator CaretBlinkCoroutine()
    {
      while (true)
      {
        yield return null;
        if (Time.time - _lastBlink > _const_blinkSpeed)
        {
          _lastBlink = Time.time;
          var blinkColor = _blinkMaterial.color;
          blinkColor.a = blinkColor.a == 1.0f ? 0.0f : 1.0f;
          _blinkMaterial.color = blinkColor;
        }
      }
    }


    private static IntPtr _hookId = IntPtr.Zero;
    private static void HookWindowsKeyboard()
    {
#if UNITY_STANDALONE_WIN
      using (Process curProcess = Process.GetCurrentProcess())
      using (ProcessModule curModule = curProcess.MainModule)
      {
        _hookId = SetWindowsHookEx(WH_KEYBOARD_LL, HookCallback, IntPtr.Zero, 0);
      }
#endif
    }
    private static void UnhookWindowsKeyboard()
    {
#if UNITY_STANDALONE_WIN
      UnhookWindowsHookEx(_hookId);
#endif
    }
#if UNITY_STANDALONE_WIN
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0)
      {
        var key = KbdKeyHelp.GetFromOsCode((uint)Marshal.ReadInt32(lParam));
        if (key != KbdKey.None)
        {
          _focusTextbox.HandleKeystroke(key, wParam == (IntPtr)WM_KEYDOWN);
        }
      }
      return IntPtr.Zero + 1; // We're eating keystrokes while focused on this textbox
    }
    // PInvoke
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
#endif

    private void OnGUI()
    {
      if (this != _focusTextbox) { return; }
      var e = Event.current;
      if (e.isKey)
      {
        HandleKeystroke(KbdKeyHelp.GetFromUnityCode(e.keyCode), e.type == EventType.KeyDown);
      }
    }

    private bool _isLeftShiftDown = false;
    private bool _isRightShiftDown = false;
    private bool _isLeftControlDown = false;
    private bool _isRightControlDown = false;
    private bool _isShiftDown { get { return _isLeftShiftDown || _isRightShiftDown; } }
    private bool _isControlDown { get { return _isLeftControlDown || _isRightControlDown; } }
    private void HandleKeystroke(KbdKey key, bool isDown)
    {
      if (isDown)
      {
        if (_isControlDown && key == KbdKey.Key_V)
        {
          Paste();
        }
        else if (key >= KbdKey.Key_A && key <= KbdKey.Key_Z)
        {
          Text += _isShiftDown ? key.GetTitle() : key.GetTitle().ToLower();
        }
        else if (!_isShiftDown && key >= KbdKey.Key_0 && key <= KbdKey.Key_9)
        {
          Text += key.GetTitle();
        }
        else if (key >= KbdKey.Numpad_0 && key <= KbdKey.Numpad_9)
        {
          Text += key.GetTitle().Substring(0, 1);
        }
        else
        {
          if (!_isShiftDown)
          {
            switch (key)
            {
              case KbdKey.Backspace:
                if (Text.Length > 0)
                {
                  Text = Text.Substring(0, Text.Length - 1);
                }
                break;
              case KbdKey.Escape:
                Text = _focusTextboxInitialText;
                OnControlClicked(null);
                break;
              case KbdKey.Enter:
                OnControlClicked(null);
                break;
              case KbdKey.Left_Shift: _isLeftShiftDown = true; break;
              case KbdKey.Right_Shift: _isRightShiftDown = true; break;
              case KbdKey.Left_Control: _isLeftControlDown = true; break;
              case KbdKey.Right_Control: _isRightControlDown = true; break;
              case KbdKey.Space: Text += " "; break;
              case KbdKey.Equals: Text += "="; break;
              case KbdKey.Left_Bracket: Text += "["; break;
              case KbdKey.Right_Bracket: Text += "]"; break;
              case KbdKey.BackSlash: Text += "\\"; break;
              case KbdKey.Semicolon: Text += ";"; break;
              case KbdKey.Apostrophe: Text += "'"; break;
              case KbdKey.Comma: Text += ","; break;
              case KbdKey.BackQuote: Text += "`"; break;
              case KbdKey.Slash: Text += "/"; break;
              case KbdKey.Numpad_Slash: Text += "/"; break;
              case KbdKey.Numpad_Asterisk: Text += "*"; break;
              case KbdKey.Minus: Text += "-"; break;
              case KbdKey.Numpad_Minus: Text += "-"; break;
              case KbdKey.Period: Text += "."; break;
              case KbdKey.Numpad_Period: Text += "."; break;
            }
          }
          else
          {
            switch (key)
            {
              case KbdKey.Backspace:
                if (Text.Length > 0)
                {
                  Text = Text.Substring(0, Text.Length - 1);
                }
                break;
              case KbdKey.Escape:
                Text = _focusTextboxInitialText;
                OnControlClicked(null);
                break;
              case KbdKey.Enter:
                OnControlClicked(null);
                break;
              case KbdKey.Key_0: Text += ")"; break;
              case KbdKey.Key_1: Text += "!"; break;
              case KbdKey.Key_2: Text += "@"; break;
              case KbdKey.Key_3: Text += "#"; break;
              case KbdKey.Key_4: Text += "$"; break;
              case KbdKey.Key_5: Text += "%"; break;
              case KbdKey.Key_6: Text += "^"; break;
              case KbdKey.Key_7: Text += "&"; break;
              case KbdKey.Key_8: Text += "*"; break;
              case KbdKey.Key_9: Text += "("; break;
              case KbdKey.Minus: Text += "_"; break;
              case KbdKey.Equals: Text += "+"; break;
              case KbdKey.Left_Bracket: Text += "{"; break;
              case KbdKey.Right_Bracket: Text += "}"; break;
              case KbdKey.BackSlash: Text += "|"; break;
              case KbdKey.Semicolon: Text += ":"; break;
              case KbdKey.Apostrophe: Text += "\""; break;
              case KbdKey.Comma: Text += "<"; break;
              case KbdKey.Period: Text += ">"; break;
              case KbdKey.Slash: Text += "?"; break;
              case KbdKey.BackQuote: Text += "~"; break;
            }
          }
        }
      }
      else
      {
        if (key == KbdKey.Left_Shift) { _isLeftShiftDown = false; }
        if (key == KbdKey.Right_Shift) { _isRightShiftDown = false; }
        if (key == KbdKey.Left_Control) { _isLeftControlDown = false; }
        if (key == KbdKey.Right_Control) { _isRightControlDown = false; }
      }
    }
  }
}
