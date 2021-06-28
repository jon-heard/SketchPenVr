using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ui_Control_Button : MonoBehaviour
{
  // NOTE: "NotLockedDown" needed as "Idle" is set on unhover, so is blocked during "LockedDown"
  public enum ButtonState { Idle, Hovered, Down, Disabled, LockedDown, NotLockedDown };

  [SerializeField] protected UnityEvent OnClick;
  [Header("Wiring")]
  public Renderer Geometry;
  public TextMesh Label;

  public static List<Ui_Control_Button> Instances = new List<Ui_Control_Button>();

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
          var button = buttonTransform.GetComponent<Ui_Control_Button>();
          if (button == this) { continue; }
          button?.Locker.SetLock(App_Details.LOCK__OTHER_IS_LOCKED_DOWN, value == ButtonState.LockedDown);
        }
      }
      _state = value;
      RefreshVisual();
    }
  }
  private ButtonState _state = ButtonState.Idle;

  public ObjectLocker Locker { get; private set; }

  public void DoClick()
  {
    if (!Locker.IsLocked && this.State != ButtonState.LockedDown)
    {
      OnClick.Invoke();
    }
  }

  protected void RefreshVisual()
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

  protected virtual void Awake()
  {
    Locker = new ObjectLocker();
    Locker.LockStateChanged += OnLockStateChanged;
    Instances.Add(this);
    _idleMaterial = Geometry.material;
    _idleLabelMaterial = Label.GetComponent<Renderer>().material;
  }

  private Material _idleMaterial;
  private Material _idleLabelMaterial;

  private void OnLockStateChanged(bool isLocked)
  {
    RefreshVisual();
  }
}