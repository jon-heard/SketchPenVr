using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Scrollbar : Control
  {
    [SerializeField] private Transform _geometry;
    [SerializeField] private Transform _draggerGeometry;

    [NonSerialized] public Action<uint> OnScrollValueChanged;

    private Control_Draggable _dragger;
    private Material _idleMaterial;

    public uint ScrollPosition
    {
      get { return _position; }
      set
      {
        if (value == _position) { return; }
        _position = value;
        OnScrollValueChanged?.Invoke(_position);
      }
    }
    private uint _position;

    public bool IsPartOfScroll(Control focus)
    {
      return focus == _dragger || focus == this;
    }

    public void SetRange(uint visibleCount, uint totalCount)
    {
      // Values
      _visibleCount = visibleCount;
      _totalCount = totalCount;
      ScrollPosition = 0;

      if (visibleCount >= totalCount)
      {
        gameObject.SetActive(false);
        return;
      }
      else
      {
        gameObject.SetActive(true);
      }

      // Dragger positions
      _draggerPositions = new float[_totalCount - _visibleCount + 1];
      var draggerSize = Mathf.Max(
        _const_MinDraggerSize,
        _geometry.transform.localScale.y * _visibleCount / _totalCount);
      var lowPosition  = (+draggerSize - _geometry.transform.localScale.y) / 2.0f;
      var highPosition = (-draggerSize + _geometry.transform.localScale.y) / 2.0f;
      for (var i = 0; i < _draggerPositions.Length; i++)
      {
        _draggerPositions[i] = Mathf.Lerp(highPosition, lowPosition, (float)i / _draggerPositions.Length);
      }

      // Visuals
      var t = _draggerGeometry.localScale;
      t.y = draggerSize;
      _draggerGeometry.localScale = t;
      if (_dragger == null)
      {
        _dragger = _draggerGeometry.parent.GetComponent<Control_Draggable>();
        _dragger.OnDragged += OnDragged;
      }
      _dragger.AxisScales = new Vector3(0.0f, 1.0f, 0.0f);
      _dragger.AxisClampLow.y = lowPosition;
      _dragger.AxisClampHigh.y = highPosition;

      // Update dragger position
      t = _dragger.transform.localPosition;
      t.y = _draggerPositions[ScrollPosition];
      _dragger.transform.localPosition = t;
    }

    private uint _visibleCount;
    private uint _totalCount;
    private float[] _draggerPositions;
    private Vector3 _pointerPosition;

    private static float _const_MinDraggerSize;

    private void Start()
    {
      _const_MinDraggerSize = App_Details.Instance.MyCommonDetails.SCROLLBAR_DRAGGER_MINSIZE;
      _idleMaterial = _geometry.GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
      Control.OnControlDown += OnControlDownEventHandler;
      Control.OnControlHovered += OnHoverEventListener;
      Control.OnControlUnhovered += OnUnhoverEventListener;
      Control.OnPointerMoved += OnPointerMovedEventHandler;
    }

    private void OnDisable()
    {
      Control.OnControlDown -= OnControlDownEventHandler;
      Control.OnPointerMoved -= OnPointerMovedEventHandler;
    }

    private void OnControlDownEventHandler(Control focus)
    {
      if (focus == this)
      {
        var clickedAboveDragger = transform.worldToLocalMatrix.MultiplyPoint(_pointerPosition).y > _draggerPositions[_position];
        if (clickedAboveDragger)
        {
          _position = (uint)Mathf.Max(0, _position - 10);
        }
        else
        {
          _position = (uint)Mathf.Min(_draggerPositions.Length - 1, _position + 10);
        }
        // Update dragger position
        var t = _dragger.transform.localPosition;
        t.y = _draggerPositions[ScrollPosition];
        _dragger.transform.localPosition = t;
        OnScrollValueChanged?.Invoke(_position);
      }
    }

    private void OnPointerMovedEventHandler(Vector3 point)
    {
      _pointerPosition = point;
    }

    private void OnHoverEventListener(Control focus)
    {
      if (focus == this)
      {
        _geometry.GetComponent<Renderer>().material =
          App_Resources.Instance.MyCommonResources.ScrollbarHoveredMaterial;
      }
    }

    private void OnUnhoverEventListener(Control focus)
    {
      if (focus == this)
      {
        _geometry.GetComponent<Renderer>().material = _idleMaterial;
      }
    }

    private void OnDragged(Vector3 draggerPosition)
    {
      var newScrollPosition = ScrollPosition;
      while (newScrollPosition > 0 && draggerPosition.y > _draggerPositions[newScrollPosition])
      {
        newScrollPosition -= 1;
      }
      while ((newScrollPosition < _draggerPositions.Length - 1) &&
             draggerPosition.y <= _draggerPositions[newScrollPosition + 1])
      {
        newScrollPosition += 1;
      }
      if (newScrollPosition != ScrollPosition)
      {
        ScrollPosition = newScrollPosition;
      }
    }
  }
}
