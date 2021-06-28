using UnityEngine;

public class Ui_Control_ControlSetter : Ui_Control_Button
{
  [SerializeField] private GameObject _actionDescription;
  [SerializeField] private GameObject _editor;

  public static void ClearFocus() { _focus = null; }

  private static Ui_Control_ControlSetter _focus
  {
    get { return __focus; }
    set
    {
      if (value == __focus) { return; }
      if (__focus)
      {
        __focus._actionDescription.SetActive(true);
        __focus._editor.SetActive(false);
      }
      __focus = value;
      if (__focus)
      {
        __focus._actionDescription.SetActive(false);
        __focus._editor.SetActive(true);
      }
    }
  }
  private static Ui_Control_ControlSetter __focus;

  private void Start()
  {
    OnClick.AddListener(OnClickEventListener);
  }

  private void OnClickEventListener()
  {
    _focus = this;
  }
}
