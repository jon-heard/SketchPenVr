using System.Runtime.InteropServices;
using UnityEngine;
using System;

public class PointerEmulation : MonoBehaviour
{
  public Vector2 Resolution = new Vector2(1920, 1080);

  [NonSerialized] public Vector2 Position;

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
  private bool _isEmulating;

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
      IsEmulatingMouse = true;
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
      IsEmulatingMouse = false;
    }
  }

  public void OnLostFocus()
  {
    MouseLeftButton = MouseRightButton = false;
  }

  public void SetPenState(float pressure, uint rotation, Vector2 tilt, bool usingEraser)
  {
    if (!IsEmulating || IsEmulatingMouse) { return; }

    var p = (Position * Resolution).ToIntVector();
    var t = tilt.ToIntVector();

    OsHook_Pen.SetState((uint)p.x, (uint)p.y, pressure, rotation, t.x, t.y, usingEraser);
  }

  public void ClearPenState()
  {
    OsHook_Pen.ClearState();
  }

  private bool IsEmulatingMouse = false;

  private void Start()
  {
    OsHook_Pen.Init();
  }
  private void OnDestroy()
  {
    OsHook_Pen.Shutdown();
  }
  private void Update()
  {
    if (IsEmulatingMouse) { UpdateMousePosition(); }
    if (OsHook_Keyboard.IsKeyDown(OsHook_Keyboard.Key.Escape)) { IsEmulating = false; }
  }

  private void UpdateMousePosition()
  {
    var p = (Position * Resolution).ToIntVector();
    OsHook_Mouse.SetPosition((uint)p.x, (uint)p.y);
  }
}
