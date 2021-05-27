using System;

[Serializable]
public class ControllerAction
{
  public enum ActionType
  {
    Nothing,
    Hold,
    PencilFlip,
    LeftMouseButton,
    RightMouseButton,
    Undo,
    Redo,
    KeyHit,
    KeyPress,
    KeyTapOnOff,
  }

  public ActionType Type;
  public int Parameter;
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
      case ActionType.PencilFlip:
        Controller.IsFlipped = isDown;
        break;
      case ActionType.LeftMouseButton:
        controller.FocusPointerEmulation.MouseLeftButton = isDown;
        break;
      case ActionType.RightMouseButton:
        controller.FocusPointerEmulation.MouseRightButton = isDown;
        break;
      case ActionType.Undo:
        if (isDown)
        {
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Control, true);
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Key_Z, true);
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Key_Z, false);
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Control, false);
        }
        break;
      case ActionType.Redo:
        if (isDown)
        {
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Control, true);
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Shift, true);
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Key_Z, true);
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Key_Z, false);
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Shift, false);
          OsHook_Keyboard.SetKeyState(OsHook_Keyboard.Key.Control, false);
        }
        break;
      case ActionType.KeyHit:
        if (isDown)
        {
          OsHook_Keyboard.SetKeyState((OsHook_Keyboard.Key)Parameter, true);
          OsHook_Keyboard.SetKeyState((OsHook_Keyboard.Key)Parameter, false);
        }
        break;
      case ActionType.KeyPress:
        OsHook_Keyboard.SetKeyState((OsHook_Keyboard.Key)Parameter, isDown);
        break;
      case ActionType.KeyTapOnOff:
        OsHook_Keyboard.SetKeyState((OsHook_Keyboard.Key)Parameter, true);
        OsHook_Keyboard.SetKeyState((OsHook_Keyboard.Key)Parameter, false);
        break;
      default:
        UnityEngine.Debug.LogError("Unhandled action type: " + Type);
        break;
    }

    if (Next != null) { Next.Run(controller, isDown); }
  }
}
