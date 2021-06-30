using System;

[Serializable]
public class MappingCollection
{
  public ControllerMapping[] Mappings;

  public MappingCollection()
  {
    Mappings = new ControllerMapping[2];
    for (var i = 0; i < Mappings.Length; i++)
    {
      Mappings[i] = new ControllerMapping();
    }
  }

  public void SetupDefault()
  {
    for (var i = 0; i < Mappings.Length; i++)
    {
      Mappings[i].SetupDefault(i);
    }
  }
}

[Serializable]
public class ControllerMapping
{
  public enum Controls
  {
    Trigger, Grip, HighButton, LowButton, ThumbButton,
    ThumbUp, ThumbDown, ThumbLeft, ThumbRight
  }

  public Action<ControllerMapping> OnMappingChanged;

  public string[] ActionTitles = new string[Enum.GetNames(typeof(Controls)).Length];
  public ControllerAction[] Actions = new ControllerAction[Enum.GetNames(typeof(Controls)).Length];

  public ControllerMapping()
  {
    for (var i = 0; i < Actions.Length; i++)
    {
      Actions[i] = new ControllerAction();
    }
  }

  public void SetupDefault(int index)
  {
    var control = (int)Controls.Grip;
    ActionTitles[control] = "Hold";
    Actions[control].Type = ControllerAction.ActionType.Hold;

    if (index == 0)
    {
      // Left - Trigger - draw
      control = (int)Controls.Trigger;
      ActionTitles[control] = "Draw / Left mouse";

      // Left - Top button - undo
      control = (int)Controls.HighButton;
      ActionTitles[control] = "Undo";
      Actions[control].Type = ControllerAction.ActionType.Undo;

      // Left - Bottom button - color pic - control
      control = (int)Controls.LowButton;
      ActionTitles[control] = "Color pick";
      Actions[control].Type = ControllerAction.ActionType.KeyPress;
      Actions[control].Parameter = (int)OsHook_Keyboard.Key.Control;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.LeftMouseButton;

      // Left - Thumb down - rotate - shift, space, left mouse
      control = (int)Controls.ThumbButton;
      ActionTitles[control] = "Rotate";
      Actions[control].Type = ControllerAction.ActionType.KeyPress;
      Actions[control].Parameter = (int)OsHook_Keyboard.Key.Shift;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.KeyPress;
      Actions[control].Next.Parameter = (int)OsHook_Keyboard.Key.Space;
      Actions[control].Next.Next = new ControllerAction();
      Actions[control].Next.Next.Type = ControllerAction.ActionType.LeftMouseButton;

      // Left - Thumb left - brush size - shift, left mouse
      control = (int)Controls.ThumbLeft;
      ActionTitles[control] = "Brush size";
      Actions[control].Type = ControllerAction.ActionType.KeyPress;
      Actions[control].Parameter = (int)OsHook_Keyboard.Key.Shift;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.LeftMouseButton;

      // Left - Thumb up - pan - space, left mouse
      control = (int)Controls.ThumbUp;
      ActionTitles[control] = "Pan";
      Actions[control].Type = ControllerAction.ActionType.KeyPress;
      Actions[control].Parameter = (int)OsHook_Keyboard.Key.Space;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.LeftMouseButton;

      // Left - Thumb down - zoom - control, space, left mouse
      control = (int)Controls.ThumbDown;
      ActionTitles[control] = "Zoom";
      Actions[control].Type = ControllerAction.ActionType.KeyPress;
      Actions[control].Parameter = (int)OsHook_Keyboard.Key.Control;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.KeyPress;
      Actions[control].Next.Parameter = (int)OsHook_Keyboard.Key.Space;
      Actions[control].Next.Next = new ControllerAction();
      Actions[control].Next.Next.Type = ControllerAction.ActionType.LeftMouseButton;

      UnityEngine.Debug.Log(UnityEngine.JsonUtility.ToJson(Actions[control]));

      // Left - Thumb right - mirror - M toggles
      control = (int)Controls.ThumbRight;
      ActionTitles[control] = "Mirror";
      Actions[control].Type = ControllerAction.ActionType.KeyHit;
      Actions[control].Parameter = (int)OsHook_Keyboard.Key.Key_M;
    }
    else
    {
      // Right - Trigger button - pencil flip
      control = (int)Controls.Trigger;
      ActionTitles[control] = "Eraser";
      Actions[control].Type = ControllerAction.ActionType.PencilFlip;

      // Right - Top button - redo
      control = (int)Controls.HighButton;
      ActionTitles[control] = "Redo";
      Actions[control].Type = ControllerAction.ActionType.Redo;

      // Right - Bottom button - brush wheel - right mouse
      control = (int)Controls.LowButton;
      ActionTitles[control] = "Brush Select";
      Actions[control].Type = ControllerAction.ActionType.RightMouseButton;
    }
  }
}
