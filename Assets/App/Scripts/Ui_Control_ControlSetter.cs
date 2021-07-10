using Common;
using Common.Vr.Ui.Controls;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Control_ControlSetter : Button
{
  [Header("Wiring")]
  [SerializeField] private uint _controllerId;
  [SerializeField] private ControllerMapping.Controls _focusControl;
  [Header("Wiring")]
  [SerializeField] private TextMesh _actionDescription;
  [SerializeField] private GameObject _editor;
  [SerializeField] private Textbox _actionDescriptionEditor;
  [SerializeField] private Dropdown[] _actionEditorUis;
  [SerializeField] private Dropdown[] _actionParameterEditorUis;
  [SerializeField] private GameObject[] _addActionButtons;

  public void OnActionUpdated()
  {
    for (var i = 0; i < _actionEditorUis.Length; i++)
    {
      var actionHasParameter =
        ((ControllerAction.ActionType)_actionEditorUis[i].Index).HasKey();
      _actionParameterEditorUis[i].gameObject.SetActive(actionHasParameter);
    }
  }

  public void OnAddActionButton()
  {
    UpdateActionsFromActionEditorUi(true);
    var action = Action;
    while (action != null)
    {
      if (action.Next == null)
      {
        action.Next = new ControllerAction();
        action.Next.Type = ControllerAction.ActionType.Nothing;
        break;
      }
      action = action.Next;
    }
    UpdateActionEditorUiFromActions();
  }

  public ControllerMapping Mapping
  {
    get { return App_Details.Instance.MyControllerMappings.Mappings[_controllerId]; }
  }

  public ControllerAction Action
  {
    get { return Mapping.Actions[(int)__focus._focusControl]; }
  }

  public static void ClearFocus()
  { _focus = null; }

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
        __focus.UpdateActionsFromActionEditorUi();
        __focus._editing = false;
      }
      __focus = value;
      if (__focus)
      {
        __focus._actionDescription.gameObject.SetActive(false);
        __focus._editor.SetActive(true);
        __focus.UpdateActionEditorUiFromActions();
        __focus._editing = true;
      }
    }
  }
  private static Ui_Control_ControlSetter __focus;

  private bool _editing = false;

  protected void Start()
  {
    Control.OnControlClicked += OnClickedEventListener;

    var actionTypeList = new List<string>(Enum.GetNames(typeof(ControllerAction.ActionType)));
    var actionParameterList = KbdKeyHelp.GetTitleList();
    for(var i = 0; i < _actionEditorUis.Length; i++)
    {
      _actionEditorUis[i].SetList(actionTypeList);
      _actionParameterEditorUis[i].SetList(actionParameterList);
    }
  }

  private void OnEnable()
  {
    _actionDescription.text = Mapping.ActionTitles[(int)_focusControl];
  }

  private void OnClickedEventListener(Control clicked)
  {
    if (clicked == this)
    {
      if (_focus != this)
      {
        _focus = this;
      }
      else
      {
        _focus = null;
      }
    }
  }

  private void UpdateActionEditorUiFromActions()
  {
    var editing = _editing;
    _editing = false;

    __focus._actionDescriptionEditor.Text = Mapping.ActionTitles[(int)__focus._focusControl];

    var action = Action;
    var buttonAdded = false;
    foreach (var button in _addActionButtons) { button.SetActive(false); }
    for (var i = 0; i < _actionEditorUis.Length; i++)
    {
      if (action != null)
      {
        _actionEditorUis[i].gameObject.SetActive(true);
        _actionParameterEditorUis[i].gameObject.SetActive(action.Type.HasKey());
        _actionEditorUis[i].Index = (uint)action.Type;
        _actionParameterEditorUis[i].Index = (uint)action.Key;
        action = action.Next;
      }
      else
      {
        _actionEditorUis[i].Index = 0;
        _actionEditorUis[i].gameObject.SetActive(false);
        _actionParameterEditorUis[i].gameObject.SetActive(false);
        if (!buttonAdded && (i - 1) < _actionEditorUis.Length)
        {
          _addActionButtons[i - 1].SetActive(true);
          buttonAdded = true;
        }
      }
    }

    _editing = editing;
  }

  private void UpdateActionsFromActionEditorUi(bool keepNothingActions = false)
  {
    Mapping.ActionTitles[(int)__focus._focusControl] = __focus._actionDescriptionEditor.Text;
    __focus._actionDescription.text = __focus._actionDescriptionEditor.Text;

    var i = 0;
    if (!keepNothingActions)
    {
      while ((i < _actionEditorUis.Length) &&
             ((_actionEditorUis[i].Index == 0) ||
              (_actionEditorUis[i].Index == Global.NullUint)))
      {
        i++;
      }
    }
    var action = Action;
    action.Type = ControllerAction.ActionType.Nothing;
    for (; i < _actionEditorUis.Length; i++)
    {
      if (action == null) { break; }
      action.Type = (ControllerAction.ActionType)_actionEditorUis[i].Index;
      if (_actionParameterEditorUis[i].Index != Global.NullUint)
      {
        action.Key = (KbdKey)_actionParameterEditorUis[i].Index;
      }
      if (!keepNothingActions)
      {
        while ((i < _actionEditorUis.Length - 1) &&
               ((_actionEditorUis[i + 1].Index == 0) ||
                (_actionEditorUis[i + 1].Index == Global.NullUint)))
        {
          i++;
        }
      }
      if (i < _actionEditorUis.Length - 1)
      {
        action.Next = (i < _actionEditorUis.Length - 1) ? new ControllerAction() : null;
        action = action?.Next;
      }
    }
  }
}
