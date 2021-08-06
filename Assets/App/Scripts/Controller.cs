using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.InputSystem.InputAction;

public class Controller : MonoBehaviour
{
  ///////////
  // Types //
  ///////////
  public enum ThumbState
  {
    Center,
    Up = ControllerMapping.ControllerInput.ThumbUp,
    Right = ControllerMapping.ControllerInput.ThumbRight,
    Down = ControllerMapping.ControllerInput.ThumbDown,
    Left = ControllerMapping.ControllerInput.ThumbLeft
  }

  ///////////////
  // Inspector //
  ///////////////
#if UNITY_EDITOR
  [Header("Emulated controls")]
  [SerializeField] [Range(0.0f, 1.0f)] private float _trigger;
  [SerializeField] [Range(-1.0f, 1.0f)] private float _thumbLR;
  [SerializeField] [Range(-1.0f, 1.0f)] private float _thumbTB;
  [SerializeField] private bool _highButton;
  [SerializeField] private bool _lowButton;
  [SerializeField] private bool _gripButton;
  [SerializeField] private bool _thumbButton;
  private bool _prevHighButton;
  private bool _prevLowButton;
  private bool _prevGripButton;
  private bool _prevThumbButton;
#endif
  [Header("Parameters")]
  [SerializeField] private float _flippedZPosition;
  [Header("Wiring")]
  public Renderer Pencil;
  [SerializeField] private LineRenderer _rayVisual;
  [SerializeField] private PinchHandler _pinchHandler;
  [SerializeField] private ControllerVis _controllerVis;

  ////////////////
  // Handedness //
  ////////////////
  public static bool IsLeftHanded
  {
    get { return _instances[0]._isLeft; }
    set
    {
      if (value == IsLeftHanded) { return; }
      // Swap controller purposes
      var buf = _instances[0];
      _instances[0] = _instances[1];
      _instances[1] = buf;
      // Update controller infos to match purposes
      _instances[0].Pencil.gameObject.SetActive(true);
      _instances[1].Pencil.gameObject.SetActive(false);
      _instances[0]._controllerIndex = 0;
      _instances[1]._controllerIndex = 1;
      _instances[0]._controllerVis.Mapping = _instances[0]._myControllerMapping =
        App_Details.Instance.MyControllerMappings.Mappings[0];
      _instances[1]._controllerVis.Mapping = _instances[1]._myControllerMapping =
        App_Details.Instance.MyControllerMappings.Mappings[1];
    }
  }
  private static Controller[] _instances = new Controller[2];
  private uint _controllerIndex = 0;
  public static Controller PrimaryController { get { return _instances[0]; } }
  public static Controller SecondaryController { get { return _instances[1]; } }

  //////////////
  // Flipping //
  //////////////
  public static bool IsFlipped
  {
    get { return _isFlipped; }
    set
    {
      if (value == _isFlipped) { return; }
      _isFlipped = value;
      var t = _instances[0].Pencil.transform.localEulerAngles;
      t.y = value ? 180.0f : 0.0f;
      _instances[0].Pencil.transform.localEulerAngles = t;
      t = _instances[0].Pencil.transform.localPosition;
      t.z = value ? _instances[0]._flippedZPosition : _instances[0]._zPosition;
      _instances[0].Pencil.transform.localPosition = t;
    }
  }
  private static bool _isFlipped;

