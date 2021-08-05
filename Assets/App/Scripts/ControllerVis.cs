using System;
using UnityEngine;

public class ControllerVis : MonoBehaviour
{
  [SerializeField] private Renderer[] _toHideOnFull;
  [SerializeField] private ControllerTutorial _tutorial;

  [NonSerialized] public Collider MyCollider;

  public bool IsHidden
  {
    get { return _isHidden; }
    set
    {
      if (value == _isHidden) { return; }
      _isHidden = value;
      Refresh();
    }
  }
  private bool _isHidden = false;

  public bool IsFocused
  {
    get { return _isFocused; }
    set
    {
      if (value == _isFocused) { return; }
      _isFocused = value;
      Refresh();
    }
  }
  private bool _isFocused = false;

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
  private App_Input _input;
  private float _const_TriggerDownPressure;

  private void Start()
  {
    _myRenderer = GetComponent<Renderer>();
    MyCollider = GetComponent<Collider>();
    _input = new App_Input();
    _input.Enable();
    _const_TriggerDownPressure = App_Details.Instance.TRIGGER_ACTIVATE_PRESSURE;
    Refresh();
  }

  private void Update()
  {
    var triggerPressure =
      Controller.IsLeftHanded ?
      _input.VrLeftHandActions.TriggerPressure.ReadValue<float>() :
      _input.VrRightHandActions.TriggerPressure.ReadValue<float>();
    MyCollider.enabled = (triggerPressure < _const_TriggerDownPressure);
  }

  private void Refresh()
  {
    _myRenderer.material =
      IsFocused ? App_Resources.Instance.ControllerVisFull :
      IsHidden ? App_Resources.Instance.ControllerVisHidden :
      App_Resources.Instance.ControllerVisShadowed;
    _tutorial.gameObject.SetActive(IsFocused);
    foreach (var toHide in _toHideOnFull)
    {
      toHide.enabled = !IsFocused;
    }
  }
}
