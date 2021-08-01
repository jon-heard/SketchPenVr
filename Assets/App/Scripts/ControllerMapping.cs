using Common;
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
    Actions[control].Type = ControllerAction.ActionType.Hold_desktop;

    if (index == 0)
    {
      // Left - Trigger - draw
      control = (int)Controls.Trigger;
      ActionTitles[control] = "Draw / Left mouse";

      // Left - High button - undo
      control = (int)Controls.HighButton;
      ActionTitles[control] = "Undo";
      Actions[control].Type = ControllerAction.ActionType.Undo;

      // Left - Low button - color pic - control
      control = (int)Controls.LowButton;
      ActionTitles[control] = "Color pick";
      Actions[control].Type = ControllerAction.ActionType.Key_Press;
      Actions[control].Key = KbdKey.Control;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;

      // Left - Thumb down - rotate - shift, space, left mouse
      control = (int)Controls.ThumbButton;
      ActionTitles[control] = "Rotate";
      Actions[control].Type = ControllerAction.ActionType.Key_Press;
      Actions[control].Key = KbdKey.Shift;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.Key_Press;
      Actions[control].Next.Key = KbdKey.Space;
      Actions[control].Next.Next = new ControllerAction();
      Actions[control].Next.Next.Type = ControllerAction.ActionType.Mouse_button__left;

      // Left - Thumb left - brush size - shift, left mouse
      control = (int)Controls.ThumbLeft;
      ActionTitles[control] = "Brush size";
      Actions[control].Type = ControllerAction.ActionType.Key_Press;
      Actions[control].Key = KbdKey.Shift;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;

      // Left - Thumb up - pan - space, left mouse
      control = (int)Controls.ThumbUp;
      ActionTitles[control] = "Pan";
      Actions[control].Type = ControllerAction.ActionType.Key_Press;
      Actions[control].Key = KbdKey.Space;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;

      // Left - Thumb down - zoom - control, space, left mouse
      control = (int)Controls.ThumbDown;
      ActionTitles[control] = "Zoom";
      Actions[control].Type = ControllerAction.ActionType.Key_Press;
      Actions[control].Key = KbdKey.Control;
      Actions[control].Next = new ControllerAction();
      Actions[control].Next.Type = ControllerAction.ActionType.Key_Press;
      Actions[control].Next.Key = KbdKey.Space;
      Actions[control].Next.Next = new ControllerAction();
      Actions[control].Next.Next.Type = ControllerAction.ActionType.Mouse_button__left;

      // Left - Thumb right - mirror - M toggles
      control = (int)Controls.ThumbRight;
      ActionTitles[control] = "Mirror";
      Actions[control].Type = ControllerAction.ActionType.Key_Hit;
      Actions[control].Key = KbdKey.Key_M;
    }
    else
    {
      // Right - Trigger button - pencil flip
      control = (int)Controls.Trigger;
      ActionTitles[control] = "Eraser";
      Actions[control].Type = ControllerAction.ActionType.Pencil_flip;

      // Right - Top button - redo
      control = (int)Controls.HighButton;
      ActionTitles[control] = "Redo";
      Actions[control].Type = ControllerAction.ActionType.Redo__ctrl___y;

      // Right - Bottom button - brush wheel - right mouse
      control = (int)Controls.LowButton;
      ActionTitles[control] = "Brush Select";
      Actions[control].Type = ControllerAction.ActionType.Mouse_button__right;
    }
  }
}
