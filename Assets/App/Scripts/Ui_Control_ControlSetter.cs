using UnityEngine;

public class Ui_Control_ControlSetter : Ui_Control_Button
{
  [Header("Wiring")]
  [SerializeField] private uint _controllerId;
  [SerializeField] private ControllerMapping.Controls _focusControl;
  [Header("Wiring")]
  [SerializeField] private TextMesh _actionDescription;
  [SerializeField] private GameObject _editor;
  [SerializeField] private Ui_Control_Textbox _descriptionEditor;

  public static void ClearFocus() { _focus = null; }

  private static Ui_Control_ControlSetter _focus
  {
    get { return __focus; }
    set
    {
      if (value == __focus) { return; }
      if (__focus)
      {
        __focus._actionDescription.gameObject.SetActive(true);
        __focus._editor.SetActive(false);
        App_Details.Instance.MyControllerMappings.
          Mappings[__focus._controllerId].ActionTitles[(int)__focus._focusControl] =
          __focus._actionDescription.text = __focus._descriptionEditor.Text;
      }
      __focus = value;
      if (__focus)
      {
        __focus._actionDescription.gameObject.SetActive(false);
        __focus._editor.SetActive(true);
        __focus._descriptionEditor.Text =
          App_Details.Instance.MyControllerMappings.
          Mappings[__focus._controllerId].ActionTitles[(int)__focus._focusControl];
      }
    }
  }
  private static Ui_Control_ControlSetter __focus;

  private ControllerAction _action;

  private void Start()
  {
    OnClick.AddListener(OnClickEventListener);
    _action =
      App_Details.Instance.MyControllerMappings.
      Mappings[_controllerId].Actions[(int)_focusControl];
  }

  private void OnEnable()
  {
    _actionDescription.text =
      App_Details.Instance.MyControllerMappings.
      Mappings[_controllerId].ActionTitles[(int)_focusControl];
  }

  private void OnClickEventListener()
  {
    _focus = this;
  }
}
