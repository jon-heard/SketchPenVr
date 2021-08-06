using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
  public enum NumericalActionType { scalar1 = 1, scalar2 = 4, scalar3 = 6 }

  public class ListenerTicket
  {
    private string _actionId;
    private uint _level;
    private Listener? _listener;
    private InputManager _manager;
    public ListenerTicket(string actionId, uint level, Listener? listener, InputManager manager)
    {
      _actionId = actionId;
      _level = level;
      _listener = listener;
      _manager = manager;
      _isListening = false;
    }
    public virtual bool IsListening
    {
      get { return _isListening; }
      set
      {
        if (value == _isListening) { return; }
        _isListening = value;
        if (_isListening)
        {
          _manager._listeners[_actionId].Add(_level, _listener.Value);
        }
        else
        {
          var listeners = _manager._listeners?[_actionId];
          var listenerIndex = listeners.IndexOfValue(_listener.Value);
          listeners.RemoveAt(listenerIndex);
        }
      }
    }
    protected bool _isListening;
  }

  public class ListenerTicketMulti : ListenerTicket
  {
    private ListenerTicket[] _subTickets;
    public ListenerTicketMulti(string[] actionIds, uint level, Listener[] listeners, InputManager manager) :
      base(null, 0, null, null)
    {
      if (actionIds.Length != listeners.Length)
      {
        Debug.LogError("ListenerTicketMulti created with different # of ids than listeners");
      }
      var count = Math.Min(actionIds.Length, listeners.Length);
      _subTickets = new ListenerTicket[count];
      for (var i = 0; i < count; i++)
      {
        _subTickets[i] = new ListenerTicket(actionIds[i], level, listeners[i], manager);
      }
    }
    public override bool IsListening
    {
      get { return _isListening; }
      set
      {
        if (value == _isListening) { return; }
        _isListening = value;
        for (var i = 0; i < _subTickets.Length; i++)
        {
          _subTickets[i].IsListening = _isListening;
        }
      }
    }
  }

  public struct Listener
  {
    public enum ListenerType { Block, Bool, Numerical }

    public Action<bool> EvtBool;
    public Action<bool, float> EvtNumerical;
    public ListenerType Type;
    public bool NotifyOnValueChanges;

    public Listener(Action<bool> evtBool)
    {
      EvtBool = evtBool;
      EvtNumerical = null;
      Type = (evtBool != null) ? ListenerType.Bool : ListenerType.Block;
      NotifyOnValueChanges = false;
    }
    public Listener(Action<bool, float> evtNumerical, bool notifyOnValueChanges)
    {
      EvtBool = null;
      EvtNumerical = evtNumerical;
      Type = (evtNumerical != null) ? ListenerType.Numerical : ListenerType.Block;
      NotifyOnValueChanges = (evtNumerical != null) ? notifyOnValueChanges : false;
    }
    public void Invoke(bool flag, float value)
    {
      switch (Type)
      {
        case ListenerType.Block:
          break;
        case ListenerType.Bool:
          EvtBool.Invoke(flag);
          break;
        case ListenerType.Numerical:
          EvtNumerical.Invoke(flag, value);
          break;
      }
    }
  }


  public ListenerTicketMulti AddBlockingListeners(string[] actionIds, uint level)
  {
    var listeners = new Listener[actionIds.Length];
    for (var i = 0; i < actionIds.Length; i++)
    {
      if (!_listeners.ContainsKey(actionIds[i]))
      {
        _listeners.Add(actionIds[i], new SortedList<uint, Listener>());
      }
      listeners[i] = new Listener(null);
    }
    var result = new ListenerTicketMulti(actionIds, level, listeners, this);
    result.IsListening = true;
    return result;
  }

  public ListenerTicket AddBooleanListener(string actionId, uint level, Action<bool> evt)
  {
    if (!_listeners.ContainsKey(actionId))
    {
      _listeners.Add(actionId, new SortedList<uint, Listener>());
    }
    var listener = new Listener(evt);
    var result = new ListenerTicket(actionId, level, listener, this);
    result.IsListening = true;
    return result;
  }

  public ListenerTicket AddNumericalListener(
    string actionId, uint level, Action<bool, float> evt, bool notifyOnValueChanges)
  {
    if (!_listeners.ContainsKey(actionId))
    {
      _listeners.Add(actionId, new SortedList<uint, Listener>());
    }
    var listener = new Listener(evt, notifyOnValueChanges);
    var result = new ListenerTicket(actionId, level, listener, this);
    result.IsListening = true;
    return result;
  }

  public void AddBooleanInput(string id, InputAction action)
  {
    _map_IdToAction.Add(id, action);
    _map_ActionToId.Add(action, id);
    action.started += BooleanInputOn;
    action.canceled += BooleanInputOff;
  }

  public void AddNumericalInput(
    string id, InputAction action,
    float threshold, NumericalActionType type = NumericalActionType.scalar1)
  {
    _map_IdToAction.Add(id, action);
    _map_ActionToId.Add(action, id);
    _numericalActions.Add(new ActionWrap(id, action, threshold, type));
  }

  private struct ActionWrap
  {
    public string Id;
    public InputAction Action;
    public float Threshold;
    public bool[] PriorFlags;
    public float[] PriorValues;
    public NumericalActionType Type;
    public ActionWrap(
      string id, InputAction action, float threshold, NumericalActionType type)
    {
      Id = id;
      Action = action;
      Threshold = threshold;
      PriorFlags = new bool[(int)type];
      PriorValues = new float[(int)type];
      Type = type;
    }
  }

  private enum SubAction { xPos, xNeg, yPos, yNeg, zPos, zNeg }

  private Dictionary<string, InputAction> _map_IdToAction = new Dictionary<string, InputAction>();
  private Dictionary<InputAction, string> _map_ActionToId = new Dictionary<InputAction, string>();
  private List<ActionWrap> _numericalActions = new List<ActionWrap>();
  private Dictionary<string, SortedList<uint, Listener>> _listeners =
    new Dictionary<string, SortedList<uint, Listener>>();

  private void BooleanInputOn(InputAction.CallbackContext obj)
  {
    _listeners?[_map_ActionToId[obj.action]]?.Values?[0].Invoke(true, 1.0f);
  }

  private void BooleanInputOff(InputAction.CallbackContext obj)
  {
    _listeners?[_map_ActionToId[obj.action]]?.Values?[0].Invoke(false, 0.0f);
  }

  private void Update()
  {
    for (var i = 0; i < _numericalActions.Count; i++)
    {
      var action = _numericalActions[i];
      switch (action.Type)
      {
        case NumericalActionType.scalar1:
          {
            if (_listeners.ContainsKey(action.Id) && _listeners[action.Id].Values.Count > 0)
            {
              var value = action.Action.ReadValue<float>();
              var flag = (value >= action.Threshold);
              var listener = _listeners[action.Id].Values?[0];
              if (!listener.HasValue) { break; }
              if ((listener?.NotifyOnValueChanges == true && value != action.PriorValues[0]) ||
                  (listener?.NotifyOnValueChanges != true && flag != action.PriorFlags[0]))
              {
                listener?.Invoke(flag, value);
                action.PriorFlags[0] = flag;
                action.PriorValues[0] = value;
              }
            }
          }
          break;
        case NumericalActionType.scalar2:
          {
            var rawValue = action.Action.ReadValue<Vector2>();
            var values = new float[(int)NumericalActionType.scalar2];
            var flags = new bool[(int)NumericalActionType.scalar2];
            if (Mathf.Abs(rawValue.x) > Mathf.Abs(rawValue.y))
            {
              if (rawValue.x > 0.0f)
              {
                values[(int)SubAction.xPos] = rawValue.x;
                values[(int)SubAction.xNeg] = 0.0f;
                flags[(int)SubAction.xPos] = (rawValue.x >= action.Threshold);
                flags[(int)SubAction.xNeg] = false;
              }
              else
              {
                values[(int)SubAction.xPos] = 0.0f;
                values[(int)SubAction.xNeg] = -rawValue.x;
                flags[(int)SubAction.xPos] = false;
                flags[(int)SubAction.xNeg] = (rawValue.x <= action.Threshold);
              }
              values[(int)SubAction.yPos] = values[(int)SubAction.yPos] = 0.0f;
              flags[(int)SubAction.yPos] = flags[(int)SubAction.yNeg] = false;
            }
            else if (Mathf.Abs(rawValue.y) > Mathf.Abs(rawValue.x))
            {
              if (rawValue.y > 0.0f)
              {
                values[(int)SubAction.yPos] = rawValue.y;
                values[(int)SubAction.yNeg] = 0.0f;
                flags[(int)SubAction.yPos] = (rawValue.y >= action.Threshold);
                flags[(int)SubAction.yNeg] = false;
              }
              else
              {
                values[(int)SubAction.yPos] = 0.0f;
                values[(int)SubAction.yNeg] = -rawValue.y;
                flags[(int)SubAction.yPos] = false;
                flags[(int)SubAction.yNeg] = (rawValue.y <= action.Threshold);
              }
              values[(int)SubAction.xPos] = values[(int)SubAction.xNeg] = 0.0f;
              flags[(int)SubAction.xPos] = flags[(int)SubAction.xNeg] = false;
            }
            else
            {
              values[(int)SubAction.xPos] = values[(int)SubAction.xNeg] =
              values[(int)SubAction.yPos] = values[(int)SubAction.yNeg] = 0.0f;
              flags[(int)SubAction.xPos] = flags[(int)SubAction.xNeg] =
              flags[(int)SubAction.yPos] = flags[(int)SubAction.yNeg] = false;
            }
            for (var k = 0; k < flags.Length; k++)
            {
              var id = action.Id + "_" + ((SubAction)k).ToString();
              if (!_listeners.ContainsKey(id) || _listeners[id].Values.Count == 0) { continue; }
              var listener = _listeners[id].Values?[0];
              if (!listener.HasValue) { break; }
              if ((listener?.NotifyOnValueChanges == true && values[k] != action.PriorValues[k]) ||
                  (listener?.NotifyOnValueChanges != true && flags[k] != action.PriorFlags[k]))
              {
                listener?.Invoke(flags[k], values[k]);
                action.PriorFlags[k] = flags[k];
                action.PriorValues[k] = values[k];
              }
            }
          }
          break;
        case NumericalActionType.scalar3:
          {
            var rawValue = action.Action.ReadValue<Vector3>();
            var values = new float[(int)NumericalActionType.scalar3];
            var flags = new bool[(int)NumericalActionType.scalar3];
            if (Mathf.Abs(rawValue.x) > Mathf.Abs(rawValue.y) && Mathf.Abs(rawValue.x) > Mathf.Abs(rawValue.z))
            {
              if (rawValue.x > 0.0f)
              {
                values[(int)SubAction.xPos] = rawValue.x;
                values[(int)SubAction.xNeg] = 0.0f;
                flags[(int)SubAction.xPos] = (rawValue.x >= action.Threshold);
                flags[(int)SubAction.xNeg] = false;
              }
              else
              {
                values[(int)SubAction.xPos] = 0.0f;
                values[(int)SubAction.xNeg] = -rawValue.x;
                flags[(int)SubAction.xPos] = false;
                flags[(int)SubAction.xNeg] = (rawValue.x <= action.Threshold);
              }
              values[(int)SubAction.yPos] = values[(int)SubAction.yNeg] =
              values[(int)SubAction.zPos] = values[(int)SubAction.zNeg] = 0.0f;
              flags[(int)SubAction.yPos] = flags[(int)SubAction.yNeg] =
              flags[(int)SubAction.zPos] = flags[(int)SubAction.zNeg] = false;
            }
            if (Mathf.Abs(rawValue.y) > Mathf.Abs(rawValue.x) && Mathf.Abs(rawValue.y) > Mathf.Abs(rawValue.z))
            {
              if (rawValue.y > 0.0f)
              {
                values[(int)SubAction.yPos] = rawValue.y;
                values[(int)SubAction.yNeg] = 0.0f;
                flags[(int)SubAction.yPos] = (rawValue.y >= action.Threshold);
                flags[(int)SubAction.yNeg] = false;
              }
              else
              {
                values[(int)SubAction.yPos] = 0.0f;
                values[(int)SubAction.yNeg] = -rawValue.y;
                flags[(int)SubAction.yPos] = false;
                flags[(int)SubAction.yNeg] = (rawValue.y <= action.Threshold);
              }
              values[(int)SubAction.xPos] = values[(int)SubAction.xNeg] =
              values[(int)SubAction.zPos] = values[(int)SubAction.zNeg] = 0.0f;
              flags[(int)SubAction.xPos] = flags[(int)SubAction.xNeg] =
              flags[(int)SubAction.zPos] = flags[(int)SubAction.zNeg] = false;
            }
            if (Mathf.Abs(rawValue.z) > Mathf.Abs(rawValue.x) && Mathf.Abs(rawValue.z) > Mathf.Abs(rawValue.y))
            {
              if (rawValue.z > 0.0f)
              {
                values[(int)SubAction.zPos] = rawValue.z;
                values[(int)SubAction.zNeg] = 0.0f;
                flags[(int)SubAction.zPos] = (rawValue.z >= action.Threshold);
                flags[(int)SubAction.zNeg] = false;
              }
              else
              {
                values[(int)SubAction.zPos] = 0.0f;
                values[(int)SubAction.zNeg] = -rawValue.z;
                flags[(int)SubAction.zPos] = false;
                flags[(int)SubAction.zNeg] = (rawValue.z <= action.Threshold);
              }
              values[(int)SubAction.xPos] = values[(int)SubAction.xNeg] =
              values[(int)SubAction.yPos] = values[(int)SubAction.yNeg] = 0.0f;
              flags[(int)SubAction.xPos] = flags[(int)SubAction.xNeg] =
              flags[(int)SubAction.yPos] = flags[(int)SubAction.yNeg] = false;
            }
            else
            {
              values[(int)SubAction.xPos] = values[(int)SubAction.xNeg] =
              values[(int)SubAction.yPos] = values[(int)SubAction.yNeg] =
              values[(int)SubAction.zPos] = values[(int)SubAction.zNeg] = 0.0f;
              flags[(int)SubAction.xPos] = flags[(int)SubAction.xNeg] =
              flags[(int)SubAction.yPos] = flags[(int)SubAction.yNeg] =
              flags[(int)SubAction.zPos] = flags[(int)SubAction.zNeg] = false;
            }
            for (var k = 0; k < flags.Length; k++)
            {
              var id = action.Id + "_" + ((SubAction)k).ToString();
              if (!_listeners.ContainsKey(id) || _listeners[id].Values.Count == 0) { continue; }
              var listener = _listeners[id].Values?[0];
              if (!listener.HasValue) { break; }
              if ((listener?.NotifyOnValueChanges == true && values[k] != action.PriorValues[k]) ||
                  (listener?.NotifyOnValueChanges != true && flags[k] != action.PriorFlags[k]))
              {
                listener?.Invoke(flags[k], values[k]);
                action.PriorFlags[k] = flags[k];
                action.PriorValues[k] = values[k];
              }
            }
          }
          break;
      }
    }
  }
}
