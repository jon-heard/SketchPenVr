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
        ((ControllerAction.ActionType)_actionEditorUis[i].Index).HasParameter();
      _actionParameterEditorUis[i].gameObject.SetActive(actionHasParameter);
    }
  }

  public void OnAddActionButton()
  {
    var action = Action;
    while (action != null)
    {
      if (action.Next == null && action.Type != ControllerAction.ActionType.Nothing)
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

  private static Dictionary<int, uint> _key_ValueToEnumIndex;
  private static Dictionary<uint, int> _key_EnumIndexToValue;

  private bool _editing = false;

  private void Start()
  {
    OnClick.AddListener(OnClickEventListener);
    var actionTypeList = new List<string>(Enum.GetNames(typeof(ControllerAction.ActionType)));
    var actionParameterList = new List<string>(Enum.GetNames(typeof(OsHook_Keyboard.Key)));
    for(var i = 0; i < _actionEditorUis.Length; i++)
    {
      _actionEditorUis[i].SetList(actionTypeList);
      _actionParameterEditorUis[i].SetList(actionParameterList);
    }
  }

  private void OnEnable()
  {
    if (_key_ValueToEnumIndex == null || _key_EnumIndexToValue == null)
    {
      SetupKeyIntConversions();
    }
    _actionDescription.text = Mapping.ActionTitles[(int)_focusControl];
  }

  private void OnClickEventListener()
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

  private static void SetupKeyIntConversions()
  {
    _key_ValueToEnumIndex = new Dictionary<int, uint>();
    _key_EnumIndexToValue = new Dictionary<uint, int>();
    var keys = Enum.GetValues(typeof(OsHook_Keyboard.Key));
    for (uint i = 0; i < keys.Length; i++)
    {
      _key_ValueToEnumIndex[(int)keys.GetValue(i)] = i;
      _key_EnumIndexToValue[i] = (int)keys.GetValue(i);
    }
  }

  private void UpdateActionEditorUiFromActions()
  {
    var editing = _editing;
    _editing = false;

    __focus._actionDescriptionEditor.Text = Mapping.ActionTitles[(int)__focus._focusControl];

    var action = Action;
    var buttonAdded = false;
    for (var i = 0; i < _actionEditorUis.Length; i++)
    {
      if (action != null)
      {
        _actionEditorUis[i].gameObject.SetActive(true);
        _actionParameterEditorUis[i].gameObject.SetActive(action.Type.HasParameter());
        _actionEditorUis[i].Index = (uint)action.Type;
        _actionParameterEditorUis[i].Index = _key_ValueToEnumIndex[(int)action.Parameter];
        action = action.Next;
      }
      else
      {
        _actionEditorUis[i].Index = 0;
        _actionEditorUis[i].gameObject.SetActive(false);
        _actionParameterEditorUis[i].gameObject.SetActive(false);
        if (!buttonAdded)
        {
          for (var k = 0; k < _addActionButtons.Length; k++)
          {
            _addActionButtons[k].SetActive(k == (i - 1));
          }
          buttonAdded = true;
        }
      }
    }

    _editing = editing;
  }

  private void UpdateActionsFromActionEditorUi()
  {
    Mapping.ActionTitles[(int)__focus._focusControl] = __focus._actionDescriptionEditor.Text;
    __focus._actionDescription.text = __focus._actionDescriptionEditor.Text;

    var action = Action;
    for (var i = 0; i < _actionEditorUis.Length; i++)
    {
      if (action == null) { break; }
      action.Type = (ControllerAction.ActionType)_actionEditorUis[i].Index;
      action.Parameter = _key_EnumIndexToValue[_actionParameterEditorUis[i].Index];
      var hasNext =
        (i < _actionEditorUis.Length - 1) &&
        (_actionEditorUis[i + 1].Index != 0) &&
        (_actionEditorUis[i + 1].Index != Global.NullUint);
      action.Next = hasNext ? new ControllerAction() : null;
      action = action.Next;
    }
  }
}
