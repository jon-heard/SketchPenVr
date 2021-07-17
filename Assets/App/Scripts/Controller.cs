 using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
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
    Up = ControllerMapping.Controls.ThumbUp,
    Right = ControllerMapping.Controls.ThumbRight,
    Down = ControllerMapping.Controls.ThumbDown,
    Left = ControllerMapping.Controls.ThumbLeft
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
  [SerializeField] private Renderer _pencil;
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
      _instances[0]._pencil.gameObject.SetActive(true);
      _instances[1]._pencil.gameObject.SetActive(false);
      _instances[0]._controllerIndex = 0;
      _instances[1]._controllerIndex = 1;
      _instances[0]._controllerVis.Mapping = _instances[0]._myControllerMapping =
        App_Details.Instance.MyControllerMappings.Mappings[0];
      _instances[1]._controllerVis.Mapping = _instances[1]._myControllerMapping =
        App_Details.Instance.MyControllerMappings.Mappings[1];
    }
  }
  private static Controller _mainController;
  private static Controller[] _instances = new Controller[2];
  private uint _controllerIndex = 0;

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
      var t = _instances[0]._pencil.transform.localEulerAngles;
      t.y = value ? 180.0f : 0.0f;
      _instances[0]._pencil.transform.localEulerAngles = t;
      t = _instances[0]._pencil.transform.localPosition;
      t.z = value ? _instances[0]._flippedZPosition : _instances[0]._zPosition;
      _instances[0]._pencil.transform.localPosition = t;
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
      if (_isHolding) { _held = _focus; } // Have separate var for held in case lost focus
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

  ///////////////////////////////
  // Emulated - separate hands //
  ///////////////////////////////
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
    Controller._const_distanceFullPressure = Controller._const_distanceTouch - length;
  }

  ////////////
  // Fields //
  ////////////
  private App_Input _input;
  private float _zPosition;
  private float _rayVisualZOffset;

  // User input
  private bool _isTriggerDown;
  private float _triggerPressure;
  private ThumbState _thumbState = ThumbState.Center;
  private bool _isPenActive = false;
  private bool _isNearFocus { get { return (_focusDistance <= _const_maxHoverDistance); } }

  // Focus info
  private Interactable _focus;
  private float _focusDistance;
  private Vector3 _focusPosition;
  public PointerEmulation FocusPointerEmulation { get; private set; }
  private ControllerVis _focusControllerVis;
  private InputHandler _inputHandler = new InputHandler();

  // Copies of values (for efficiency)
  private bool _isLeft;
  private Material _geometryMaterial;
  private ControllerMapping _myControllerMapping;
  private InputDevice? _myXrDevice;
  private static float _const_maxInteractDistance;
  private static float _const_TriggerDownPressure;
  private static float _const_ThumbDownPressure;
  private static float _const_maxHoverDistance;
  private static float _const_distanceTouch;
  private static float _const_distanceFullPressure;

  ////////////////////
  // Initialization //
  ////////////////////
  private void Awake()
  {
    // Copies of values init (for efficiency)
    _isLeft = transform.parent.GetComponent<Vr_Hand>().IsLeft;
    _geometryMaterial = _pencil.material;
    _const_maxInteractDistance = App_Details.Instance.MAX_INTERACT_DISTANCE;
    _const_TriggerDownPressure = App_Details.Instance.TRIGGER_DOWN_PRESSURE;
    _const_ThumbDownPressure = App_Details.Instance.THUMB_DOWN_PRESSURE;
    _const_maxHoverDistance = App_Details.Instance.CONTROLLER_DISTANCE_NEAR_SCREEN;
    _const_distanceTouch = App_Details.Instance.CONTROLLER_DISTANCE_TOUCH;
    _const_distanceFullPressure = App_Details.Instance.CONTROLLER_DISTANCE_FULL_PRESSURE;

    _controllerIndex = (uint)(_isLeft ? 0 : 1);
    _instances[_controllerIndex] = this;
  }

  private void Start()
  {
    // Fields init
    _zPosition = _pencil.transform.localPosition.z;
    _rayVisualZOffset = _rayVisual.transform.localPosition.z;

    // User input init
    _input = new App_Input();
    _input.Enable();
    if (_isLeft)
    {
      _mainController = this;
      _input.VrLeftHandActions.Grip.performed += OnGripButtonStart;
      _input.VrLeftHandActions.Grip.canceled += OnGripButtonEnd;
      _input.VrLeftHandActions.HighButton.performed += OnHighButtonStart;
      _input.VrLeftHandActions.HighButton.canceled += OnHighButtonEnd;
      _input.VrLeftHandActions.LowButton.performed += OnLowButtonStart;
      _input.VrLeftHandActions.LowButton.canceled += OnLowButtonEnd;
      _input.VrLeftHandActions.ThumbDown.performed += OnThumbButtonStart;
      _input.VrLeftHandActions.ThumbDown.canceled += OnThumbButtonEnd;
    }
    else
    {
      _input.VrRightHandActions.Grip.performed += OnGripButtonStart;
      _input.VrRightHandActions.Grip.canceled += OnGripButtonEnd;
      _input.VrRightHandActions.HighButton.performed += OnHighButtonStart;
      _input.VrRightHandActions.HighButton.canceled += OnHighButtonEnd;
      _input.VrRightHandActions.LowButton.performed += OnLowButtonStart;
      _input.VrRightHandActions.LowButton.canceled += OnLowButtonEnd;
      _input.VrRightHandActions.ThumbDown.performed += OnThumbButtonStart;
      _input.VrRightHandActions.ThumbDown.canceled += OnThumbButtonEnd;
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
      Debug.Log("No haptics: " + caps.supportsImpulse + " :: " + caps.supportsBuffer);
    }
  }

  //////////////////////////
  // Focus and draw logic //
  //////////////////////////
  private void Update()
  {
    // Input
    HandleTrigger();
    HandleThumbPressure();
#if UNITY_EDITOR
    HandleEmulatedButtons();
#endif

    // Logic
    HandleFocus();
    HandleNearControl();
  }
  private void HandleFocus()
  {
    _controllerVis.MyCollider.enabled = false;
    var hitInfo = new RaycastHit();
    if (Physics.Raycast(transform.position, transform.forward, out hitInfo) &&
        hitInfo.distance < _const_maxInteractDistance)
    {
      _focusDistance = hitInfo.distance;
      _focusPosition = hitInfo.point;
      _focus = hitInfo.transform.GetComponent<Interactable>();
      var focusParent = _focus?.transform?.parent;

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
      var newControllerVis = _focus?.GetComponent<ControllerVis>();
      if (newControllerVis != _focusControllerVis)
      {
        if (_focusControllerVis)
        {
          _focusControllerVis.MyState = ControllerVis.State.Shadowed;
        }
        _focusControllerVis = newControllerVis;
        if (_focusControllerVis)
        {
          _focusControllerVis.MyState = ControllerVis.State.Full;
        }
      }
    }
    else
    {
      _focusDistance = _const_maxInteractDistance;
      _focus = null;
      FocusPointerEmulation = null;
      _inputHandler.UpdatePointer(null, _isTriggerDown, Vector3.zero);
      if (_focusControllerVis)
      {
        _focusControllerVis.MyState = ControllerVis.State.Shadowed;
      }
      _focusControllerVis = null;
    }

    _rayVisual.SetPosition(1, new Vector3(0, 0, 1) * (_focusDistance - _rayVisualZOffset));
    _controllerVis.MyCollider.enabled = true;
  }

  private void HandleNearControl()
  {
    // Early out
    if (_controllerIndex > 0 || IsInGripAdjust) { return; }

    // Turn off pen if not near the screen or if holding
    if (!_isNearFocus || !FocusPointerEmulation || _isHolding)
    {
      if (_isPenActive)
      {
        _isPenActive = false;
        FocusPointerEmulation?.ClearPenState();
        _geometryMaterial.color = Color.white;
        _rayVisual.gameObject.SetActive(true);
        _controllerVis.MyState = ControllerVis.State.Shadowed;
      }
      return;
    }

    _rayVisual.gameObject.SetActive(false);
    _controllerVis.MyState = ControllerVis.State.Hidden;

    // Calc pen pressure
    var penPressure =
      (_focusDistance > _const_distanceTouch) ? 0.0f :
      (_focusDistance < _const_distanceFullPressure) ? 1.0f :
      1.0f - (_focusDistance - _const_distanceFullPressure) / (_const_distanceTouch - _const_distanceFullPressure);

    // Calc pen rotation
    var rotation =
      (uint)((int)(transform.eulerAngles.z - FocusPointerEmulation.transform.eulerAngles.z + 360)
      % 360);

    // Calc pen pressure trigger adjust
    var triggerAdjust = 1.0f;
    var geometryOpacity = 1.0f;
    if      (_triggerPressure > 0.9f) { }
    else if (_triggerPressure <= 0.0f && IsFlipped) { }
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
    _isPenActive = true;
    if (penPressure * triggerAdjust > 0.0f)
    {
      _myXrDevice?.SendHapticImpulse(0, 1.0f, 0.1f);
    }
  }

  /////////////////
  // User inputs //
  /////////////////
  private void HandleTrigger()
  {
    // Early out
    if (_isHolding && !IsInGripAdjust) { return; }

    // Get value (Analogue)
    _triggerPressure =
      _isLeft ?
      _input.VrLeftHandActions.TriggerPressure.ReadValue<float>() :
      _input.VrRightHandActions.TriggerPressure.ReadValue<float>();
#if UNITY_EDITOR
    if (App_Details.Instance.MyInputType == App_Details.InputType.VrSimulation)
    {
      _triggerPressure = _trigger;
    }
#endif

    // Get value (Binary)
    var hasTriggerDownChanged = false;
    if (_triggerPressure >= _const_TriggerDownPressure && !_isTriggerDown)
    {
      _isTriggerDown = true;
      hasTriggerDownChanged = true;
    }
    else if (_triggerPressure < _const_TriggerDownPressure && _isTriggerDown)
    {
      _isTriggerDown = false;
      hasTriggerDownChanged = true;
    }

    // React to value (binary)
    if (hasTriggerDownChanged)
    {
      // Is down
      if (_isTriggerDown)
      {
        // Screen
        if (FocusPointerEmulation)
        {
          FocusPointerEmulation.IsEmulating = true;
          if (_controllerIndex == 0 && !_isNearFocus)
          {
            FocusPointerEmulation.MouseLeftButton = true;
          }
        }
      }
      // Is up
      else
      {
        // Screen
        if (FocusPointerEmulation && _controllerIndex == 0 && !_isNearFocus)
        {
          FocusPointerEmulation.MouseLeftButton = false;
        }
      }
      // Trigger action for non-main controller
      if (_controllerIndex > 0)
      {
        _myControllerMapping.Actions[(int)ControllerMapping.Controls.Trigger].
          Run(this, _isTriggerDown);
      }
    }
  }
  private void HandleThumbPressure()
  {
    // Early out
    if (_isHolding) { return; }

    // Get value
    var pressure =
      _isLeft ?
      _input.VrLeftHandActions.ThumbPressure.ReadValue<Vector2>() :
      _input.VrRightHandActions.ThumbPressure.ReadValue<Vector2>();
#if UNITY_EDITOR
    if (App_Details.Instance.MyInputType == App_Details.InputType.VrSimulation)
    {
      pressure.x = _thumbLR;
      pressure.x = _thumbTB;
    }
#endif
    // Analogue to binary
    ThumbState newThumbState;
    newThumbState =
      (pressure.x > +_const_ThumbDownPressure && Mathf.Abs(pressure.x) > Mathf.Abs(pressure.y)) ? ThumbState.Right :
      (pressure.x < -_const_ThumbDownPressure && Mathf.Abs(pressure.x) > Mathf.Abs(pressure.y)) ? ThumbState.Left :
      (pressure.y > +_const_ThumbDownPressure) ? ThumbState.Up :
      (pressure.y < -_const_ThumbDownPressure) ? ThumbState.Down :
      ThumbState.Center;

    // React to value
    if (newThumbState != _thumbState)
    {
      if (_thumbState != ThumbState.Center)
      {
        _myControllerMapping.Actions[(int)_thumbState].Run(this, false);
      }
      _thumbState = newThumbState;
      if (_thumbState != ThumbState.Center)
      {
        _myControllerMapping.Actions[(int)_thumbState].Run(this, true);
      }
    }
  }
