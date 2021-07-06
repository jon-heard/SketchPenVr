
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
  [System.AttributeUsage(System.AttributeTargets.Field)]
  public class KbdKeyAttribute : System.Attribute
  {
    public uint OsCode { get; private set; }
    public KeyCode UnityCode { get; private set; }
    public string Title { get; private set; }
    public KbdKeyAttribute(uint osCode, KeyCode unityCode, string title)
    {
      OsCode = osCode;
      UnityCode = unityCode;
      Title = title;
    }
  }

  public static class KbdKeyHelp
  {
    // Setup dictionaries for efficient access
    static KbdKeyHelp()
    {
      var keys = Enum.GetValues(typeof(KbdKey));
      foreach (KbdKey key in keys)
      {
        var attributes = (KbdKeyAttribute[])key.GetType().GetField(key.ToString()).GetCustomAttributes(typeof(KbdKeyAttribute), false);
        if (attributes.Length > 0)
        {
          // Attribute 
          _keyToAttribute[key] = attributes[0];

          // from os code
          _osCodeToKey[attributes[0].OsCode] = key;

          // from unity code
          _unityCodeToKey[attributes[0].UnityCode] = key;
        }
      }
    }

    public static List<string> GetTitleList()
    {
      var result = new List<string>();
      foreach (KbdKey key in Enum.GetValues(typeof(KbdKey)))
      {
        result.Add(key.GetTitle());
      }
      return result;
    }

    public static KbdKey GetFromOsCode(uint code)
    {
      try
      {
        return _osCodeToKey[code];
      }
      catch
      {
        return KbdKey.None;
      }
    }

    public static KbdKey GetFromUnityCode(KeyCode code)
    {
      try
      {
        return _unityCodeToKey[code];
      }
      catch
      {
        return KbdKey.None;
      }
    }

    public static uint GetOsCode(this KbdKey value)
    {
      try
      {
        return _keyToAttribute[value].OsCode;
      }
      catch
      {
        return 0;
      }
    }

    public static KeyCode GetUnityCode(this KbdKey value)
    {
      try
      {
        return _keyToAttribute[value].UnityCode;
      }
      catch
      {
        return KeyCode.None;
      }
    }
    public static string GetTitle(this KbdKey value)
    {
      try
      {
        return _keyToAttribute[value].Title;
      }
      catch
      {
        return value.ToString();
      }
    }

    private static Dictionary<KbdKey, KbdKeyAttribute> _keyToAttribute =
      new Dictionary<KbdKey, KbdKeyAttribute>();
    private static Dictionary<uint, KbdKey> _osCodeToKey = new Dictionary<uint, KbdKey>();
    private static Dictionary<KeyCode, KbdKey> _unityCodeToKey = new Dictionary<KeyCode, KbdKey>();
  }

  public enum KbdKey
  {
#if UNITY_STANDALONE_WIN
    [KbdKey(0x00, KeyCode.None, "None")] None,

    // Modifiers
    [KbdKey(0x10, KeyCode.LeftShift, "Shift")]     Shift,
    [KbdKey(0x11, KeyCode.LeftControl, "Control")] Control,
    [KbdKey(0x12, KeyCode.LeftAlt, "Alt")]         Alt,

    // Arrow keys
    [KbdKey(0x26, KeyCode.UpArrow, "Up")]       Up,
    [KbdKey(0x28, KeyCode.DownArrow, "Down")]   Down,
    [KbdKey(0x25, KeyCode.LeftArrow, "Left")]   Left,
    [KbdKey(0x27, KeyCode.RightArrow, "Right")] Right,

    // Common keys
    [KbdKey(0x08, KeyCode.Backspace, "Backspace")] Backspace,
    [KbdKey(0x2e, KeyCode.Delete, "Delete")]       Delete,
    [KbdKey(0x0d, KeyCode.Return, "Enter")]        Enter,
    [KbdKey(0x1b, KeyCode.Escape, "Escape")]       Escape,
    [KbdKey(0x20, KeyCode.Space, "Space")]         Space,

    // Basic keys
    [KbdKey(0x30, KeyCode.Alpha0, "0")] Key_0,
    [KbdKey(0x31, KeyCode.Alpha1, "1")] Key_1,
    [KbdKey(0x32, KeyCode.Alpha2, "2")] Key_2,
    [KbdKey(0x33, KeyCode.Alpha3, "3")] Key_3,
    [KbdKey(0x34, KeyCode.Alpha4, "4")] Key_4,
    [KbdKey(0x35, KeyCode.Alpha5, "5")] Key_5,
    [KbdKey(0x36, KeyCode.Alpha6, "6")] Key_6,
    [KbdKey(0x37, KeyCode.Alpha7, "7")] Key_7,
    [KbdKey(0x38, KeyCode.Alpha8, "8")] Key_8,
    [KbdKey(0x39, KeyCode.Alpha9, "9")] Key_9,
    [KbdKey(0x41, KeyCode.A, "A")] Key_A,
    [KbdKey(0x42, KeyCode.B, "B")] Key_B,
    [KbdKey(0x43, KeyCode.C, "C")] Key_C,
    [KbdKey(0x44, KeyCode.D, "D")] Key_D,
    [KbdKey(0x45, KeyCode.E, "E")] Key_E,
    [KbdKey(0x46, KeyCode.F, "F")] Key_F,
    [KbdKey(0x47, KeyCode.G, "G")] Key_G,
    [KbdKey(0x48, KeyCode.H, "H")] Key_H,
    [KbdKey(0x49, KeyCode.I, "I")] Key_I,
    [KbdKey(0x4a, KeyCode.J, "J")] Key_J,
    [KbdKey(0x4b, KeyCode.K, "K")] Key_K,
    [KbdKey(0x4c, KeyCode.L, "L")] Key_L,
    [KbdKey(0x4d, KeyCode.M, "M")] Key_M,
    [KbdKey(0x4e, KeyCode.N, "N")] Key_N,
    [KbdKey(0x4f, KeyCode.O, "O")] Key_O,
    [KbdKey(0x50, KeyCode.P, "P")] Key_P,
    [KbdKey(0x51, KeyCode.Q, "Q")] Key_Q,
    [KbdKey(0x52, KeyCode.R, "R")] Key_R,
    [KbdKey(0x53, KeyCode.S, "S")] Key_S,
    [KbdKey(0x54, KeyCode.T, "T")] Key_T,
    [KbdKey(0x55, KeyCode.U, "U")] Key_U,
    [KbdKey(0x56, KeyCode.V, "V")] Key_V,
    [KbdKey(0x57, KeyCode.W, "W")] Key_W,
    [KbdKey(0x58, KeyCode.X, "X")] Key_X,
    [KbdKey(0x59, KeyCode.Y, "Y")] Key_Y,
    [KbdKey(0x5a, KeyCode.Z, "Z")] Key_Z,

    // Function keys
    [KbdKey(0x70, KeyCode.F1, "F1")]   F1,
    [KbdKey(0x71, KeyCode.F2, "F2")]   F2,
    [KbdKey(0x72, KeyCode.F3, "F3")]   F3,
    [KbdKey(0x73, KeyCode.F4, "F4")]   F4,
    [KbdKey(0x74, KeyCode.F5, "F5")]   F5,
    [KbdKey(0x75, KeyCode.F6, "F6")]   F6,
    [KbdKey(0x76, KeyCode.F7, "F7")]   F7,
    [KbdKey(0x77, KeyCode.F8, "F8")]   F8,
    [KbdKey(0x78, KeyCode.F9, "F9")]   F9,
    [KbdKey(0x79, KeyCode.F10, "F10")] F10,
    [KbdKey(0x7a, KeyCode.F11, "F11")] F11,
    [KbdKey(0x7b, KeyCode.F12, "F12")] F12,

    // Numpad keys
    [KbdKey(0x60, KeyCode.Keypad0, "0 (numpad)")] Numpad0,
    [KbdKey(0x61, KeyCode.Keypad1, "1 (numpad)")] Numpad1,
    [KbdKey(0x62, KeyCode.Keypad2, "2 (numpad)")] Numpad2,
    [KbdKey(0x63, KeyCode.Keypad3, "3 (numpad)")] Numpad3,
    [KbdKey(0x64, KeyCode.Keypad4, "4 (numpad)")] Numpad4,
    [KbdKey(0x65, KeyCode.Keypad5, "5 (numpad)")] Numpad5,
    [KbdKey(0x66, KeyCode.Keypad6, "6 (numpad)")] Numpad6,
    [KbdKey(0x67, KeyCode.Keypad7, "7 (numpad)")] Numpad7,
    [KbdKey(0x68, KeyCode.Keypad8, "8 (numpad)")] Numpad8,
    [KbdKey(0x69, KeyCode.Keypad9, "9 (numpad)")] Numpad9,

    // Other keys
    [KbdKey(0xde, KeyCode.Quote, "Apostrophe")]             Apostrophe,
    [KbdKey(0x6a, KeyCode.KeypadMultiply, "Asterisk")]      Asterisk,
    [KbdKey(0xc0, KeyCode.BackQuote, "Backquote")]          BackQuote,
    [KbdKey(0xdb, KeyCode.LeftBracket, "Bracket (left)")]   Left_Bracket,
    [KbdKey(0xdd, KeyCode.RightBracket, "Bracket (right)")] Right_Bracket,
    [KbdKey(0x14, KeyCode.CapsLock, "Caps lock")]           Capslock,
    [KbdKey(0xbc, KeyCode.Comma, "Comma")]                  Comma,
    [KbdKey(0xa2, KeyCode.LeftControl, "Control (left)")]   Left_Control,
    [KbdKey(0xa3, KeyCode.RightControl, "Control (right)")] Right_Control,
    [KbdKey(0x23, KeyCode.End, "End")]                      End,
    [KbdKey(0xbb, KeyCode.Equals, "Equals")]                Equals,
    [KbdKey(0x24, KeyCode.Home, "Home")]                    Home,
    [KbdKey(0x2d, KeyCode.Insert, "Insert")]                Insert,
    [KbdKey(0x5d, KeyCode.Menu, "Menu")]                    Menu,
    [KbdKey(0xbd, KeyCode.Minus, "Minus")]                  Minus,
    [KbdKey(0x6d, KeyCode.KeypadMinus, "Minus (numpad)")]   NumpadMinus,
    [KbdKey(0x90, KeyCode.Numlock, "Num lock")]             Numlock,
    [KbdKey(0x22, KeyCode.PageDown, "Page down")]           Pagedown,
    [KbdKey(0x21, KeyCode.PageUp, "Page up")]               Pageup,
    [KbdKey(0xbe, KeyCode.Period, "Period")]                Period,
    [KbdKey(0x6e, KeyCode.KeypadPeriod, "Period (numpad)")] NumpadPeriod,
    [KbdKey(0x6b, KeyCode.KeypadPlus, "Plus (numpad)")]     Plus,
    [KbdKey(0xba, KeyCode.Semicolon, "Semicolon")]          Semicolon,
    [KbdKey(0xa0, KeyCode.LeftShift, "Shift (left)")]       Left_Shift,
    [KbdKey(0xa1, KeyCode.RightShift, "Shift (right)")]     Right_Shift,
    [KbdKey(0xbf, KeyCode.Slash, "Slash (forward)")]        Slash,
    [KbdKey(0x6f, KeyCode.KeypadDivide, "Slash (numpad)")]  NumpadSlash,
    [KbdKey(0xdc, KeyCode.Backslash, "Slash (back)")]       BackSlash,
    [KbdKey(0x09, KeyCode.Tab, "Tab")]                      Tab,
    [KbdKey(0x5b, KeyCode.LeftWindows, "Windows (left)")]   Left_Windows,
    [KbdKey(0x5c, KeyCode.RightWindows, "Windows (right)")] Right_Windows,
#endif
  }
}
