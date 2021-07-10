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
    UpdateActionEditorUiFromActions(true);
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

  private void UpdateActionEditorUiFromActions(bool showNothingActions = false)
  {
    // Falsify "editing" flag
    var editing = _editing;
    _editing = false;

    // Title update
    __focus._actionDescriptionEditor.Text = Mapping.ActionTitles[(int)__focus._focusControl];

    // All "Add action" buttons default to hidden
    foreach (var button in _addActionButtons) { button.SetActive(false); }

    // ALWAYS Setup first action ui
    var action = Action;
    var addActionButtonEnabled = false;
    _actionEditorUis[0].gameObject.SetActive(true);
    _actionParameterEditorUis[0].gameObject.SetActive(action.Type.HasKey());
    _actionEditorUis[0].Index = (uint)action.Type;
    _actionParameterEditorUis[0].Index = (uint)action.Key;
    action = action.Next;

    // Setup subsequent action uis conditionally on existance of their actions
    for (var i = 1; i < _actionEditorUis.Length; i++)
    {
      if (action != null &&
          (action.Type != ControllerAction.ActionType.Nothing || showNothingActions))
      {
        _actionEditorUis[i].gameObject.SetActive(true);
        _actionParameterEditorUis[i].gameObject.SetActive(action.Type.HasKey());
        _actionEditorUis[i].Index = (uint)action.Type;
        _actionParameterEditorUis[i].Index = (uint)action.Key;
        action = action.Next;
      }
      else
      {
        _actionEditorUis[i].gameObject.SetActive(false);
        _actionParameterEditorUis[i].gameObject.SetActive(false);
        _actionEditorUis[i].Index = 0;
        if (!addActionButtonEnabled && (i - 1) < _actionEditorUis.Length)
        {
          _addActionButtons[i - 1].SetActive(true);
          addActionButtonEnabled = true;
        }
      }
    }

    _editing = editing;
  }

  private void UpdateActionsFromActionEditorUi(bool showNothingActions = false)
  {
    // Title update
    Mapping.ActionTitles[(int)__focus._focusControl] = __focus._actionDescription.text =
      __focus._actionDescriptionEditor.Text;

    var i = 0;
    if (!showNothingActions)
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
    action.Key = KbdKey.None;
    for (; i < _actionEditorUis.Length; i++)
    {
      if (action == null) { break; }
      action.Type = (ControllerAction.ActionType)_actionEditorUis[i].Index;
      action.Key = action.Type.HasKey() ? (KbdKey)_actionParameterEditorUis[i].Index : KbdKey.None;
      while ((i < _actionEditorUis.Length - 1) &&
              ((_actionEditorUis[i + 1].Index == 0) ||
              (_actionEditorUis[i + 1].Index == Global.NullUint)) &&
              (!showNothingActions || action.Next == null ||
               action.Next.Type != ControllerAction.ActionType.Nothing))
      {
        action.Next = null;
        i++;
      }
      if (i < _actionEditorUis.Length - 1)
      {
        action.Next = action.Next ?? new ControllerAction();
        action = action?.Next;
      }
    }
  }
}
