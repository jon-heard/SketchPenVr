using Common;
using System;
using System.Runtime.InteropServices;

public static class OsHook_Keyboard
{
  public static bool IsKeyDown(KbdKey key)
  {
#if UNITY_STANDALONE_WIN
    return GetAsyncKeyState((int)key.GetOsCode()) != 0;
#endif
  }

  public static void SetKeyState(KbdKey key, bool isDown)
  {
#if UNITY_STANDALONE_WIN
    var input = new INPUT();
    input.Type = INPUT_TYPE_KEYBOARD;
    input.Data.Keyboard.Vk = (ushort)key.GetOsCode();
    input.Data.Keyboard.Flags = isDown ? KEYEVENT_KEYDOWN : KEYEVENT_KEYUP;
    SendInput(1, ref input, Marshal.SizeOf(typeof(INPUT)));
#endif
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
