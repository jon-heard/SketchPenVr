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
    HighButton.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.HighButton];
    HighButton.Line.SetActive(HighButton.Label.text != "");
    LowButton.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.LowButton];
    LowButton.Line.SetActive(LowButton.Label.text != "");
    Trigger.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.Trigger];
    Trigger.Line.SetActive(Trigger.Label.text != "");
    Grip.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.Grip];
    Grip.Line.SetActive(Grip.Label.text != "");
    ThumbButton.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.ThumbButton];
    ThumbButton.Line.SetActive(ThumbButton.Label.text != "");
    ThumbUp.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.ThumbUp];
    ThumbUp.Line.SetActive(ThumbUp.Label.text != "");
    ThumbRight.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.ThumbRight];
    ThumbRight.Line.SetActive(ThumbRight.Label.text != "");
    ThumbDown.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.ThumbDown];
    ThumbDown.Line.SetActive(ThumbDown.Label.text != "");
    ThumbLeft.Label.text = mapping.ActionTitles[(int)ControllerMapping.ControllerInput.ThumbLeft];
    ThumbLeft.Line.SetActive(ThumbLeft.Label.text != "");
    ThumbLine.SetActive(
      ThumbButton.Label.text != "" ||
      ThumbUp.Label.text != "" ||
      ThumbRight.Label.text != "" ||
      ThumbDown.Label.text != "" ||
      ThumbLeft.Label.text != "");
  }
}
