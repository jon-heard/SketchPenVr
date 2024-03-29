﻿using Common;
using System;

[Serializable]
public class MappingCollection
{
  public const uint MappingsCount = 5;

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
    ThumbUp, ThumbRight, ThumbDown, ThumbLeft
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
        // Main - Low button - pan - middle mouse
          control = (int)ControllerInput.LowButton;
          ActionTitles[control] = "Pan";
          Actions[control].Type = ControllerAction.ActionType.Mouse_button__middle;
        // Main - Thumbstick button - zoom - control, space, left mouse
          control = (int)ControllerInput.ThumbButton;
          ActionTitles[control] = "Zoom";
          Actions[control].Type = ControllerAction.ActionType.Key_press;
          Actions[control].Key = KbdKey.Control;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Key_press;
          Actions[control].Next.Key = KbdKey.Space;
          Actions[control].Next.Next = new ControllerAction();
          Actions[control].Next.Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Main - Thumbstick direction up - nothing
        control = (int)ControllerInput.ThumbUp;
        ActionTitles[control] = "Scroll up";
        Actions[control].Type = ControllerAction.ActionType.Scroll_up;
        // Main - Thumbstick direction right - nothing
        control = (int)ControllerInput.ThumbRight;
        ActionTitles[control] = "Scroll right";
        Actions[control].Type = ControllerAction.ActionType.Scroll_right;
        // Main - Thumbstick direction down - nothing
        control = (int)ControllerInput.ThumbDown;
        ActionTitles[control] = "Scroll down";
        Actions[control].Type = ControllerAction.ActionType.Scroll_down;
        // Main - Thumbstick direction left - nothing
        control = (int)ControllerInput.ThumbLeft;
          ActionTitles[control] = "Scroll left";
          Actions[control].Type = ControllerAction.ActionType.Scroll_left;
        break;
      case 1:
        // Second - Trigger - pen flip
          control = (int)ControllerInput.Trigger;
          ActionTitles[control] = "Eraser";
          Actions[control].Type = ControllerAction.ActionType.Pen_flip;
        // Second - Grip - Hold desktop
          control = (int)ControllerInput.Grip;
          ActionTitles[control] = "Hold";
          Actions[control].Type = ControllerAction.ActionType.Hold_focus;
        // Second - High button - redo
          control = (int)ControllerInput.HighButton;
          ActionTitles[control] = "Redo";
          Actions[control].Type = ControllerAction.ActionType.Redo__ctrl___shift___z;
        // Second - Low button - right mouse
          control = (int)ControllerInput.LowButton;
          ActionTitles[control] = "Right mouse";
          Actions[control].Type = ControllerAction.ActionType.Mouse_button__right;
        // Second - Thumbstick button - mirror
          control = (int)ControllerInput.ThumbButton;
          ActionTitles[control] = "Mirror";
          Actions[control].Type = ControllerAction.ActionType.Key_hit;
          Actions[control].Key = KbdKey.Key_M;
        // Second - Thumbstick direction up - size panel up
          control = (int)ControllerInput.ThumbUp;
          ActionTitles[control] = "Size down";
          Actions[control].Type = ControllerAction.ActionType.Size_panel_down;
        // Second - Thumbstick direction right - transform
          control = (int)ControllerInput.ThumbRight;
          ActionTitles[control] = "";
          Actions[control].Type = ControllerAction.ActionType.Nothing;
        // Second - Thumbstick direction down - size panel down
          control = (int)ControllerInput.ThumbDown;
          ActionTitles[control] = "Size up";
          Actions[control].Type = ControllerAction.ActionType.Size_panel_up;
        // Second - Thumbstick direction left - nothing
          control = (int)ControllerInput.ThumbLeft;
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
        // Draw - Low button - pan - middle mouse
          control = (int)ControllerInput.LowButton;
          ActionTitles[control] = "Pan";
          Actions[control].Type = ControllerAction.ActionType.Mouse_button__middle;
        // Draw - Thumbstick button - zoom - control, space, left mouse
          control = (int)ControllerInput.ThumbButton;
          ActionTitles[control] = "Zoom";
          Actions[control].Type = ControllerAction.ActionType.Key_press;
          Actions[control].Key = KbdKey.Control;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Key_press;
          Actions[control].Next.Key = KbdKey.Space;
          Actions[control].Next.Next = new ControllerAction();
          Actions[control].Next.Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Draw - Thumbstick direction up - nothing
          control = (int)ControllerInput.ThumbUp;
          ActionTitles[control] = "Line";
          Actions[control].Type = ControllerAction.ActionType.Key_press;
          Actions[control].Key = KbdKey.Key_V;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Draw - Thumbstick direction right - mirror - M toggles
          control = (int)ControllerInput.ThumbRight;
          ActionTitles[control] = "Color flip";
          Actions[control].Type = ControllerAction.ActionType.Key_hit;
          Actions[control].Key = KbdKey.Key_X;
        // Draw - Thumbstick direction down - color pick - control
          control = (int)ControllerInput.ThumbDown;
          ActionTitles[control] = "Color pick";
          Actions[control].Type = ControllerAction.ActionType.Key_press;
          Actions[control].Key = KbdKey.Control;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;
        // Draw - Thumbstick direction left - brush size - shift, left mouse
          control = (int)ControllerInput.ThumbLeft;
          ActionTitles[control] = "Brush size";
          Actions[control].Type = ControllerAction.ActionType.Key_press;
          Actions[control].Key = KbdKey.Shift;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Mouse_button__left;
      break;
      case 3:
        // Left buttons - 0 - brush tool - b key 
          control = 0;
          ActionTitles[control] = "Brush";
          Actions[control].Type = ControllerAction.ActionType.Key_hit;
          Actions[control].Key = KbdKey.Key_B;
        // Left buttons - 1 - fill tool - f key
          control = 1;
          ActionTitles[control] = "Fill";
          Actions[control].Type = ControllerAction.ActionType.Key_hit;
          Actions[control].Key = KbdKey.Key_F;
        // Left buttons - 2 - gradient tool - g key
          control = 2;
          ActionTitles[control] = "Gradient";
          Actions[control].Type = ControllerAction.ActionType.Key_hit;
          Actions[control].Key = KbdKey.Key_G;
      break;
      case 4:
        // Right buttons - 0 - select tool - control-r
          control = 0;
          ActionTitles[control] = "Select";
          Actions[control].Type = ControllerAction.ActionType.Key_press;
          Actions[control].Key = KbdKey.Control;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Key_hit;
          Actions[control].Next.Key = KbdKey.Key_R;
        // Right buttons - 1 - Thumbstick direction right - transform
          control = 1;
          ActionTitles[control] = "Transform";
          Actions[control].Type = ControllerAction.ActionType.Key_press;
          Actions[control].Key = KbdKey.Control;
          Actions[control].Next = new ControllerAction();
          Actions[control].Next.Type = ControllerAction.ActionType.Key_hit;
          Actions[control].Next.Key = KbdKey.Key_T;
        // Right buttons - 2 - eliptical select tool - j key
          control = 2;
          ActionTitles[control] = "Select (elipse)";
          Actions[control].Type = ControllerAction.ActionType.Key_hit;
          Actions[control].Key = KbdKey.Key_J;
      break;
    }
  }
}
