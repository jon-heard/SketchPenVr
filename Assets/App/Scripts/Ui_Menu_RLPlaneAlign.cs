using Common;
using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using Common.Vr.Ui.Popups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Menu_RLPlaneAlign : Ui_Menu
{
  [SerializeField] private GameObject[] _panels;
  [SerializeField] private Button[] _buttons;
  [SerializeField] private TextMesh _overPointUi;
  [SerializeField] private TextMesh _planePointUi;
  [SerializeField] private Button[] _buttonsToSimulate;
  [SerializeField] private Button _button_alignFinished;

  public override bool Show(Button source = null)
  {
    foreach (var panel in _panels)
    {
      panel.SetActive(false);
    }
    foreach (var button in _buttons)
    {
      button.State = Button.ButtonState.NotLockedDown;
    }
    _infoGatherType = InfoGatherType.None;
    return base.Show(source);
  }

  public override bool Hide()
  {
    _inputBlockingTicket?.StopListening();
    _inputBlockingTicket = null;
    return base.Hide();
  }

  public void OnAlignButton()
  {
    StartInfoGather(InfoGatherType.Align);
  }

  public void OnLightestPressureButton()
  {
    StartInfoGather(InfoGatherType.Lightest);
  }

  public void OnHeaviestPressureButton()
  {
    StartInfoGather(InfoGatherType.Heaviest);
  }

  public void OnCancelButton()
  {
    EndInfoGather();
  }

  public void OnAlignFinishButton()
  {
    var screenTransform = App_Functions.Instance.MyScreen.transform;
    var originalPosition = screenTransform.localPosition;
    var originalRotation = screenTransform.localRotation;
    var originalScale = screenTransform.localScale;
    var originalScreenLock = App_Functions.Instance.MyScreen.LockType;

    var maxDistanceSquared = 0.0f;

    // Position
    var center = Global.GetAverageVector(_points);
    screenTransform.position = center;

    // Rotation (find plane through least squares, then get triangle on plane, then get triangle's normal, then get rotation from normal)
    // NOTE: from the answer here: https://math.stackexchange.com/questions/1657030/fit-plane-to-3d-data-using-least-squares
    // 1. Calculate plane (least squares)
    double avgX, avgY, avgZ, avgSqrX, avgSqrY, avgSqrZ, avgMltXY, avgMltXZ, avgMltYZ;
    avgX = avgY = avgZ = avgSqrX = avgSqrY = avgSqrZ = avgMltXY = avgMltXZ = avgMltYZ = 0.0;
    foreach (var point in _points)
    {
      var distanceSquared = (point - center).sqrMagnitude;
      if (distanceSquared > maxDistanceSquared) { maxDistanceSquared = distanceSquared; }
      avgX += point.x;    avgY += point.y;    avgZ += point.z;
      avgSqrX += point.x * point.x;    avgSqrY += point.y * point.y;    avgSqrZ += point.z * point.z;
      avgMltXY += point.x * point.y;    avgMltXZ += point.x * point.z;    avgMltYZ += point.y * point.z;
    }
    avgX /= _points.Count;    avgY /= _points.Count;    avgZ /= _points.Count;
    avgSqrX /= _points.Count;   avgSqrY /= _points.Count;   avgSqrZ /= _points.Count;
    avgMltXY /= _points.Count;    avgMltXZ /= _points.Count;    avgMltYZ /= _points.Count;
    var denominator = ((avgSqrX - avgX * avgX) * (avgSqrY - avgY * avgY)) - ((avgMltXY - avgX * avgY) * (avgMltXY - avgX * avgY));
    var a = ((avgMltXZ - avgX * avgZ) * (avgSqrY - avgY * avgY) - (avgMltYZ - avgY * avgZ) * (avgMltXY - avgX * avgY)) / denominator;
    var b = ((avgSqrX - avgX * avgX) * (avgMltYZ - avgY * avgZ) - (avgMltXZ - avgX * avgZ) * (avgMltXY - avgX * avgY)) / denominator;
    var c = avgZ - a * avgX - b * avgY;
    // 2. Triangle on plane;
    var tri_p1 = new Vector3(0.0f, 0.0f, (float)c);
    var tri_p2 = new Vector3(10000.0f, 0.0f, (float)(a * 10000.0f + c));
    var tri_p3 = new Vector3(0.0f, 10000.0f, (float)(b * 10000.0f + c));
    // 3. Normal of triangle (and plane)
    //    NOTE: Get normal on both sides of triangle, then use overVector to pick which one to use
    var u = tri_p1 - tri_p2;
    var v = tri_p1 - tri_p3;
    var n1 = Vector3.Cross(u, v).normalized;
    var n2 = Vector3.Cross(v, u).normalized;
    var overVector = (tri_p1 - _overPoint).normalized;
    var a1 = Mathf.Acos(Vector3.Dot(n1, overVector));
    var a2 = Mathf.Acos(Vector3.Dot(n2, overVector));
    var normal = (a1 < a2) ? n1 : n2;
    // 4. Rotation
    screenTransform.rotation = Quaternion.FromToRotation(Vector3.forward, normal);

    // scale screen to points gathered
    var scale = Mathf.Sqrt(maxDistanceSquared) * 2.0f;
    screenTransform.localScale = new Vector3(scale, scale, scale);

    // Wrapup
    App_Functions.Instance.MyScreen.LockType = Screen.ScreenLockType.Plane;
    _alignConfirming = true;
    EndInfoGather();
    StartCoroutine(AlignTimeout());
    Confirm.ShowOnButton(_buttons[0], "Keep alignment?\nReverting in\n10 seconds!",
    (keep) =>
    {
      if (!keep)
      {
        screenTransform.localPosition = originalPosition;
        screenTransform.localRotation = originalRotation;
        screenTransform.localScale = originalScale;
        App_Functions.Instance.MyScreen.LockType = originalScreenLock;
      }
      _alignConfirming = false;
    });
  }
  private bool _alignConfirming = false;
  private IEnumerator AlignTimeout()
  {
    yield return new WaitForSeconds(10.0f);
    if (_alignConfirming) { Confirm.Instance.OnCancelButton(); }
  }

  private string[] _inputIdsToBlock =
  {
    "left_trigger", "left_grip", "left_high", "left_low", "left_thumbstick_down",
    "left_thumbstick_direction_xPos", "left_thumbstick_direction_xNeg",
    "left_thumbstick_direction_yPos", "left_thumbstick_direction_yNeg",
    "right_trigger", "right_grip", "right_high", "right_low", "right_thumbstick_down",
    "right_thumbstick_direction_xPos", "right_thumbstick_direction_xNeg",
    "right_thumbstick_direction_yPos", "right_thumbstick_direction_yNeg",
  };

  private App_Input _input;
  private enum InfoGatherType { Align = 0, Lightest = 1, Heaviest = 2, None = 3}
  private InfoGatherType _infoGatherType;
  private float _const_TriggerDownPressure;
  private float _const_AlignDataDistance;
  private Vector3 _overPoint;
  private List<Vector3> _points = new List<Vector3>();
  private bool _isTriggerDown;
  private InputManager.ListenerTicket _inputBlockingTicket;

  protected override void Start()
  {
    base.Start();
    _input = new App_Input();
    _input.Enable();
    _const_TriggerDownPressure = App_Details.Instance.TRIGGER_ACTIVATE_PRESSURE;
    _const_AlignDataDistance = App_Details.Instance.ALIGN_DATA_DISTANCE;
    var t = transform.localPosition;
    t.y = 0;
    transform.localPosition = t;
    foreach (var panel in _panels)
    {
      t = panel.transform.localPosition;
      t.y = 0;
      panel.transform.localPosition = t;
    }
  }

  private void Update()
  {
    if (_infoGatherType == InfoGatherType.None) { return; }
    var triggerPressure =
      Controller.IsLeftHanded ?
      _input.VrLeftHandActions.TriggerPressure.ReadValue<float>() :
      _input.VrRightHandActions.TriggerPressure.ReadValue<float>();
    Update_InfoGather(triggerPressure > _const_TriggerDownPressure);
  }

  private void StartInfoGather(InfoGatherType type)
  {
    _buttons[(int)type].State = Button.ButtonState.LockedDown;
    _panels[(int)type].SetActive(true);
    _inputBlockingTicket =
      App_Functions.Instance.MyInputManager.AddBlockingListeners(_inputIdsToBlock, 50);
    _isTriggerDown = true;
    if (type == InfoGatherType.Align)
    {
      App_Functions.Instance.MyScreen.LockType = Screen.ScreenLockType.None;
      _overPoint = Vector3.positiveInfinity;
      _points.Clear();
      _overPointUi.text = "Over point: ...";
      _planePointUi.text = "Plane point count: 0";
      _button_alignFinished.State = Button.ButtonState.Disabled;
    }
    _infoGatherType = type;
  }

  private void EndInfoGather()
  {
    _buttons[(int)_infoGatherType].State = Button.ButtonState.NotLockedDown;
    _panels[(int)_infoGatherType].SetActive(false);
    _infoGatherType = InfoGatherType.None;
    _inputBlockingTicket?.StopListening();
    _inputBlockingTicket = null;
  }

  private void Update_InfoGather(bool IsTriggerDown)
  {
    // Only react to new trigger down events
    if (IsTriggerDown == _isTriggerDown) { return; }
    _isTriggerDown = IsTriggerDown;

    // Simulate clicking specially assigned buttons
    if (_isTriggerDown)
    {
      var controllerFocus = Controller.PrimaryController.Focus;
      foreach (var button in _buttonsToSimulate)
      {
        if (controllerFocus?.transform?.parent?.gameObject == button.gameObject && button.State == Button.ButtonState.Hovered)
        {
          button.SimulateClick();
          return;
        }
      }
    }

    // React to the pencil point in a InfoGatherType specific way
    switch (_infoGatherType)
    {
      case InfoGatherType.Align:
        if (float.IsPositiveInfinity(_overPoint.x))
        {
          if (!_isTriggerDown) { return; }
          _overPointUi.text = "Over point: ²";
          _overPoint = Controller.PrimaryController.Pencil.transform.position;
        }
        else
        {
          StartCoroutine(AlignmentDrag());
        }
        break;
      case InfoGatherType.Lightest:
        {
          if (!_isTriggerDown) { return; }
          var screenTransform = App_Functions.Instance.MyScreen.transform;
          var distance = Vector3.Dot(
            Controller.PrimaryController.Pencil.transform.position - screenTransform.position,
            screenTransform.forward);
          App_Functions.Instance.MyScreen.LockType = Screen.ScreenLockType.None;
          screenTransform.position += screenTransform.forward * distance;
          App_Functions.Instance.MyScreen.LockType = Screen.ScreenLockType.Plane;
          EndInfoGather();
        }
        break;
      case InfoGatherType.Heaviest:
        {
          if (!_isTriggerDown) { return; }
          var screenTransform = App_Functions.Instance.MyScreen.transform;
          var distance = Mathf.Abs(Vector3.Dot(
            Controller.PrimaryController.Pencil.transform.position - screenTransform.position,
            screenTransform.forward));
          App_Details.Instance.PressureLength = distance;
          EndInfoGather();
        }
        break;
    }
  }

  private IEnumerator AlignmentDrag()
  {
    var latestPoint = Vector3.positiveInfinity;
    while (_isTriggerDown)
    {
      yield return null;
      var point = Controller.PrimaryController.Pencil.transform.position;
      if ((point - latestPoint).sqrMagnitude > _const_AlignDataDistance)
      {
        latestPoint = point;
        _points.Add(latestPoint);
        _planePointUi.text = "Plane point count: " + (_points.Count);
        if (_points.Count >= 3)
        {
          _button_alignFinished.State = Button.ButtonState.NotLockedDown;
        }
      }
    }
  }
}
