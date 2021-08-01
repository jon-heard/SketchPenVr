using Common;
using System;
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
    Hold,
    Hold_desktop,
    Pencil_flip,
    Mouse_button__left,
    Mouse_button__right,
    Undo,
    Redo__ctrl___shift___z,
    Redo__ctrl___y,
    Key_Hit,
    Key_Press
  }

  public ActionType Type;
  public KbdKey Key;
  [SerializeReference]
  public ControllerAction Next;

  public void Run(Controller controller, bool isDown)
  {
    switch (Type)
    {
      case ActionType.Nothing:
        break;
      case ActionType.Hold:
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
}
