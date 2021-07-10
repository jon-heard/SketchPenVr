using System;
using System.Collections;
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Control_Draggable : Control
  {
    public Vector3 DraggingOverage = new Vector3(10.0f, 10.0f, 1.0f);
    public Vector3 AxisScales = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 AxisClampLow = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    public Vector3 AxisClampHigh = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    [SerializeField] private BoxCollider Geometry;
    public Action<Vector3> OnDragged;

    private Vector3 _originalSize;
    private Vector3 _draggingSize;
    private Vector3 _point;
    private Vector3 _dragPoint;
    private Vector3 _dragStartPosition;
    private bool _isDragging;
    private bool _isHovering = true;

    private void Start()
    {
      _originalSize = Geometry.size;
      _draggingSize = _originalSize.GetScaled(DraggingOverage);
    }

    private void OnEnable()
    {
      Control.OnControlDown += OnDownEventListener;
      Control.OnControlUp += OnUpEventListener;
      Control.OnControlUnhovered += OnUnhoveredEventListener;
      Control.OnPointMoved += OnPointerMovedEventListener;
    }

    private void OnDisable()
    {
      Control.OnControlDown -= OnDownEventListener;
      Control.OnControlUp -= OnUpEventListener;
      Control.OnControlUnhovered -= OnUnhoveredEventListener;
      Control.OnPointMoved -= OnPointerMovedEventListener;
    }

    private void OnDownEventListener(Control focus)
    {
      if (focus == this)
      {
        Geometry.size = _draggingSize;
        _isDragging = true;
        _dragStartPosition = transform.localPosition;
        _dragPoint = transform.position - _point;
      }
    }

    private void OnUpEventListener(Control focus)
    {
      Geometry.size = _originalSize;
      _isDragging = false;
    }

    private void OnUnhoveredEventListener(Control focus)
    {
      if (focus == this && _isDragging)
      {
        transform.localPosition = _dragStartPosition;
        OnDragged?.Invoke(transform.localPosition);
      }
    }

    private void OnPointerMovedEventListener(Vector3 point)
    {
      if (point != _point)
      {
        _point = point;
        if (_isDragging)
        {
          var dragAdjust = (transform.position - _point) - _dragPoint;
          transform.position -= dragAdjust.GetScaled(AxisScales);
          transform.localPosition = transform.localPosition.ClampComponents(AxisClampLow, AxisClampHigh);
          OnDragged?.Invoke(transform.localPosition);
        }
      }
    }
  }
}
