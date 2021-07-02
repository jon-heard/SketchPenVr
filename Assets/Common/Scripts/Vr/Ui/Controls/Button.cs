using UnityEngine;
using UnityEngine.Events;

namespace Common.Vr.Ui.Controls
{
  public class Button : Control
  {
    // NOTE: "NotLockedDown" needed as "Idle" is set on unhover, so is blocked during "LockedDown"
    public enum ButtonState { Idle, Hovered, Down, Disabled, LockedDown, NotLockedDown };

    [SerializeField] protected UnityEvent OnClick;
    [Header("Wiring")]
    public Renderer Geometry;
    public TextMesh Label;

    public ButtonState State
    {
      get { return _state; }
      set
      {
        if (value == _state) { return; }
        // Hovered, Down & Idle states are blocked by Disabled and LockDown states
        if ((value == ButtonState.Hovered || value == ButtonState.Down || value == ButtonState.Idle) &&
            (_state == ButtonState.Disabled || _state == ButtonState.LockedDown))
        {
          return;
        }
        if (value == ButtonState.NotLockedDown) { value = ButtonState.Idle; }
        Locker.SetLock(App_Details.LOCK__DIRECT, value == ButtonState.Disabled);
        if ((value == ButtonState.LockedDown) != (_state == ButtonState.LockedDown))
        {
          foreach (Transform buttonTransform in transform.parent)
          {
            var button = buttonTransform.GetComponent<Button>();
            if (button == this) { continue; }
            button?.Locker.SetLock(App_Details.LOCK__OTHER_IS_LOCKED_DOWN, value == ButtonState.LockedDown);
          }
        }
        _state = value;
        RefreshVisual();
      }
    }
    private ButtonState _state = ButtonState.Idle;

    private Material _idleMaterial;
    private Material _idleLabelMaterial;
    private bool _isHovering;
    private bool _isDown;

    private void OnClickedEventListener(Control focus)
    {
      if (focus == this)
      {
        OnClick.Invoke();
      }
    }

    private void OnDownEventListener(Control focus)
    {
      if (focus == this)
      {
        State = ButtonState.Down;
        _isDown = true;
      }
    }

    private void OnUpEventListener(Control focus)
    {
      if (focus == this)
      {
        State = _isHovering ? ButtonState.Hovered : ButtonState.Idle;
        _isDown = false;
      }
    }

    private void OnHoveredEventListener(Control focus)
    {
      if (focus == this)
      {
        State = _isDown ? ButtonState.Down : ButtonState.Hovered;
        _isHovering = true;
      }
    }

    private void OnUnhoveredEventListener(Control focus)
    {
      if (focus == this)
      {
        State = ButtonState.Idle;
        _isHovering = false;
      }
    }

    private void RefreshVisual()
    {
      if (State == ButtonState.LockedDown)
      {
        Geometry.material = App_Resources.Instance.ButtonDownMaterial;
        Label.GetComponent<Renderer>().material = _idleLabelMaterial;
      }
      else if (Locker.IsLocked)
      {
        Geometry.material = _idleMaterial;
        Label.GetComponent<Renderer>().material = App_Resources.Instance.LabelDisabledMaterial;
      }
      else
      {
        switch (State)
        {
          case ButtonState.Idle:
            Geometry.material = _idleMaterial;
            break;
          case ButtonState.Hovered:
            Geometry.material = App_Resources.Instance.ButtonHoveredMaterial;
            break;
          case ButtonState.Down:
            Geometry.material = App_Resources.Instance.ButtonDownMaterial;
            break;
        }
        Label.GetComponent<Renderer>().material = _idleLabelMaterial;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      Locker.LockStateChanged += OnLockStateChanged;
      _idleMaterial = Geometry.material;
      _idleLabelMaterial = Label.GetComponent<Renderer>().material;
    }

    protected virtual void Start()
    {
      Control.OnControlClicked += OnClickedEventListener;
      Control.OnControlDown += OnDownEventListener;
      Control.OnControlUp += OnUpEventListener;
      Control.OnControlHovered += OnHoveredEventListener;
      Control.OnControlUnhovered += OnUnhoveredEventListener;
    }

    private void OnLockStateChanged(bool isLocked)
    {
      RefreshVisual();
    }
  }
}