  /////////////
  // Holding //
  /////////////
  public bool IsHolding
  {
    get { return _isHolding; }
    set
    {
      if (value == _isHolding) { return; }
      if (_isTriggerDown && value) { return; }
      _isHolding = value;
      var screen = App_Functions.Instance.MyScreen;
      // Break hold logic if holding a locked screen
      if (_isHolding &&
          Focus?.DragInteractable?.gameObject == screen.gameObject &&
          screen._lockType == Screen.ScreenLockType.Full)
      {
        return;
      }
      if (_isHolding) { _held = Focus; } // Have separate var for held in case lost focus
      if (!IsInGripAdjust)
      {
        _held?.DragInteractable?.SetTemporaryParent(
          value ? transform.parent : null,
          value ? null : transform.parent);
        var otherInstance = _instances[_controllerIndex == 0 ? 1 : 0];
        if (_isHolding && otherInstance.IsHolding)
        {
          _instances[0]._pinchHandler.transform.position = _instances[0]._focusPosition;
          _instances[1]._pinchHandler.transform.position = _instances[1]._focusPosition;
          _instances[0]._pinchHandler.Focus = _held.DragInteractable;
          _instances[0]._pinchHandler.IsPinching = true;
        }
        else
        {
          _instances[0]._pinchHandler.IsPinching = false;
          if (IsHolding)
          {
            _held?.DragInteractable?.SetTemporaryParent(transform.parent);
          }
          else if (otherInstance.IsHolding)
          {
            _held?.DragInteractable?.SetTemporaryParent(otherInstance.transform.parent);
          }
        }
      }
      else
      {
        transform.parent = value ? _parent : null;
      }
    }
  }
  private bool _isHolding;

  public bool IsHoldingDesktop
  {
    get { return _isHoldingDesktop; }
    set
    {
      if (value == _isHoldingDesktop) { return; }
      _isHoldingDesktop = value;
      var originalFocus = Focus;
      Focus = App_Functions.Instance.MyScreen.GetComponent<Interactable>();
      IsHolding = value;
      Focus = originalFocus;
    }
  }
  private bool _isHoldingDesktop;

  private Interactable _held;

  /////////////////
  // Grip adjust //
  /////////////////
  public static bool IsInGripAdjust
  {
    get { return _isInGripAdjust; }
    set
    {
      if (value == _isInGripAdjust) { return; }
      _isInGripAdjust = value;
      if (_isInGripAdjust)
      {
        _parent = _instances[0].transform.parent;
        _instances[0].transform.parent = null;
        _instances[0]._isTriggerDown = false;
        _instances[0]._triggerPressure = 0.0f;
      }
      else
      {
        _instances[0].transform.parent = _parent;
      }
    }
  }
  private static bool _isInGripAdjust;
  private static Transform _parent;
  public static void AcceptGripAdjust()
  {
    IsInGripAdjust = false;
    var gripAdjustPrefsValue =
      _instances[0].GetComponent<TransformSerializer>().SerializedTransform;
    PlayerPrefs.SetString(_instances[0].controllerTransformPrefsKey, gripAdjustPrefsValue);
  }
  public static void CancelGripAdjust()
  {
    IsInGripAdjust = false;
    _instances[0].GetComponent<TransformSerializer>().SerializedTransform =
      PlayerPrefs.GetString(_instances[0].controllerTransformPrefsKey);
  }
  public static void ResetGripAdjust()
  {
    IsInGripAdjust = false;
    _instances[0].GetComponent<TransformSerializer>().SerializedTransform = "";
    var gripAdjustPrefsValue =
      _instances[0].GetComponent<TransformSerializer>().SerializedTransform;
    PlayerPrefs.SetString(_instances[0].controllerTransformPrefsKey, gripAdjustPrefsValue);
  }
  private string controllerTransformPrefsKey
  {
    get
    {
      return App_Details.CFG__CONTROLLER_TRANSFORM.Replace("%1", _isLeft ? "Left" : "right");
    }
  }

  ///////////////////////////////////
  // Emulated - separate the hands //
  ///////////////////////////////////
  public static IEnumerator SeparateControllers()
  {
    yield return new WaitForSeconds(App_Details.Instance.TIMESPAN_BEFORE_SETTING_SCREEN_HEIGHT);
    foreach (var instance in _instances)
    {
      var t = instance.transform.parent.localPosition;
      t.x =
        App_Details.Instance.CONTROLLER_EMULATED_SEPARATION * (instance._isLeft ? -1.0f : +1.0f);
      instance.transform.parent.localPosition = t;
    }
  }

