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

    public bool IsListeningForThumbstick
    {
      get { return _isListeningForThumbstick; }
      set
      {
        if (value == _isListeningForThumbstick) { return; }
        _isListeningForThumbstick = value;
        if (!value)
        {
          _listeningTicket_Left_ThumbstickUp.IsListening =
          _listeningTicket_Left_ThumbstickDown.IsListening =
          _listeningTicket_Right_ThumbstickUp.IsListening =
          _listeningTicket_Right_ThumbstickDown.IsListening = false;
        }
        else if (Control.WasPrimaryController == Controller.IsLeftHanded)
        {
          _listeningTicket_Left_ThumbstickUp.IsListening =
          _listeningTicket_Left_ThumbstickDown.IsListening = true;
          _listeningTicket_Right_ThumbstickUp.IsListening =
          _listeningTicket_Right_ThumbstickDown.IsListening = false;
        }
        else
        {
          _listeningTicket_Left_ThumbstickUp.IsListening =
          _listeningTicket_Left_ThumbstickDown.IsListening = false;
          _listeningTicket_Right_ThumbstickUp.IsListening =
          _listeningTicket_Right_ThumbstickDown.IsListening = true;
        }
      }
    }
    private bool _isListeningForThumbstick;

    private InputManager.ListenerTicket _listeningTicket_Left_ThumbstickUp;
    private InputManager.ListenerTicket _listeningTicket_Left_ThumbstickDown;
    private InputManager.ListenerTicket _listeningTicket_Right_ThumbstickUp;
    private InputManager.ListenerTicket _listeningTicket_Right_ThumbstickDown;
    private Vector3 _originalSize;
    private Vector3 _draggingSize;
    private Vector3 _primaryPoint, _secondaryPoint;
    private Vector3 _dragPoint;
    private Vector3 _dragStartPosition;
    private bool _isDragging;
    private bool _isDraggedByPrimary;
    private Material _idleMaterial;
    private float _const_thumbstickSpeed;

    private void Start()
    {
      _originalSize = Geometry.size;
      _draggingSize = _originalSize.GetScaled(DraggingOverage);
      _idleMaterial = Geometry.GetComponent<Renderer>().material;
      _const_thumbstickSpeed = App_Details.Instance.MyCommonDetails.DRAGGABLE_THUMBSTICK_SPEED;

      _listeningTicket_Left_ThumbstickUp = App_Functions.Instance.MyInputManager.AddNumericalListener("left_thumbstick_direction_yPos", 75, OnThumbstickUp, true);
      _listeningTicket_Left_ThumbstickUp.IsListening = false;
      _listeningTicket_Left_ThumbstickDown = App_Functions.Instance.MyInputManager.AddNumericalListener("left_thumbstick_direction_yNeg", 75, OnThumbstickDown, true);
      _listeningTicket_Left_ThumbstickDown.IsListening = false;
      _listeningTicket_Right_ThumbstickUp = App_Functions.Instance.MyInputManager.AddNumericalListener("right_thumbstick_direction_yPos", 75, OnThumbstickUp, true);
      _listeningTicket_Right_ThumbstickUp.IsListening = false;
      _listeningTicket_Right_ThumbstickDown = App_Functions.Instance.MyInputManager.AddNumericalListener("right_thumbstick_direction_yNeg", 75, OnThumbstickDown, true);
      _listeningTicket_Right_ThumbstickDown.IsListening = false;
    }

    private void OnEnable()
    {
      Control.OnControlDown += OnDownEventListener;
      Control.OnControlUp += OnUpEventListener;
      Control.OnControlHovered += OnHoveredEventListener;
      Control.OnControlUnhovered += OnUnhoveredEventListener;
      Control.OnPointerMoved += OnPointerMovedEventListener;
    }

    private void OnDisable()
    {
      Control.OnControlDown -= OnDownEventListener;
      Control.OnControlUp -= OnUpEventListener;
      Control.OnControlUnhovered += OnUnhoveredEventListener;
      Control.OnControlUnhovered -= OnUnhoveredEventListener;
      Control.OnPointerMoved -= OnPointerMovedEventListener;
    }

    private void OnDownEventListener(Control focus)
    {
      if (focus == this)
      {
        Geometry.size = _draggingSize;
        _dragStartPosition = transform.localPosition;
        _isDraggedByPrimary = Control.WasPrimaryController;
        _dragPoint =
          transform.localPosition -
          (_isDraggedByPrimary ? _primaryPoint : _secondaryPoint);
        _isDragging = true;
      }
    }

    private void OnUpEventListener(Control focus)
    {
      Geometry.size = _originalSize;
      _isDragging = false;
    }

    private void OnHoveredEventListener(Control focus)
    {
      if (focus == this)
      {
        Geometry.GetComponent<Renderer>().material =
          App_Resources.Instance.MyCommonResources.ButtonHoveredMaterial;
        IsListeningForThumbstick = true;
      }
    }

    private void OnUnhoveredEventListener(Control focus)
    {
      if (focus == this)
      {
        Geometry.GetComponent<Renderer>().material = _idleMaterial;
        IsListeningForThumbstick = false;
        if (_isDragging)
        {
          transform.localPosition = _dragStartPosition;
          OnDragged?.Invoke(transform.localPosition);
        }
      }
    }

    private void OnPointerMovedEventListener(Vector3 point)
    {
      if (point != (Control.WasPrimaryController ? _primaryPoint : _secondaryPoint))
      {
        if (Control.WasPrimaryController)
        {
          _primaryPoint = point;
        }
        else
        {
          _secondaryPoint = point;
        }
        if (_isDragging && Control.WasPrimaryController == _isDraggedByPrimary)
        {
          var dragAdjust = (transform.localPosition - point) - _dragPoint;
          transform.localPosition -= dragAdjust.GetScaled(AxisScales);
          transform.localPosition =
            transform.localPosition.ClampComponents(AxisClampLow, AxisClampHigh);
          OnDragged?.Invoke(transform.localPosition);
        }
      }
    }

    private void OnThumbstickUp(bool flag, float value)
    {
      var t = transform.localPosition;
      t.y += _const_thumbstickSpeed * value * Time.deltaTime;
      transform.localPosition = t;
      transform.localPosition =
        transform.localPosition.ClampComponents(AxisClampLow, AxisClampHigh);
      OnDragged?.Invoke(transform.localPosition);
    }

    private void OnThumbstickDown(bool flag, float value)
    {
      var t = transform.localPosition;
      t.y -= _const_thumbstickSpeed * value * Time.deltaTime;
      transform.localPosition = t;
      transform.localPosition =
        transform.localPosition.ClampComponents(AxisClampLow, AxisClampHigh);
      OnDragged?.Invoke(transform.localPosition);
    }
  }
}
