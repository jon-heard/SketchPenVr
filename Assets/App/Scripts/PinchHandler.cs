using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchHandler : MonoBehaviour
{
  [System.NonSerialized] public PinchHandler Other;
  [System.NonSerialized] public Interactable Focus;

  public bool IsPinching
  {
    get { return _isPinching; }
    set
    {
      if (value == _isPinching) { return; }
      _isPinching = value;
      if (_isPinching)
      {
        _startPinchMagnitude = (transform.position - Other.transform.position).magnitude;
        _startScale = Focus.transform.localScale;
      }
    }
  }
  private bool _isPinching;

  private float _startPinchMagnitude;
  private Vector3 _startScale;

  private float _minScaleSize;

  private void Start()
  {
    _minScaleSize = App_Details.Instance.MIN_SCALE_SIZE;
  }

  private void Update()
  {
    if (!IsPinching) { return; }
    var currentPinch = transform.position - Other.transform.position;
    Focus.transform.localScale = _startScale * (currentPinch.magnitude / _startPinchMagnitude);
    if (Focus.transform.localScale.x < _minScaleSize)
    {
      Focus.transform.localScale *= (_minScaleSize / Focus.transform.localScale.x);
    }
  }
}