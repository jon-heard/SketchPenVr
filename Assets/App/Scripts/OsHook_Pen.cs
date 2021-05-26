using System;
using System.Runtime.InteropServices;

public static class OsHook_Pen
{
  public static void Init()
  {
UnityEngine.Debug.Log("Initializing");
#if UNITY_STANDALONE_WIN
    _penHandle = CreateSyntheticPointerDevice(PT_PEN, 1, POINTER_FEEDBACK_DEFAULT);
    _info.type = PT_PEN;
    _info.i.penInfo.penMask = PEN_MASK.PRESSURE | PEN_MASK.ROTATION | PEN_MASK.TILT_X | PEN_MASK.TILT_Y;
    _info.i.penInfo.pointerInfo.pointerType = PT_PEN;
    _info.i.penInfo.pointerInfo.pointerId = 0;
    _info.i.penInfo.pointerInfo.frameId = 0;
    _info.i.penInfo.pointerInfo.sourceDevice = _penHandle;
    _info.i.penInfo.pointerInfo.hwndTarget = IntPtr.Zero;
    _info.i.penInfo.pointerInfo.dwTime = 0;
    _info.i.penInfo.pointerInfo.historyCount = 1;
    _info.i.penInfo.pointerInfo.InputData = 0;
    _info.i.penInfo.pointerInfo.dwKeyStates = 0;
    _info.i.penInfo.pointerInfo.PerformanceCount = 0;
#endif
  }

  public static void Shutdown()
  {
UnityEngine.Debug.Log("shutting down");
#if UNITY_STANDALONE_WIN
    DestroySyntheticPointerDevice(_penHandle);
#endif
  }

  public static void SetState(
    uint x, uint y, float pressure, uint rotation, int tiltx, int tilty, bool usingEraser)
  {
#if UNITY_STANDALONE_WIN
    // Windows requires using and not-using eraser to be in separate states
    if (_prevUsingEraser != usingEraser) { ClearState(); }

    _info.i.penInfo.pointerInfo.ptPixelLocation.x =
      _info.i.penInfo.pointerInfo.ptHimetricLocation.x =
      _info.i.penInfo.pointerInfo.ptPixelLocationRaw.x =
      _info.i.penInfo.pointerInfo.ptHimetricLocationRaw.x = (int)x;
    _info.i.penInfo.pointerInfo.ptPixelLocation.y =
      _info.i.penInfo.pointerInfo.ptHimetricLocation.y =
      _info.i.penInfo.pointerInfo.ptPixelLocationRaw.y =
      _info.i.penInfo.pointerInfo.ptHimetricLocationRaw.y = (int)y;
    _info.i.penInfo.pressure = (uint)(pressure * 1024); // 0 to 1024
    _info.i.penInfo.rotation = rotation; // 0 to 359
    _info.i.penInfo.tiltX = tiltx; // -90 to 90
    _info.i.penInfo.tiltY = tilty; // -90 to 90
    _info.i.penInfo.penFlags = usingEraser ? PEN_FLAGS.INVERTED : PEN_FLAGS.NONE;

    var inContact = (pressure > 0.0f);

    _info.i.penInfo.pointerInfo.pointerFlags =
      POINTER_FLAGS.INRANGE | POINTER_FLAGS.CONFIDENCE |
      (_isNewInteraction ? POINTER_FLAGS.NEW : 0) |
      (inContact ? POINTER_FLAGS.INCONTACT | POINTER_FLAGS.FIRSTBUTTON : 0) |
      (inContact && !_prevInContact ? POINTER_FLAGS.DOWN : 0) |
      (!inContact && _prevInContact ? POINTER_FLAGS.UP : 0);
    _info.i.penInfo.pointerInfo.ButtonChangeType =
      (inContact && !_prevInContact) ? POINTER_BUTTON_CHANGE_TYPE.FIRSTBUTTON_DOWN :
      (!inContact && _prevInContact) ? POINTER_BUTTON_CHANGE_TYPE.FIRSTBUTTON_UP :
      POINTER_BUTTON_CHANGE_TYPE.NONE;

UnityEngine.Debug.Log("Pen state: " + _info.i.penInfo.pressure + " :: " + _info.i.penInfo.penFlags + " :: " +  _info.i.penInfo.penMask);
    InjectSyntheticPointerInput(_penHandle, ref _info, 1);

    _prevInContact = inContact;
    _prevUsingEraser = usingEraser;
    _isNewInteraction = false;
#endif
  }

  public static void ClearState()
  {
UnityEngine.Debug.Log("Clear state");
#if UNITY_STANDALONE_WIN
    _info.i.penInfo.pressure = _info.i.penInfo.rotation = 0;
    _info.i.penInfo.tiltX = _info.i.penInfo.tiltY = 0;
    _info.i.penInfo.penFlags = PEN_FLAGS.NONE;
    _info.i.penInfo.pointerInfo.pointerFlags = POINTER_FLAGS.NONE;
    _info.i.penInfo.pointerInfo.ButtonChangeType =
      (_prevInContact) ? POINTER_BUTTON_CHANGE_TYPE.FIRSTBUTTON_UP :
      POINTER_BUTTON_CHANGE_TYPE.NONE;

    InjectSyntheticPointerInput(_penHandle, ref _info, 1);

    _prevInContact = false;
    _isNewInteraction = true;
#endif
  }



#if UNITY_STANDALONE_WIN
  // Static fields (should be singleton?)
  private static long _penHandle;
  private static POINTER_TYPE_INFO _info;
  private static bool _prevInContact;
  private static bool _prevUsingEraser;
  private static bool _isNewInteraction;


