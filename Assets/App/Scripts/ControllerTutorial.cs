using System;
using UnityEngine;

public class ControllerTutorial : MonoBehaviour
{
  [Serializable]
  public struct LabelLine
  {
    public TextMesh Label;
    public GameObject Line;
  }

  public LabelLine HighButton;
  public LabelLine LowButton;
  public LabelLine Trigger;
  public LabelLine Grip;
  public LabelLine ThumbButton;
  public LabelLine ThumbUp;
  public LabelLine ThumbRight;
  public LabelLine ThumbDown;
  public LabelLine ThumbLeft;
  public GameObject ThumbLine;

  public void OnMappingChanged(ControllerMapping mapping)
  {
    HighButton.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.HighButton];
    HighButton.Line.SetActive(HighButton.Label.text != "");
    LowButton.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.LowButton];
    LowButton.Line.SetActive(LowButton.Label.text != "");
    Trigger.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.Trigger];
    Trigger.Line.SetActive(Trigger.Label.text != "");
    Grip.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.Grip];
    Grip.Line.SetActive(Grip.Label.text != "");
    ThumbButton.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbButton];
    ThumbButton.Line.SetActive(ThumbButton.Label.text != "");
    ThumbUp.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbUp];
    ThumbUp.Line.SetActive(ThumbUp.Label.text != "");
    ThumbRight.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbRight];
    ThumbRight.Line.SetActive(ThumbRight.Label.text != "");
    ThumbDown.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbDown];
    ThumbDown.Line.SetActive(ThumbDown.Label.text != "");
    ThumbLeft.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbLeft];
    ThumbLeft.Line.SetActive(ThumbLeft.Label.text != "");
    ThumbLine.SetActive(
      ThumbButton.Label.text != "" ||
      ThumbUp.Label.text != "" ||
      ThumbRight.Label.text != "" ||
      ThumbDown.Label.text != "" ||
      ThumbLeft.Label.text != "");
  }
}