  //////////////////////////
  // AdjustPressureLength //
  //////////////////////////
  public static void SetPressureLength(float length)
  {
    Controller._const_distance_tipBase = Controller._const_distance_tipPoint - length;
  }

  ////////////
  // Fields //
  ////////////
  private App_Input _input;
  private float _zPosition;
  private float _rayVisualZOffset;
  private bool _isDrawing;

  // User input
  private bool _isTriggerDown;
  private float _triggerPressure;
  private bool _isPenActive = false;
  private bool _isPenMode
  {
    get { return (FocusPointerEmulation && _focusDistance <= _const_maxHoverDistance); }
  }

  // Focus info
  public Interactable Focus { get; private set; }
  private float _focusDistance;
  private Vector3 _focusPosition;
  public PointerEmulation FocusPointerEmulation { get; private set; }
  private ControllerVis _focusControllerVis;
  private UiInputHandler _inputHandler = new UiInputHandler();

  // Copies of values (for efficiency)
  private bool _isLeft;
  private Material _geometryMaterial;
  private ControllerMapping _myControllerMapping;
  public InputDevice? _myXrDevice;
  private static float _const_maxInteractDistance;
  private static float _const_maxHoverDistance;
  private static float _const_distance_tipPoint;
  private static float _const_distance_tipBase;
  private static float _const_rumbleStrength_hard;
  private static float _const_rumbleStrength_medium;
  private static float _const_rumbleStrength_light;

  ////////////////////
  // Initialization //
  ////////////////////
  private void Awake()
  {
    // Copies of values init (for efficiency)
    _isLeft = transform.parent.GetComponent<Vr_Hand>().IsLeft;
    _geometryMaterial = Pencil.material;
    _const_maxInteractDistance = App_Details.Instance.MAX_INTERACT_DISTANCE;
    _const_maxHoverDistance = App_Details.Instance.CONTROLLER_DISTANCE_NEAR_SCREEN;
    _const_distance_tipPoint = App_Details.Instance.CONTROLLER_DISTANCE_TIP_POINT;
    _const_distance_tipBase = App_Details.Instance.CONTROLLER_DISTANCE_TIP_BASE;
    _const_rumbleStrength_hard = App_Details.Instance.RUMBLE_STRENGTH_HARD;
    _const_rumbleStrength_medium = App_Details.Instance.RUMBLE_STRENGTH_MEDIUM;
    _const_rumbleStrength_light = App_Details.Instance.RUMBLE_STRENGTH_LIGHT;

    _controllerIndex = (uint)(_isLeft ? 0 : 1);
    _instances[_controllerIndex] = this;
  }