  [DllImport("User32.dll")]
  private static extern long CreateSyntheticPointerDevice(
    uint pointerType, ulong maxCount, uint mode);
  [DllImport("User32.dll")]
  private static extern void DestroySyntheticPointerDevice(long handle);
  [DllImport("User32.dll")]
  private static extern void InjectSyntheticPointerInput(
    long device, ref POINTER_TYPE_INFO pointerInfo, UInt32 count);

  private const uint PT_PEN = 3;
  private const uint POINTER_FEEDBACK_DEFAULT = 1;
  [StructLayout(LayoutKind.Sequential)]
  private struct POINT
  {
    public int x;
    public int y;
  }
  [StructLayout(LayoutKind.Sequential)]
  private struct POINTER_TYPE_INFO
  {
    public uint type;
    public Union_PointerTypeInfo i;
  }
  [StructLayout(LayoutKind.Explicit)]
  private struct Union_PointerTypeInfo
  {
    [FieldOffset(0)]
    public POINTER_PEN_INFO penInfo;
    // Need this other type to keep structure size proper
    [FieldOffset(0)]
    public POINTER_TOUCH_INFO touchInfo;
  }
  [StructLayout(LayoutKind.Sequential)]
  private struct POINTER_PEN_INFO
  {
    public POINTER_INFO pointerInfo;
    public PEN_FLAGS penFlags;
    public PEN_MASK penMask;
    public uint pressure;
    public uint rotation;
    public int tiltX;
    public int tiltY;
  }
  [Flags]
  private enum PEN_FLAGS
  {
    NONE = 0x00000000,
    BARREL = 0x00000001,
    INVERTED = 0x00000002,
    ERASER = 0x00000004
  }
  [Flags]
  private enum PEN_MASK
  {
    NONE = 0x00000000,
    PRESSURE = 0x00000001,
    ROTATION = 0x00000002,
    TILT_X = 0x00000004,
    TILT_Y = 0x00000008
  }
  [StructLayout(LayoutKind.Sequential)]
  private struct POINTER_INFO
  {
    public uint pointerType;
    public int pointerId;
    public int frameId;
    public POINTER_FLAGS pointerFlags;
    public long sourceDevice;
    public IntPtr hwndTarget;
    public POINT ptPixelLocation;
    public POINT ptHimetricLocation;
    public POINT ptPixelLocationRaw;
    public POINT ptHimetricLocationRaw;
    public uint dwTime;
    public int historyCount;
    public int InputData;
    public uint dwKeyStates;
    public UInt64 PerformanceCount;
    public POINTER_BUTTON_CHANGE_TYPE ButtonChangeType;
  }
  [Flags]
  private enum POINTER_FLAGS
  {
    NONE = 0x00000000,
    NEW = 0x00000001,
    INRANGE = 0x00000002,
    INCONTACT = 0x00000004,
    FIRSTBUTTON = 0x00000010,
    SECONDBUTTON = 0x00000020,
    THIRDBUTTON = 0x00000040,
    FOURTHBUTTON = 0x00000080,
    FIFTHBUTTON = 0x00000100,
    PRIMARY = 0x00002000,
    CONFIDENCE = 0x000004000,
    CANCELED = 0x000008000,
    DOWN = 0x00010000,
    UPDATE = 0x00020000,
    UP = 0x00040000,
    WHEEL = 0x00080000,
    HWHEEL = 0x00100000,
    CAPTURECHANGED = 0x00200000,
    HASTRANSFORM = 0x00400000
  }
  private enum POINTER_BUTTON_CHANGE_TYPE
  {
    NONE,
    FIRSTBUTTON_DOWN,
    FIRSTBUTTON_UP,
    SECONDBUTTON_DOWN,
    SECONDBUTTON_UP,
    THIRDBUTTON_DOWN,
    THIRDBUTTON_UP,
    FOURTHBUTTON_DOWN,
    FOURTHBUTTON_UP,
    FIFTHBUTTON_DOWN,
    FIFTHBUTTON_UP
  }
  [StructLayout(LayoutKind.Sequential)]
  private struct POINTER_TOUCH_INFO
  {
    POINTER_INFO pointerInfo;
    PEN_FLAGS touchFlags; // Incorrect type, but we're just using this type to keep structure size
    PEN_MASK touchMask; // Incorrect type, but we're just using this type to keep structure size
    RECT rcContact;
    RECT rcContactRaw;
    UInt32 orientation;
    UInt32 pressure;
  }
  [StructLayout(LayoutKind.Sequential)]
  struct RECT
  {
    long left;
    long top;
    long right;
    long bottom;
  }
}
#endif
