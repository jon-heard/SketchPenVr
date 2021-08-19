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

  public static void GetPrimaryScreenOffset(Action<int, int> onResult)
  {
    var xOffset = 0;
    var yOffset = 0;
#if UNITY_STANDALONE_WIN
    EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
      delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
      {
        var offsetChanged = false;
        if (lprcMonitor.left < xOffset)
        {
          xOffset = lprcMonitor.left;
          offsetChanged = true;
        }
        if (lprcMonitor.top < yOffset)
        {
          yOffset = lprcMonitor.top;
          offsetChanged = true;
        }
        if (offsetChanged)
        {
          onResult.Invoke(-xOffset, -yOffset);
        }
        return true;
      }, IntPtr.Zero);
#endif
  }



#if UNITY_STANDALONE_WIN

  [DllImport("User32.dll")]
  private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

  [DllImport("User32.dll")]
  private static extern IntPtr GetActiveWindow();

  [DllImport("user32.dll")]
  static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip,
     EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

  [StructLayout(LayoutKind.Sequential)]
  public struct Rect
  {
    public int left;
    public int top;
    public int right;
    public int bottom;
  }

  delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

  [DllImport("user32.dll")]
  static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

  // size of a device name string
  private const int CCHDEVICENAME = 32;

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal struct MonitorInfoEx
  {
    public int Size;
    public Rect Monitor;
    public Rect WorkArea;
    public uint Flags;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
    public string DeviceName;

    public void Init()
    {
      this.Size = 40 + 2 * CCHDEVICENAME;
      this.DeviceName = string.Empty;
    }
  }

  const uint MONITORINFOF_PRIMARY = 1;

  const int MONITOR_DEFAULTTOPRIMARY = 1;

  [DllImport("user32.dll")]
  static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

#endif
}
