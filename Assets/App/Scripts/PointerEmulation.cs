using UnityEngine;
using System;
using Common;
using System.Collections;

//////////////////////////////////////////////////////////////////////
// Emulate desktop pointer (mouse and stylus) within screen texture //
//////////////////////////////////////////////////////////////////////
public class PointerEmulation : MonoBehaviour
{
  [NonSerialized] public Vector2Int Resolution;
  [NonSerialized] public float Distance;

  public Vector2 Position
  {
    get { return _position; }
    set
    {
      if (value == _position) { return; }
      _position = value;
      _pixelPosition = (Position * Resolution).ToIntVector();
    }
  }
  private Vector2 _position;
  private Vector2Int _pixelPosition;

  public bool IsEmulating
  {
    get { return _isEmulating; }
    set
    {
      if (value == _isEmulating) { return; }
      _isEmulating = value;
      if (!_isEmulating)
      {
        OnLostFocus();
      }
    }
  }
  private bool _isEmulating = true;

  // Mouse - left button
  public bool MouseLeftButton
  {
    get { return _mouseLeftButton; }
    set
    {
      if (!IsEmulating) { return; }
      if (value == _mouseLeftButton) { return; }
      _mouseLeftButton = value;
      StartCoroutine(SetMouseButtonCoroutine(OsHook_Mouse.Button.Left, value));
    }
  }
  private bool _mouseLeftButton;

  // Mouse - Right button
  public bool MouseRightButton
  {
    get { return _mouseRightButton; }
    set
    {
      if (!IsEmulating) { return; }
      if (value == _mouseRightButton) { return; }
      _mouseRightButton = value;
      StartCoroutine(SetMouseButtonCoroutine(OsHook_Mouse.Button.Right, value));
    }
  }
  private bool _mouseRightButton;

  // Mouse - Middle button
  public bool MouseMiddleButton
  {
    get { return _mouseMiddleButton; }
    set
    {
      if (!IsEmulating) { return; }
      if (value == _mouseMiddleButton) { return; }
      _mouseMiddleButton = value;
      StartCoroutine(SetMouseButtonCoroutine(OsHook_Mouse.Button.Middle, value));
    }
  }
  private bool _mouseMiddleButton;

  private IEnumerator SetMouseButtonCoroutine(OsHook_Mouse.Button button, bool down)
  {
    if (_mouseLeftButton || _mouseRightButton || _mouseMiddleButton)
    {
      _isEmulatingMouse = true;
      ClearPenState();
      yield return new WaitForSeconds(App_Details.Instance.TIMESPAN_POINTER_CHANGEOVER); // "clear" needs a moment to process
      UpdateMousePosition();
      OsHook_Mouse.SetButton(button, down);
    }
    else
    {
      UpdateMousePosition();
      OsHook_Mouse.SetButton(button, down);
      yield return new WaitForSeconds(App_Details.Instance.TIMESPAN_POINTER_CHANGEOVER); // "clear" needs a moment to process
      _isEmulatingMouse = false;
    }
    yield break;
  }

  // Mouse - scroll
  public void DoScroll(bool isVertical, bool isDown)
  {
    StartCoroutine(DoScrollCoroutine(isVertical, isDown));
  }
  private IEnumerator DoScrollCoroutine(bool isVertical, bool isDown)
  {
    _isEmulatingMouse = true;
    ClearPenState();
    yield return new WaitForSeconds(App_Details.Instance.TIMESPAN_POINTER_CHANGEOVER); // "clear" needs a moment to process
    UpdateMousePosition();
    OsHook_Mouse.SetButton(
      isVertical ? OsHook_Mouse.Button.VScroll : OsHook_Mouse.Button.HScroll, isDown);
    yield return new WaitForSeconds(App_Details.Instance.TIMESPAN_POINTER_CHANGEOVER); // "clear" needs a moment to process
    _isEmulatingMouse = false;
  }


  public void OnLostFocus()
  {
    MouseLeftButton = MouseRightButton = MouseMiddleButton = false;
  }

  public void SetPenState(float pressure, uint rotation, Vector2 tilt, bool usingEraser)
  {
    if (!IsEmulating || _isEmulatingMouse) { return; }

    var t = tilt.ToIntVector();

    OsHook_Pen.SetState(
      (uint)_pixelPosition.x, (uint)_pixelPosition.y, pressure, rotation, t.x, t.y, usingEraser);

    // Setup the pen shadow
    var size = (Distance - _const_maxNearDistance * 0.5f) * 2.0f;
    var opacity = (size < .001f) ? 0.0f : (1.25f - size * 7.0f);
    _screenRenderer.material.SetVector(
      "_ShadowState", new Vector4(Position.x, Position.y, size, opacity));
  }

  public void ClearPenState()
  {
    OsHook_Pen.ClearState();
  }

  private bool _isEmulatingMouse = false;
  private float _const_maxNearDistance;
  private Renderer _screenRenderer;

  private void Awake()
  {
    Resolution = new Vector2Int(
      UnityEngine.Screen.currentResolution.width,
      UnityEngine.Screen.currentResolution.height);
  }

  private void Start()
  {
    OsHook_Pen.Init();
    _const_maxNearDistance = App_Details.Instance.CONTROLLER_DISTANCE_NEAR_SCREEN;
    _screenRenderer = App_Functions.Instance.ScreenRenderer;
  }
  private void OnDestroy()
  {
    OsHook_Pen.Shutdown();
  }
  private void Update()
  {
    if (_isEmulatingMouse) { UpdateMousePosition(); }
    if (OsHook_Keyboard.IsKeyDown(KbdKey.Escape)) { IsEmulating = false; }
  }

  private void UpdateMousePosition()
  {
    OsHook_Mouse.SetPosition((uint)_pixelPosition.x, (uint)_pixelPosition.y);
  }
}
