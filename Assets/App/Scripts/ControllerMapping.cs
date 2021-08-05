using Common;
using System;

[Serializable]
public class MappingCollection
{
  public const uint MappingsCount = 3;

  public ControllerMapping[] Mappings;

  public MappingCollection()
  {
    Mappings = new ControllerMapping[MappingsCount];
    for (var i = 0; i < MappingsCount; i++)
    {
      Mappings[i] = new ControllerMapping();
    }
  }

  public void SetupDefault()
  {
    for (var i = 0; i < MappingsCount; i++)
    {
      Mappings[i].SetupDefault(i);
    }
  }
}

[Serializable]
public class ControllerMapping
{
  public enum ControllerInput
  {
    Trigger, Grip, HighButton, LowButton, ThumbButton,
    ThumbUp, ThumbDown, ThumbLeft, ThumbRight
  }

  public Action<ControllerMapping> OnMappingChanged;

  public string[] ActionTitles = new string[Enum.GetNames(typeof(ControllerInput)).Length];
  public ControllerAction[] Actions =
    new ControllerAction[Enum.GetNames(typeof(ControllerInput)).Length];

  public ControllerMapping()
  {
    for (var i = 0; i < Actions.Length; i++)
    {
      Actions[i] = new ControllerAction();
    }
  }

  public void SetupDefault(int index)
  {
    int control;
    switch (index)
    {
      case 0:
        // Main - Trigger - left mouse
          control = (int)ControllerInput.Trigger;
          ActionTitles[control] = "Left mouse";
        // Main - Grip - Hold focus
          control = (int)ControllerInput.Grip;
          ActionTitles[control] = "Hold focus";
          Actions[control].Type = ControllerAction.ActionType.Hold_focus;
        // Main - High button - undo
          control = (int)ControllerInput.HighButton;
          ActionTitles[control] = "Undo";
          Actions[control].Type = ControllerAction.ActionType.Undo;
        // Main - Low button - pan - space, left mouse
          control = (int)ControllerInput.LowButton;
          ActionTitles[control] = "Pan";
          Actions[control].Type = ControllerAction.ActionType.Key_Press;
          Actions[control].Key = KbdKey.Space;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Main - Thumbstick button - zoom - control, space, left mouse
          control = (int)ControllerInput.ThumbButton;
          ActionTitles[control] = "Zoom";
          Actions[control].Type = ControllerAction.ActionType.Key_Press;
          Actions[control].Key = KbdKey.Control;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Key_Press;
          Actions[control].Next.Key = KbdKey.Space;
          Actions[control].Next.Next = new ControllerAction();
          Actions[control].Next.Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Main - Thumbstick direction left - nothing
          control = (int)ControllerInput.ThumbLeft;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Main - Thumbstick direction up - nothing
          control = (int)ControllerInput.ThumbUp;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Main - Thumbstick direction down - nothing
          control = (int)ControllerInput.ThumbDown;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Main - Thumbstick direction right - nothing
          control = (int)ControllerInput.ThumbRight;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        break;
      case 1:
        // Second - Trigger - pencil flip
          control = (int)ControllerInput.Trigger;
          ActionTitles[control] = "Eraser";
          Actions[control].Type = ControllerAction.ActionType.Pencil_flip;
        // Second - Grip - Hold desktop
          control = (int)ControllerInput.Grip;
          ActionTitles[control] = "Hold desktop";
          Actions[control].Type = ControllerAction.ActionType.Hold_desktop;
        // Second - High button - redo
          control = (int)ControllerInput.HighButton;
          ActionTitles[control] = "Redo";
          Actions[control].Type = ControllerAction.ActionType.Redo__ctrl___y;
        // Second - Low button - right mouse
          control = (int)ControllerInput.LowButton;
          ActionTitles[control] = "Right mouse";
          Actions[control].Type = ControllerAction.ActionType.Mouse_button__right;
        // Second - Thumbstick button - nothing
          control = (int)ControllerInput.ThumbButton;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Second - Thumbstick direction left - nothing
          control = (int)ControllerInput.ThumbLeft;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Second - Thumbstick direction up - nothing
          control = (int)ControllerInput.ThumbUp;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Second - Thumbstick direction down - nothing
          control = (int)ControllerInput.ThumbDown;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Second - Thumbstick direction right - nothing
          control = (int)ControllerInput.ThumbRight;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
      break;
      case 2:
        // Draw - Trigger - draw
          control = (int)ControllerInput.Trigger;
          ActionTitles[control] = "Draw";
        // Draw - Grip - Hold focus
          control = (int)ControllerInput.Grip;
          ActionTitles[control] = "Hold focus";
          Actions[control].Type = ControllerAction.ActionType.Hold_focus;
        // Draw - High button - undo
          control = (int)ControllerInput.HighButton;
          ActionTitles[control] = "Undo";
          Actions[control].Type = ControllerAction.ActionType.Undo;
        // Draw - Low button - pan - space, left mouse
          control = (int)ControllerInput.LowButton;
          ActionTitles[control] = "Pan";
          Actions[control].Type = ControllerAction.ActionType.Key_Press;
          Actions[control].Key = KbdKey.Space;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Draw - Thumbstick button - zoom - control, space, left mouse
          control = (int)ControllerInput.ThumbButton;
          ActionTitles[control] = "Zoom";
          Actions[control].Type = ControllerAction.ActionType.Key_Press;
          Actions[control].Key = KbdKey.Control;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Key_Press;
          Actions[control].Next.Key = KbdKey.Space;
          Actions[control].Next.Next = new ControllerAction();
          Actions[control].Next.Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Draw - Thumbstick direction left - brush size - shift, left mouse
          control = (int)ControllerInput.ThumbLeft;
          ActionTitles[control] = "Brush size";
          Actions[control].Type = ControllerAction.ActionType.Key_Press;
          Actions[control].Key = KbdKey.Shift;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Draw - Thumbstick direction up - nothing
          control = (int)ControllerInput.ThumbUp;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Draw - Thumbstick direction down - color pick - control
          control = (int)ControllerInput.ThumbDown;
          ActionTitles[control] = "Color pick";
          Actions[control].Type = ControllerAction.ActionType.Key_Press;
          Actions[control].Key = KbdKey.Control;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Draw - Thumbstick direction right - mirror - M toggles
          control = (int)ControllerInput.ThumbRight;
          ActionTitles[control] = "Mirror";
          Actions[control].Type = ControllerAction.ActionType.Key_Hit;
          Actions[control].Key = KbdKey.Key_M;
      break;
    }
  }
}
