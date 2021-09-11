using Common.Vr.Ui.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionManager : MonoBehaviour
{
  public Button[] LeftButtons;
  public Button[] RightButtons;

  public void OnActionButtonClick(Button src)
  {
    // Get associated action
    var actionId = _mapButtonToAction[src];
    var action = actionId.Item1 ? _leftActions[actionId.Item2] : _rightActions[actionId.Item2];
    var controller = (actionId.Item1 == Controller.IsLeftHanded) ? Controller.PrimaryController : Controller.SecondaryController;
    action.Run(controller, true, 1.0f);
    action.Run(controller, false, 0.0f);
  }

  public void OnLeftMappingChanged(ControllerMapping mapping)
  {
    _leftActions = mapping.Actions;
    OnMappingChanged(mapping, LeftButtons);
  }
  public void OnRightMappingChanged(ControllerMapping mapping)
  {
    _rightActions = mapping.Actions;
    OnMappingChanged(mapping, RightButtons);
  }

  private Dictionary<Button, Tuple<bool, uint>> _mapButtonToAction = new Dictionary<Button, Tuple<bool, uint>>();
  private ControllerAction[] _leftActions;
  private ControllerAction[] _rightActions;

  private void Start()
  {
    for (uint i = 0; i < RightButtons.Length; i++)
    {
      _mapButtonToAction.Add(RightButtons[i], new Tuple<bool, uint>(false, i));
    }
    for (uint i = 0; i < LeftButtons.Length; i++)
    {
      _mapButtonToAction.Add(LeftButtons[i], new Tuple<bool, uint>(true, i));
    }
  }

  private void OnMappingChanged(ControllerMapping mapping, Button[] buttons)
  {
    for (var i = 0; i < mapping.Actions.Length; i++)
    {
      var title = mapping.ActionTitles[i];
      if (title != null && title != "")
      {
        buttons[i].Label.text = title;
        buttons[i].gameObject.SetActive(true);
      }
      else
      {
        buttons[i].gameObject.SetActive(false);
      }
    }
  }
}
