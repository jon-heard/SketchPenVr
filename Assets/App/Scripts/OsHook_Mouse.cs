﻿using System;
using System.Runtime.InteropServices;

public static class OsHook_Mouse
{
  public static void SetPosition(uint x, uint y)
  {
#if UNITY_STANDALONE_WIN
    SetCursorPos((int)x, (int)y);
#endif
  }

  public enum Button { Left, Right, Middle }
  public static void SetButton(Button button, bool down)
  {
    var structInput = new INPUT();
    structInput.Type = INPUT_TYPE_MOUSE;
    structInput.Data.Mouse.X = structInput.Data.Mouse.Y = 0;
    switch (button)
    {
      case Button.Left: structInput.Data.Mouse.Flags = down ? MouseEventFlags.LEFTDOWN : MouseEventFlags.LEFTUP; break;
      case Button.Middle: structInput.Data.Mouse.Flags = down ? MouseEventFlags.RIGHTDOWN : MouseEventFlags.RIGHTUP; break;
      case Button.Right: structInput.Data.Mouse.Flags = down ? MouseEventFlags.MIDDLEDOWN : MouseEventFlags.MIDDLEUP; break;
    }
    SendInput(1, ref structInput, Marshal.SizeOf(structInput));
  }

#if UNITY_STANDALONE_WIN
  [DllImport("user32.dll")]
  private static extern bool SetCursorPos(int X, int Y);
  [DllImport("user32.dll")]
  private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

  private const uint INPUT_TYPE_MOUSE = 0;

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
    public MOUSEINPUT Mouse;
  }
  [StructLayout(LayoutKind.Sequential)]
  private struct MOUSEINPUT
  {
    public int X;
    public int Y;
    public uint MouseData;
    public MouseEventFlags Flags;
    public uint Time;
    public IntPtr ExtraInfo;
  }
  [Flags]
  private enum MouseEventFlags
  {
    LEFTDOWN = 0x00000002,
    LEFTUP = 0x00000004,
    MIDDLEDOWN = 0x00000020,
    MIDDLEUP = 0x00000040,
    RIGHTDOWN = 0x00000008,
    RIGHTUP = 0x00000010
  }
#endif
}
