using System;
using UnityEngine;

public class ControllerVis : MonoBehaviour
{
  [SerializeField] private Renderer[] _toHideOnFull;
  [SerializeField] private ControllerTutorial _tutorial;

  [NonSerialized] public Collider MyCollider;

  public enum State { Hidden, Shadowed, Full }
  public State MyState
  {
    get { return _myState; }
    set
    {
      if (value == _myState) { return; }
      _myState = value;
      switch(_myState)
      {
        case State.Hidden:
          _myRenderer.material = App_Resources.Instance.ControllerVisHidden;
          break;
        case State.Shadowed:
          _myRenderer.material = App_Resources.Instance.ControllerVisShadowed;
          break;
        case State.Full:
          _myRenderer.material = App_Resources.Instance.ControllerVisFull;
          break;
      }
      _tutorial.gameObject.SetActive(_myState == State.Full);
      foreach (var toHide in _toHideOnFull)
      {
        toHide.enabled = (_myState != State.Full);
      }
    }
  }
  private State _myState;

  public ControllerMapping Mapping
  {
    get { return _mapping; }
    set
    {
      if (value == _mapping) { return; }
      if (_mapping != null) { _mapping.OnMappingChanged -= _tutorial.OnMappingChanged; }
      _mapping = value;
      if (_mapping != null)
      {
        _mapping.OnMappingChanged += _tutorial.OnMappingChanged;
        _tutorial.OnMappingChanged(_mapping);
      }
    }
  }
  private ControllerMapping _mapping;

  private Renderer _myRenderer;

  private void Start()
  {
    _myRenderer = GetComponent<Renderer>();
    MyCollider = GetComponent<Collider>();
    MyState = State.Shadowed;
  }
}
