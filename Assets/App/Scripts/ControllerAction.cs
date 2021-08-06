using Common;
using System;
using System.Collections;
using UnityEngine;

public static class ActionTypeExtension
{
  public static bool HasKey(this ControllerAction.ActionType type)
  {
    return
      type == ControllerAction.ActionType.Key_Hit ||
      type == ControllerAction.ActionType.Key_Press;
  }
}

[Serializable]
public class ControllerAction
{
  public enum ActionType
  {
    Nothing,
    Hold_focus,
    Hold_desktop,
    Undo,
    Redo__ctrl___shift___z,
    Redo__ctrl___y,
    Key_Hit,
    Key_Press,
    Mouse_button__left,
    Mouse_button__right,
    Mouse_button__middle,
    Scroll_Up,
    Scroll_Right,
    Scroll_Down,
    Scroll_Left,
    Pencil_flip,
  }

  public ActionType Type;
  public KbdKey Key;
  [SerializeReference]
  public ControllerAction Next;

  public void Run(Controller controller, bool isDown, float value = 0.0f)
  {
    switch (Type)
    {
      case ActionType.Nothing:
        break;
      case ActionType.Hold_focus:
        controller.IsHolding = isDown;
        break;
      case ActionType.Hold_desktop:
        controller.IsHoldingDesktop = isDown;
        break;
      case ActionType.Pencil_flip:
        Controller.IsFlipped = isDown;
        break;
      case ActionType.Mouse_button__left:
        if (controller.FocusPointerEmulation)
        {
          controller.FocusPointerEmulation.MouseLeftButton = isDown;
        }
        break;
      case ActionType.Mouse_button__right:
        if (controller.FocusPointerEmulation)
        {
          controller.FocusPointerEmulation.MouseRightButton = isDown;
        }
        break;
      case ActionType.Mouse_button__middle:
        if (controller.FocusPointerEmulation)
        {
          controller.FocusPointerEmulation.MouseMiddleButton = isDown;
        }
        break;
      case ActionType.Undo:
        if (isDown)
        {
          OsHook_Keyboard.SetKeyState(KbdKey.Control, true);
          OsHook_Keyboard.SetKeyState(KbdKey.Key_Z, true);
          OsHook_Keyboard.SetKeyState(KbdKey.Key_Z, false);
          OsHook_Keyboard.SetKeyState(KbdKey.Control, false);
        }
        break;
      case ActionType.Redo__ctrl___shift___z:
        if (isDown)
        {
          OsHook_Keyboard.SetKeyState(KbdKey.Control, true);
          OsHook_Keyboard.SetKeyState(KbdKey.Shift, true);
          OsHook_Keyboard.SetKeyState(KbdKey.Key_Z, true);
          OsHook_Keyboard.SetKeyState(KbdKey.Key_Z, false);
          OsHook_Keyboard.SetKeyState(KbdKey.Shift, false);
          OsHook_Keyboard.SetKeyState(KbdKey.Control, false);
        }
        break;
      case ActionType.Redo__ctrl___y:
        if (isDown)
        {
          OsHook_Keyboard.SetKeyState(KbdKey.Control, true);
          OsHook_Keyboard.SetKeyState(KbdKey.Key_Y, true);
          OsHook_Keyboard.SetKeyState(KbdKey.Key_Y, false);
          OsHook_Keyboard.SetKeyState(KbdKey.Control, false);
        }
        break;
      case ActionType.Scroll_Up:
        _value = value;
        if (!_runningCoroutine && value > 0.0f)
        {
          if (_app_functions == null) { _app_functions = App_Functions.Instance; }
          _app_functions.StartCoroutine(RunScrolling(controller, true, false));
        }
        break;
      case ActionType.Scroll_Right:
        _value = value;
        if (!_runningCoroutine && value > 0.0f)
        {
          if (_app_functions == null) { _app_functions = App_Functions.Instance; }
          _app_functions.StartCoroutine(RunScrolling(controller, false, false));
        }
        break;
      case ActionType.Scroll_Down:
        _value = value;
        if (!_runningCoroutine && value > 0.0f)
        {
          if (_app_functions == null) { _app_functions = App_Functions.Instance; }
          _app_functions.StartCoroutine(RunScrolling(controller, true, true));
        }
        break;
      case ActionType.Scroll_Left:
        _value = value;
        if (!_runningCoroutine && value > 0.0f)
        {
          if (_app_functions == null) { _app_functions = App_Functions.Instance; }
          _app_functions.StartCoroutine(RunScrolling(controller, false, true));
        }
        break;
      case ActionType.Key_Hit:
        if (isDown)
        {
          OsHook_Keyboard.SetKeyState((KbdKey)Key, true);
          OsHook_Keyboard.SetKeyState((KbdKey)Key, false);
        }
        break;
      case ActionType.Key_Press:
        OsHook_Keyboard.SetKeyState((KbdKey)Key, isDown);
        break;
      default:
        UnityEngine.Debug.LogError("Unhandled action type: " + Type);
        break;
    }

    if (Next != null) { Next.Run(controller, isDown); }
  }

  private App_Functions _app_functions;
  private float _value;
  private bool _runningCoroutine;

  private IEnumerator RunScrolling(Controller controller, bool param1, bool param2)
  {
    _runningCoroutine = true;
    var lastTime = 0.0f;
    while (_value > 0.0f)
    {
      var now = Time.time;
      if (now - lastTime > (1.0f - (_value * 0.25f + 0.75f)))
      {
        controller.FocusPointerEmulation?.DoScroll(param1, param2);
        lastTime = now;
      }
      yield return null;
    }
    _runningCoroutine = false;
  }
}