  private void Start()
  {
    // Fields init
    _zPosition = Pencil.transform.localPosition.z;
    _rayVisualZOffset = _rayVisual.transform.localPosition.z;

    // User input init
    _input = new App_Input();
    _input.Enable();
    if (_isLeft)
    {
      App_Functions.Instance.MyInputManager.AddNumericalListener("left_trigger", 100, OnTrigger, true);
      App_Functions.Instance.MyInputManager.AddBooleanListener("left_grip", 100, OnGripButton);
      App_Functions.Instance.MyInputManager.AddBooleanListener("left_high", 100, OnHighButton);
      App_Functions.Instance.MyInputManager.AddBooleanListener("left_low", 100, OnLowButton);
      App_Functions.Instance.MyInputManager.AddBooleanListener("left_thumbstick_down", 100, OnThumbstickButton);
      App_Functions.Instance.MyInputManager.AddNumericalListener("left_thumbstick_direction_yPos", 100, OnThumbstickUp, true);
      App_Functions.Instance.MyInputManager.AddNumericalListener("left_thumbstick_direction_xPos", 100, OnThumbstickRight, true);
      App_Functions.Instance.MyInputManager.AddNumericalListener("left_thumbstick_direction_yNeg", 100, OnThumbstickDown, true);
      App_Functions.Instance.MyInputManager.AddNumericalListener("left_thumbstick_direction_xNeg", 100, OnThumbstickLeft, true);
    }
    else
    {
      App_Functions.Instance.MyInputManager.AddNumericalListener("right_trigger", 100, OnTrigger, true);
      App_Functions.Instance.MyInputManager.AddBooleanListener("right_grip", 100, OnGripButton);
      App_Functions.Instance.MyInputManager.AddBooleanListener("right_high", 100, OnHighButton);
      App_Functions.Instance.MyInputManager.AddBooleanListener("right_low", 100, OnLowButton);
      App_Functions.Instance.MyInputManager.AddBooleanListener("right_thumbstick_down", 100, OnThumbstickButton);
      App_Functions.Instance.MyInputManager.AddNumericalListener("right_thumbstick_direction_yPos", 100, OnThumbstickUp, true);
      App_Functions.Instance.MyInputManager.AddNumericalListener("right_thumbstick_direction_xPos", 100, OnThumbstickRight, true);
      App_Functions.Instance.MyInputManager.AddNumericalListener("right_thumbstick_direction_yNeg", 100, OnThumbstickDown, true);
      App_Functions.Instance.MyInputManager.AddNumericalListener("right_thumbstick_direction_xNeg", 100, OnThumbstickLeft, true);
    }

    // Grip adjust init
    GetComponent<TransformSerializer>().SerializedTransform =
      PlayerPrefs.GetString(controllerTransformPrefsKey);

    // Pinching init
    _pinchHandler.Other = _instances[_controllerIndex == 0 ? 1 : 0]._pinchHandler;

    // ControllerVis init
    _controllerVis.Mapping = _myControllerMapping =
      App_Details.Instance.MyControllerMappings.Mappings[_controllerIndex];

    // Haptic feedback init
    _myXrDevice = InputDevices.GetDeviceAtXRNode(_isLeft ? XRNode.LeftHand : XRNode.RightHand);
    HapticCapabilities caps;
    if (!_myXrDevice.Value.TryGetHapticCapabilities(out caps) || !caps.supportsImpulse)
    {
      _myXrDevice = null;
      Common.Vr.Ui.Controls.Console.Print(
        "No haptics: " + caps.supportsImpulse + " :: " + caps.supportsBuffer);
    }
  }

  //////////////////////////
  // Focus and draw logic //
  //////////////////////////
  private void LateUpdate()
  {
    _rayVisual.SetPosition(1, new Vector3(0, 0, 1) * (_focusDistance - _rayVisualZOffset));
  }
  private void Update()
  {
    // Input
#if UNITY_EDITOR
    Update_EmulatedButtons();
#endif

    // Logic
    Update_Focus();
    Update_NearControl();
  }
  private void Update_Focus()
  {
    var originalEnabled = _controllerVis.MyCollider.enabled;
    _controllerVis.MyCollider.enabled = false;
    var hitInfo = new RaycastHit();
    if (Physics.Raycast(transform.position, transform.forward, out hitInfo) &&
        hitInfo.distance < _const_maxInteractDistance)
    {
      _focusDistance = hitInfo.distance;
      _focusPosition = hitInfo.point;
      Focus = hitInfo.transform.GetComponent<Interactable>();
      var focusParent = Focus?.transform?.parent;

      // Screen focus
      var newFocusPointerEmulation = focusParent?.GetComponent<PointerEmulation>();
      if (newFocusPointerEmulation != FocusPointerEmulation)
      {
        if (FocusPointerEmulation) { FocusPointerEmulation.OnLostFocus(); }
        FocusPointerEmulation = newFocusPointerEmulation;
      }
      // Send focus position (if screen is in focus)
      if (_controllerIndex == 0 && FocusPointerEmulation)
      {
        FocusPointerEmulation.Position = hitInfo.textureCoord;
        FocusPointerEmulation.Distance = _focusDistance;
      }

      // Control focus
      _inputHandler.UpdatePointer(focusParent?.GetComponent<Control>(), _isTriggerDown, hitInfo.point);

      // ControllerVis focus
      var newControllerVis = Focus?.GetComponent<ControllerVis>();
      if (newControllerVis != _focusControllerVis)
      {
        if (_focusControllerVis)
        {
          _focusControllerVis.IsFocused = false;
        }
        _focusControllerVis = newControllerVis;
        if (_focusControllerVis)
        {
          _focusControllerVis.IsFocused = true;
        }
      }
    }
    else
    {
      _focusDistance = _const_maxInteractDistance;
      Focus = null;
      FocusPointerEmulation?.SetPenState(0.0f, 0, Vector2.zero, Controller.IsFlipped);
      FocusPointerEmulation = null;
      _inputHandler.UpdatePointer(null, _isTriggerDown, Vector3.zero);
      if (_focusControllerVis)
      {
        _focusControllerVis.IsFocused = false;
      }
      _focusControllerVis = null;
    }
    _controllerVis.MyCollider.enabled = originalEnabled;
  }

