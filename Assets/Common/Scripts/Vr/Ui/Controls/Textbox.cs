using Common;
using System.Collections;
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
            Geometry.transform.localScale.x - App_Details.Instance.VrUiDetails.TEXTBOX_TEXT_X_OFFSET)
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
    private float _blinkSpeed;
    private Material _blinkMaterial;

    private static Textbox _focusTextbox;

    protected override void Awake()
    {
      base.Awake();
      Label.anchor = TextAnchor.MiddleLeft;
      var p = Label.transform.localPosition;
      p.x = -Geometry.transform.localScale.x * 0.5f + App_Details.Instance.VrUiDetails.TEXTBOX_TEXT_X_OFFSET;
      Label.transform.localPosition = p;
      _blinkSpeed = App_Details.Instance.VrUiDetails.TEXTBOX_CARET_BLINKSPEED;
      _blinkMaterial = Label.GetComponent<Renderer>().materials[1];
      Text = "";
    }

    private void Start()
    {
      Control.OnControlClicked += OnClickedEventListener;
    }

    protected void OnClickedEventListener(Control focus)
    {
      if (focus == this)
      {
        _focusTextbox = this;
        StartCoroutine(VrKeyboardCoroutine());
        StartCoroutine(CaretBlinkCoroutine());
      }
      else if (_focusTextbox == this)
      {
        _focusTextbox = null;
        StopAllCoroutines();
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
        if (Time.time - _lastBlink > _blinkSpeed)
        {
          _lastBlink = Time.time;
          var blinkColor = _blinkMaterial.color;
          blinkColor.a = blinkColor.a == 1.0f ? 0.0f : 1.0f;
          _blinkMaterial.color = blinkColor;
        }
      }
    }

    private bool _isLeftShiftDown = false;
    private bool _isRightShiftDown = false;
    private bool _isLeftControlDown = false;
    private bool _isRightControlDown = false;
    private bool _isShiftDown { get { return _isLeftShiftDown || _isRightShiftDown; } }
    private bool _isControlDown { get { return _isLeftControlDown || _isRightControlDown; } }
    private void OnGUI()
    {
      if (this != _focusTextbox) { return; }
      var e = Event.current;
      if (e.isKey)
      {
        var newKey = e.keyCode;
        if (e.type == EventType.KeyDown)
        {
          if (_isControlDown && newKey == KeyCode.V)
          {
            Paste();
          }
          else if (newKey >= KeyCode.A && newKey <= KeyCode.Z)
          {
            Text += _isShiftDown ? newKey.ToString() : newKey.ToString().ToLower();
          }
          else if (!_isShiftDown && newKey >= KeyCode.Alpha0 && newKey <= KeyCode.Alpha9)
          {
            Text += newKey.ToString().Substring(5);
          }
          else if (newKey >= KeyCode.Keypad0 && newKey <= KeyCode.Keypad9)
          {
            Text += newKey.ToString().Substring(6);
          }
          else
          {
            if (!_isShiftDown)
            {
              switch (newKey)
              {
                case KeyCode.Backspace:
                  if (Text.Length > 0)
                  {
                    Text = Text.Substring(0, Text.Length - 1);
                  }
                  break;
                case KeyCode.LeftShift:
                  _isLeftShiftDown = true;
                  break;
                case KeyCode.RightShift:
                  _isRightShiftDown = true;
                  break;
                case KeyCode.LeftControl:
                  _isLeftControlDown = true;
                  break;
                case KeyCode.RightControl:
                  _isRightControlDown = true;
                  break;
                case KeyCode.Space: Text += " "; break;
                case KeyCode.Minus: Text += "-"; break;
                case KeyCode.Equals: Text += "="; break;
                case KeyCode.LeftBracket: Text += "["; break;
                case KeyCode.RightBracket: Text += "]"; break;
                case KeyCode.Backslash: Text += "\\"; break;
                case KeyCode.Semicolon: Text += ";"; break;
                case KeyCode.Quote: Text += "'"; break;
                case KeyCode.Comma: Text += ","; break;
                case KeyCode.Period: Text += "."; break;
                case KeyCode.Slash: Text += "/"; break;
                case KeyCode.BackQuote: Text += "`"; break;
                case KeyCode.KeypadDivide: Text += "/"; break;
                case KeyCode.KeypadMultiply: Text += "*"; break;
                case KeyCode.KeypadMinus: Text += "-"; break;
                case KeyCode.KeypadPeriod: Text += "."; break;
              }
            }
            else
            {
              switch (newKey)
              {
                case KeyCode.Backspace:
                  if (Text.Length > 0)
                  {
                    Text = Text.Substring(0, Text.Length - 1);
                  }
                  break;
                case KeyCode.Alpha0: Text += ")"; break;
                case KeyCode.Alpha1: Text += "!"; break;
                case KeyCode.Alpha2: Text += "@"; break;
                case KeyCode.Alpha3: Text += "#"; break;
                case KeyCode.Alpha4: Text += "$"; break;
                case KeyCode.Alpha5: Text += "%"; break;
                case KeyCode.Alpha6: Text += "^"; break;
                case KeyCode.Alpha7: Text += "&"; break;
                case KeyCode.Alpha8: Text += "*"; break;
                case KeyCode.Alpha9: Text += "("; break;
                case KeyCode.Minus: Text += "_"; break;
                case KeyCode.Equals: Text += "+"; break;
                case KeyCode.LeftBracket: Text += "{"; break;
                case KeyCode.RightBracket: Text += "}"; break;
                case KeyCode.Backslash: Text += "|"; break;
                case KeyCode.Semicolon: Text += ":"; break;
                case KeyCode.Quote: Text += "\""; break;
                case KeyCode.Comma: Text += "<"; break;
                case KeyCode.Period: Text += ">"; break;
                case KeyCode.Slash: Text += "?"; break;
                case KeyCode.BackQuote: Text += "~"; break;
              }
            }
          }
        }
        else
        {
          if (newKey == KeyCode.LeftShift) { _isLeftShiftDown = false; }
          if (newKey == KeyCode.RightShift) { _isRightShiftDown = false; }
          if (newKey == KeyCode.LeftControl) { _isLeftControlDown = false; }
          if (newKey == KeyCode.RightControl) { _isRightControlDown = false; }
        }
      }
    }
  }
}
