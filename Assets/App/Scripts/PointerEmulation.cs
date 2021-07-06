using UnityEngine;
using System;
using Common;

//////////////////////////////////////////////////////////////////////
// Emulate desktop pointer (mouse and stylus) within screen texture //
//////////////////////////////////////////////////////////////////////
public class PointerEmulation : MonoBehaviour
{
  public Vector2 Resolution = new Vector2(1920, 1080);
  public Renderer ScreenRenderer;

  [NonSerialized] public Vector2 Position;
  [NonSerialized] public float Distance;

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

  // Moues - left button
  public bool MouseLeftButton
  {
    get { return _mouseLeftButton; }
    set
    {
      if (!IsEmulating) { return; }
      if (value == _mouseLeftButton) { return; }
      _mouseLeftButton = value;
      StartCoroutine(SetMouseButton(OsHook_Mouse.Button.Left, value));
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
      StartCoroutine(SetMouseButton(OsHook_Mouse.Button.Right, value));
    }
  }
  private bool _mouseRightButton;

  private System.Collections.IEnumerator SetMouseButton(OsHook_Mouse.Button button, bool down)
  {
    if (_mouseLeftButton || _mouseRightButton)
    {
      _isEmulatingMouse = true;
      ClearPenState();
      yield return new WaitForEndOfFrame();
      UpdateMousePosition();
      OsHook_Mouse.SetButton(button, down);
    }
    else
    {
      UpdateMousePosition();
      OsHook_Mouse.SetButton(button, down);
      yield return new WaitForEndOfFrame();
      _isEmulatingMouse = false;
    }
  }

  public void OnLostFocus()
  {
    MouseLeftButton = MouseRightButton = false;
  }

  public void SetPenState(float pressure, uint rotation, Vector2 tilt, bool usingEraser)
  {
    if (!IsEmulating || _isEmulatingMouse) { return; }

    var p = (Position * Resolution).ToIntVector();
    var t = tilt.ToIntVector();

    OsHook_Pen.SetState((uint)p.x, (uint)p.y, pressure, rotation, t.x, t.y, usingEraser);

    // Setup the pen shadow
    var size = (Distance - _maxNearDistance * 0.5f) * 2.0f;
    var opacity = (size < .001f) ? 0.0f : (1.25f - size * 7.0f);
    ScreenRenderer.material.SetVector(
      "_ShadowState", new Vector4(Position.x, Position.y, size, opacity));
  }

  public void ClearPenState()
  {
    OsHook_Pen.ClearState();
  }

  private bool _isEmulatingMouse = false;
  private float _maxNearDistance;

  private void Start()
  {
    OsHook_Pen.Init();
    _maxNearDistance = App_Details.Instance.CONTROLLER_DISTANCE_NEAR_SCREEN;
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
    var p = (Position * Resolution).ToIntVector();
    OsHook_Mouse.SetPosition((uint)p.x, (uint)p.y);
  }
}
