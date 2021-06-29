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

  public LabelLine Top;
  public LabelLine Bottom;
  public LabelLine Trigger;
  public LabelLine Grip;
  public LabelLine ThumbPush;
  public LabelLine ThumbUp;
  public LabelLine ThumbRight;
  public LabelLine ThumbDown;
  public LabelLine ThumbLeft;
  public GameObject ThumbLine;

  public void OnMappingChanged(ControllerMapping mapping)
  {
    Top.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.HighButton];
    Top.Line.SetActive(Top.Label.text != "");
    Bottom.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.LowButton];
    Bottom.Line.SetActive(Bottom.Label.text != "");
    Trigger.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.Trigger];
    Trigger.Line.SetActive(Trigger.Label.text != "");
    Grip.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.Grip];
    Grip.Line.SetActive(Grip.Label.text != "");
    ThumbPush.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbButton];
    ThumbPush.Line.SetActive(ThumbPush.Label.text != "");
    ThumbUp.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbUp];
    ThumbUp.Line.SetActive(ThumbUp.Label.text != "");
    ThumbRight.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbRight];
    ThumbRight.Line.SetActive(ThumbRight.Label.text != "");
    ThumbDown.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbDown];
    ThumbDown.Line.SetActive(ThumbDown.Label.text != "");
    ThumbLeft.Label.text = mapping.ActionTitles[(int)ControllerMapping.Controls.ThumbLeft];
    ThumbLeft.Line.SetActive(ThumbLeft.Label.text != "");
    ThumbLine.SetActive(
      ThumbPush.Label.text != "" ||
      ThumbUp.Label.text != "" ||
      ThumbRight.Label.text != "" ||
      ThumbDown.Label.text != "" ||
      ThumbLeft.Label.text != "");
  }
}
