using Common;
using System;
using UnityEngine;

public static class ActionTypeExtension
{
  public static bool HasKey(this ControllerAction.ActionType type)
  {
    return
      type == ControllerAction.ActionType.KeyHit ||
      type == ControllerAction.ActionType.KeyPress;
  }
}

[Serializable]
public class ControllerAction
{
  public enum ActionType
  {
    Nothing,
    Hold,
    HoldDesktop,
    PencilFlip,
    LeftMouseButton,
    RightMouseButton,
    Undo,
    Redo,
    KeyHit,
    KeyPress
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
      case ActionType.HoldDesktop:
        controller.IsHoldingDesktop = isDown;
        break;
      case ActionType.PencilFlip:
        Controller.IsFlipped = isDown;
        break;
      case ActionType.LeftMouseButton:
        if (controller.FocusPointerEmulation)
        {
          controller.FocusPointerEmulation.MouseLeftButton = isDown;
        }
        break;
      case ActionType.RightMouseButton:
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
      case ActionType.Redo:
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
      case ActionType.KeyHit:
        if (isDown)
        {
          OsHook_Keyboard.SetKeyState((KbdKey)Key, true);
          OsHook_Keyboard.SetKeyState((KbdKey)Key, false);
        }
        break;
      case ActionType.KeyPress:
        OsHook_Keyboard.SetKeyState((KbdKey)Key, isDown);
        break;
      default:
        UnityEngine.Debug.LogError("Unhandled action type: " + Type);
        break;
    }

    if (Next != null) { Next.Run(controller, isDown); }
  }
}