  private void DoSnapRumble()
  {
    var rumbleStrength = App_Details.Instance.RumbleStrength;
    if (rumbleStrength != App_Details.RumbleStrengthType.None)
    {
      var a =
        (rumbleStrength == App_Details.RumbleStrengthType.Hard) ? _const_rumbleStrength_hard :
        (rumbleStrength == App_Details.RumbleStrengthType.Medium) ? _const_rumbleStrength_medium :
        _const_rumbleStrength_light;
      _myXrDevice?.SendHapticImpulse(0, a, 0.1f);
      _myXrDevice?.SendHapticImpulse(1, a, 0.1f);
      _myXrDevice?.SendHapticImpulse(2, a, 0.1f);
      _myXrDevice?.SendHapticImpulse(3, a, 0.1f);
    }
  }

  private void Update_NearControl()
  {
    // Early out
    if (_controllerIndex > 0 || IsInGripAdjust) { return; }

    // Turn off pen if not in pen mode or if holding
    if (!_isPenMode || _isHolding)
    {
      if (_isPenActive)
      {
        _isPenActive = false;
        if (_isDrawing)
        {
          _isDrawing = false;
          DoSnapRumble();
        }
        FocusPointerEmulation?.ClearPenState();
        _geometryMaterial.color = Color.white;
        _controllerVis.IsHidden = false;
        _rayVisual.gameObject.SetActive(true);
        _isTriggerDown = false; // force mouse down if trigger is pushed when leaving near-zone

        _instances[0]._controllerVis.Mapping = _instances[0]._myControllerMapping =
                App_Details.Instance.MyControllerMappings.Mappings[0];
      }
      return;
    }
    else
    {
      if (!_isPenActive)
      {
        _isPenActive = true;
        FocusPointerEmulation.MouseLeftButton = false;
        _instances[0]._controllerVis.Mapping = _instances[0]._myControllerMapping =
                App_Details.Instance.MyControllerMappings.Mappings[2];
        _controllerVis.IsHidden = true;
      }
    }

    _rayVisual.gameObject.SetActive(false);

    // Calc pen pressure
    var penPressure =
      (_focusDistance > _const_distance_tipPoint) ? 0.0f :
      (_focusDistance < _const_distance_tipBase) ? 1.0f :
      1.0f - (_focusDistance - _const_distance_tipBase) / (_const_distance_tipPoint - _const_distance_tipBase);

    // Calc pen rotation
    var rotation =
      (uint)((int)(FocusPointerEmulation.transform.eulerAngles.z - transform.eulerAngles.z + 360)
      % 360);

    // Calc pen pressure trigger adjust
    var triggerAdjust = 1.0f;
    var geometryOpacity = 1.0f;
    if      (_triggerPressure > 0.9f) { }
    else if (_triggerPressure <= 0.0f && IsFlipped) { } // Erase without needing trigger down
    else if (_triggerPressure <= 0.0f) { triggerAdjust = 0.0f; geometryOpacity = 1.0f; }
    else if (_triggerPressure > 0.3f) { triggerAdjust = 0.6f; geometryOpacity = 0.6f; }
    else { triggerAdjust = 0.3f; geometryOpacity = 0.2f; }

    // Calc tilt
    var forward = FocusPointerEmulation.transform.InverseTransformDirection(transform.forward);
    var tilt = new Vector2(
      Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg * -1.0f,
      Mathf.Atan2(forward.y, forward.z) * Mathf.Rad2Deg);

    // Visualize trigger adjust on geometry
    _geometryMaterial.SetFloat("_Opacity", geometryOpacity);

    // Update pen
#if UNITY_EDITOR
    if (App_Details.Instance.MyInputType == App_Details.InputType.Vr)
#endif
    {
      FocusPointerEmulation.SetPenState(
        penPressure * triggerAdjust, rotation, tilt, Controller.IsFlipped);
    }

    // Calc _isDrawing
    var newIsDrawing = (penPressure * triggerAdjust) > 0.0f;
    if (newIsDrawing != _isDrawing)
    {
      _isDrawing = newIsDrawing;
      DoSnapRumble();
    }
  }

