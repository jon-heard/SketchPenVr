using System;
using System.Runtime.InteropServices;

public static class OsHook_Keyboard
{
  public static bool IsKeyDown(Key key)
  {
#if UNITY_STANDALONE_WIN
    return GetAsyncKeyState((int)key) != 0;
#endif
  }

  public static void SetKeyState(Key key, bool isDown)
  {
#if UNITY_STANDALONE_WIN
    var input = new INPUT();
    input.Type = INPUT_TYPE_KEYBOARD;
    input.Data.Keyboard.Vk = (ushort)key;
    input.Data.Keyboard.Flags = isDown ? KEYEVENT_KEYDOWN : KEYEVENT_KEYUP;
    SendInput(1, ref input, Marshal.SizeOf(typeof(INPUT)));
#endif
  }

  public enum Key
  {
    // Basic keys
    Key_0 = 0x30,
    Key_1 = 0x31,
    Key_2 = 0x32,
    Key_3 = 0x33,
    Key_4 = 0x34,
    Key_5 = 0x35,
    Key_6 = 0x36,
    Key_7 = 0x37,
    Key_8 = 0x38,
    Key_9 = 0x39,
    Key_A = 0x41,
    Key_B = 0x42,
    Key_C = 0x43,
    Key_D = 0x44,
    Key_E = 0x45,
    Key_F = 0x46,
    Key_G = 0x47,
    Key_H = 0x48,
    Key_I = 0x49,
    Key_J = 0x4a,
    Key_K = 0x4b,
    Key_L = 0x4c,
    Key_M = 0x4d,
    Key_N = 0x4e,
    Key_O = 0x4f,
    Key_P = 0x50,
    Key_Q = 0x51,
    Key_R = 0x52,
    Key_S = 0x53,
    Key_T = 0x54,
    Key_U = 0x55,
    Key_V = 0x56,
    Key_W = 0x57,
    Key_X = 0x58,
    Key_Y = 0x59,
    Key_Z = 0x5a,

    // Modifiers
    Control = 0x11,
    Alt = 0x12,
    Shift = 0x10,

    // Common keys
    Space = 0x20,
    Down = 40,
    Left = 0x25,
    Right = 0x27,
    Up = 0x26,
    Escape = 0x1b,
    Enter = 13,

    // Function keys
    F1 = 0x70,
    F2 = 0x71,
    F3 = 0x72,
    F4 = 0x73,
    F5 = 0x74,
    F6 = 0x75,
    F7 = 0x76,
    F8 = 0x77,
    F9 = 120,
    F10 = 0x79,
    F11 = 0x7a,
    F12 = 0x7b,

    // Other keys
    Capslock = 20,
    Backspace = 8,
    Dash = 0x6d,
    Delete = 0x2e,
    Slash = 0x6f,
    End = 0x23,
    Home = 0x24,
    Insert = 0x2d,
    Left_Control = 0xa2,
    Left_Shift = 160,
    Left_Windows = 0x5b,
    Numlock = 0x90,
    Numpad0 = 0x60,
    Numpad1 = 0x61,
    Numpad2 = 0x62,
    Numpad3 = 0x63,
    Numpad4 = 100,
    Numpad5 = 0x65,
    Numpad6 = 0x66,
    Numpad7 = 0x67,
    Numpad8 = 0x68,
    Numpad9 = 0x69,
    Pageup = 0x21,
    Pagedown = 0x22,
    Period = 110,
    Plus = 0x6b,
    Right_Control = 0xa3,
    Right_Shift = 0xa1,
    Right_Windows = 0x5c,
    Star = 0x6a,
    Tab = 9,
    OTHER_KEY = 0x0,
  }

#if UNITY_STANDALONE_WIN
  [DllImport("User32.dll")]
  private static extern short GetAsyncKeyState(int vkKey);

  [DllImport("user32.dll")]
  private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

  private const uint INPUT_TYPE_KEYBOARD = 1;
  private const uint KEYEVENT_KEYDOWN = 0x0000;
  private const uint KEYEVENT_KEYUP = 0x0002;
  [StructLayout(LayoutKind.Sequential)]
  private struct INPUT
  {
    public uint Type;
    public Union_InputType Data;
  }
  [StructLayout(LayoutKind.Explicit)]
  private struct Union_InputType
  {
    [FieldOffset(0)]
    public KEYBDINPUT Keyboard;
    [FieldOffset(0)]
    public MOUSEINPUT Mouse;
  }
  [StructLayout(LayoutKind.Sequential)]
  private struct KEYBDINPUT
  {
    public ushort Vk;
    public ushort Scan;
    public uint Flags;
    public uint Time;
    public IntPtr ExtraInfo;
  }
  [StructLayout(LayoutKind.Sequential)]
  private struct MOUSEINPUT
  {
    public int X;
    public int Y;
    public uint MouseData;
    public uint Flags;
    public uint Time;
    public IntPtr ExtraInfo;
  }
#endif
}
