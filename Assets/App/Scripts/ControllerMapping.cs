using System;

[Serializable]
public class ControllerMapping
{
  public ControllerAction TopButtonAction = new ControllerAction();
  public ControllerAction BottomButtonAction = new ControllerAction();
  public ControllerAction TriggerButtonAction = new ControllerAction();
  public ControllerAction GripButtonAction = new ControllerAction();
  public ControllerAction ThumbButtonAction = new ControllerAction();
  public ControllerAction[] ThumbDirectionActions = new ControllerAction[4];

  public ControllerMapping(int index)
  {
    for (var i = 0; i < ThumbDirectionActions.Length; i++)
    {
      ThumbDirectionActions[i] = new ControllerAction();
    }

    GripButtonAction.Type = ControllerAction.ActionType.Hold;
    if (index == 0)
    {
      // Top button - undo
      TopButtonAction.Type = ControllerAction.ActionType.Undo;
      // Bottom button - color pic - control
      BottomButtonAction.Type = ControllerAction.ActionType.KeyPress;
      BottomButtonAction.Parameter = (int)OsHook_Keyboard.Key.Control;
      BottomButtonAction.Next = new ControllerAction();
      BottomButtonAction.Next.Type = ControllerAction.ActionType.LeftMouseButton;
      // Thumb down - pan - space, left mouse
      ThumbButtonAction.Type = ControllerAction.ActionType.KeyPress;
      ThumbButtonAction.Parameter = (int)OsHook_Keyboard.Key.Space;
      ThumbButtonAction.Next = new ControllerAction();
      ThumbButtonAction.Next.Type = ControllerAction.ActionType.LeftMouseButton;
      // Thumb left - pan - shift, left mouse
      ThumbDirectionActions[(int)Controller.ThumbState.Left].Type = ControllerAction.ActionType.KeyPress;
      ThumbDirectionActions[(int)Controller.ThumbState.Left].Parameter = (int)OsHook_Keyboard.Key.Shift;
      ThumbDirectionActions[(int)Controller.ThumbState.Left].Next = new ControllerAction();
      ThumbDirectionActions[(int)Controller.ThumbState.Left].Next.Type = ControllerAction.ActionType.LeftMouseButton;
      // Thumb up - rotate - shift, space, left mouse
      ThumbDirectionActions[(int)Controller.ThumbState.Up].Type = ControllerAction.ActionType.KeyPress;
      ThumbDirectionActions[(int)Controller.ThumbState.Up].Parameter = (int)OsHook_Keyboard.Key.Shift;
      ThumbDirectionActions[(int)Controller.ThumbState.Up].Next = new ControllerAction();
      ThumbDirectionActions[(int)Controller.ThumbState.Up].Next.Type = ControllerAction.ActionType.KeyPress;
      ThumbDirectionActions[(int)Controller.ThumbState.Up].Next.Parameter = (int)OsHook_Keyboard.Key.Space;
      ThumbDirectionActions[(int)Controller.ThumbState.Left].Next.Next = new ControllerAction();
      ThumbDirectionActions[(int)Controller.ThumbState.Left].Next.Next.Type = ControllerAction.ActionType.LeftMouseButton;
      // Thumb down - zoom - control, space, left mouse
      ThumbDirectionActions[(int)Controller.ThumbState.Down].Type = ControllerAction.ActionType.KeyPress;
      ThumbDirectionActions[(int)Controller.ThumbState.Down].Parameter = (int)OsHook_Keyboard.Key.Control;
      ThumbDirectionActions[(int)Controller.ThumbState.Down].Next = new ControllerAction();
      ThumbDirectionActions[(int)Controller.ThumbState.Down].Next.Type = ControllerAction.ActionType.KeyPress;
      ThumbDirectionActions[(int)Controller.ThumbState.Down].Next.Parameter = (int)OsHook_Keyboard.Key.Space;
      ThumbDirectionActions[(int)Controller.ThumbState.Left].Next.Next = new ControllerAction();
      ThumbDirectionActions[(int)Controller.ThumbState.Left].Next.Next.Type = ControllerAction.ActionType.LeftMouseButton;
    }
    else
    {
      TopButtonAction.Type = ControllerAction.ActionType.Redo;
      TriggerButtonAction.Type = ControllerAction.ActionType.PencilFlip;
    }
  }
}