  /////////////////
  // User inputs //
  /////////////////
#if UNITY_EDITOR
  private void Update_EmulatedButtons()
  {
    if (App_Details.Instance.MyInputType == App_Details.InputType.VrSimulation)
    {
      OnTrigger(_trigger > App_Details.Instance.TRIGGER_ACTIVATE_PRESSURE, _trigger);
      if (_gripButton != _prevHighButton) { OnGripButton(_gripButton); }
      if (_highButton != _prevHighButton) { OnHighButton(_highButton); }
      if (_lowButton != _prevLowButton) { OnLowButton(_lowButton); }
      if (_thumbButton != _prevThumbButton) { OnThumbstickButton(_thumbButton); }
      _prevHighButton = _highButton;
      _prevLowButton = _lowButton;
      _prevGripButton = _gripButton;
      _prevThumbButton = _thumbButton;
    }
  }
#endif

  private void OnTrigger(bool flag, float value)
  {
    if (_isHolding && !IsInGripAdjust) { return; }
    if (flag != _isTriggerDown)
    {
      _isTriggerDown = flag;
      // Primary controller - Update screen based on input
      if (_controllerIndex == 0)
      {
        if (FocusPointerEmulation)
        {
          // In mouse-mode, have trigger press/release left mouse button
          if (!_isPenMode)
          {
            FocusPointerEmulation.MouseLeftButton = flag;
          }
          // Reestablish emulation if it was disabled (through escape key)
          FocusPointerEmulation.IsEmulating = true;
        }
      }
      // Secondary controller - Update action from mapping
      else
      {
        _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.Trigger].
          Run(this, flag);
      }
    }
    _triggerPressure = value;
  }
  private void OnGripButton(bool value)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.Grip].Run(this, value);
  }
  private void OnHighButton(bool value)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.HighButton].Run(this, value);
  }
  private void OnLowButton(bool value)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.LowButton].Run(this, value);
  }
  private void OnThumbstickButton(bool value)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.ThumbButton].Run(this, value);
  }
  private void OnThumbstickUp(bool flag, float value)
  {
    if (_isHolding) { return; }
    _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.ThumbUp].
      Run(this, flag, value);
  }
  private void OnThumbstickRight(bool flag, float value)
  {
    if (_isHolding) { return; }
    _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.ThumbRight].
      Run(this, flag, value);
  }
  private void OnThumbstickDown(bool flag, float value)
  {
    if (_isHolding) { return; }
    _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.ThumbDown].
      Run(this, flag, value);
  }
  private void OnThumbstickLeft(bool flag, float value)
  {
    if (_isHolding) { return; }
    _myControllerMapping.Actions[(int)ControllerMapping.ControllerInput.ThumbLeft].
      Run(this, flag, value);
  }
}
