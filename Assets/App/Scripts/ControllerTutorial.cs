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
    Top.Label.text = mapping.TopButtonTitle;
    Top.Line.SetActive(Top.Label.text != "");
    Bottom.Label.text = mapping.BottomButtonTitle;
    Bottom.Line.SetActive(Bottom.Label.text != "");
    Trigger.Label.text = mapping.TriggerButtonTitle;
    Trigger.Line.SetActive(Trigger.Label.text != "");
    Grip.Label.text = mapping.GripButtonTitle;
    Grip.Line.SetActive(Grip.Label.text != "");
    ThumbPush.Label.text = mapping.ThumbButtonTitle;
    ThumbPush.Line.SetActive(ThumbPush.Label.text != "");
    ThumbUp.Label.text = mapping.ThumbDirectionTitles[0];
    ThumbUp.Line.SetActive(ThumbUp.Label.text != "");
    ThumbRight.Label.text = mapping.ThumbDirectionTitles[1];
    ThumbRight.Line.SetActive(ThumbRight.Label.text != "");
    ThumbDown.Label.text = mapping.ThumbDirectionTitles[2];
    ThumbDown.Line.SetActive(ThumbDown.Label.text != "");
    ThumbLeft.Label.text = mapping.ThumbDirectionTitles[3];
    ThumbLeft.Line.SetActive(ThumbLeft.Label.text != "");
    ThumbLine.SetActive(
      ThumbPush.Label.text != "" ||
      ThumbUp.Label.text != "" ||
      ThumbRight.Label.text != "" ||
      ThumbDown.Label.text != "" ||
      ThumbLeft.Label.text != "");
  }
}
