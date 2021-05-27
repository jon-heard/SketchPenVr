using System;
using System.Runtime.InteropServices;

public static class OsHook_Window
{
  public static void Minimize()
  {
#if UNITY_STANDALONE_WIN
    ShowWindow(GetActiveWindow(), 2);
#endif
  }

#if UNITY_STANDALONE_WIN

  [DllImport("User32.dll")]
  private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

  [DllImport("User32.dll")]
  private static extern IntPtr GetActiveWindow();
#endif
}