#if UNITY_EDITOR
  private void HandleEmulatedButtons()
  {
    if (App_Details.Instance.MyInputType == App_Details.InputType.VrSimulation)
    {
      var dummy = new CallbackContext();
      if (_highButton && !_prevHighButton) { OnHighButtonStart(dummy); }
      if (!_highButton && _prevHighButton) { OnHighButtonEnd(dummy); }
      if (_lowButton && !_prevLowButton) { OnLowButtonStart(dummy); }
      if (!_lowButton && _prevLowButton) { OnLowButtonEnd(dummy); }
      if (_gripButton && !_prevGripButton) { OnGripButtonStart(dummy); }
      if (!_gripButton && _prevGripButton) { OnGripButtonEnd(dummy); }
      if (_thumbButton && !_prevThumbButton) { OnThumbButtonStart(dummy); }
      if (!_thumbButton && _prevThumbButton) { OnThumbButtonEnd(dummy); }
      _prevHighButton = _highButton;
      _prevLowButton = _lowButton;
      _prevGripButton = _gripButton;
      _prevThumbButton = _thumbButton;
    }
  }
#endif
  private void OnGripButtonStart(CallbackContext obj)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.Controls.Grip].Run(this, true);
  }
  private void OnGripButtonEnd(CallbackContext obj)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.Controls.Grip].Run(this, false);
  }
  private void OnHighButtonStart(CallbackContext obj)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.Controls.HighButton].Run(this, true);
  }
  private void OnHighButtonEnd(CallbackContext obj)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.Controls.HighButton].Run(this, false);
  }
  private void OnLowButtonStart(CallbackContext obj)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.Controls.LowButton].Run(this, true);
  }
  private void OnLowButtonEnd(CallbackContext obj)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.Controls.LowButton].Run(this, false);
  }
  private void OnThumbButtonStart(CallbackContext obj)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.Controls.ThumbButton].Run(this, true);
  }
  private void OnThumbButtonEnd(CallbackContext obj)
  {
    _myControllerMapping.Actions[(int)ControllerMapping.Controls.ThumbButton].Run(this, false);
  }
}
