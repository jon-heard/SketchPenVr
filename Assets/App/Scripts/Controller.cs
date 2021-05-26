using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Controller : MonoBehaviour
{
  ///////////
  // Types //
  ///////////
  public enum ThumbState { Up, Right, Down, Left, Center }

  ///////////////
  // Inspector //
  ///////////////
#if UNITY_EDITOR
  [Header("Emulated controls")]
  [SerializeField] [Range(0.0f, 1.0f)] private float _trigger;
  [SerializeField] [Range(-1.0f, 1.0f)] private float _thumbLR;
  [SerializeField] [Range(-1.0f, 1.0f)] private float _thumbTB;
  [SerializeField] private bool _topButton;
  [SerializeField] private bool _bottomButton;
  [SerializeField] private bool _gripButton;
  [SerializeField] private bool _thumbButton;
  private bool _prevTopButton;
  private bool _prevBottomButton;
  private bool _prevGripButton;
  private bool _prevThumbButton;
#endif
  [Header("Parameters")]
  [SerializeField] private float _flippedZPosition;
  [Header("Wiring")]
  [SerializeField] private Renderer _geometry;
  [SerializeField] private LineRenderer _rayVisual;

  /////////
  // Map //
  /////////
  public static ControllerMapping[] Mappings = new ControllerMapping[2];

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
      _instances[0]._geometry.gameObject.SetActive(true);
      _instances[1]._geometry.gameObject.SetActive(false);
      _instances[0]._controllerIndex = 0;
      _instances[1]._controllerIndex = 1;
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
      var t = _instances[0]._geometry.transform.localEulerAngles;
      t.y = value ? 180.0f : 0.0f;
      _instances[0]._geometry.transform.localEulerAngles = t;
      t = _instances[0]._geometry.transform.localPosition;
      t.z = value ? _instances[0]._flippedZPosition : _instances[0]._zPosition;
      _instances[0]._geometry.transform.localPosition = t;
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
      if (!IsInGripAdjust)
      {
        _focus?.DragInteractable?.SetTemporaryParent(
          value ? transform.parent : null,
          value ? null : transform.parent);
      }
      else
      {
        transform.parent = value ? _parent : null;
      }
    }
  }
  private bool _isHolding;

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
        var gripAdjustPrefsValue =
          _instances[0].GetComponent<TransformSerializer>().SerializedTransform;
        PlayerPrefs.SetString(_instances[0].controllerTransformPrefsKey, gripAdjustPrefsValue);
      }
    }
  }
  private static bool _isInGripAdjust;
  private static Transform _parent;
  public static void CancelGripAdjust()
  {
    _instances[0].GetComponent<TransformSerializer>().SerializedTransform =
      PlayerPrefs.GetString(_instances[0].controllerTransformPrefsKey);
    IsInGripAdjust = false;
  }
  public static void ResetGripAdjust()
  {
    _instances[0].GetComponent<TransformSerializer>().SerializedTransform = "";
    IsInGripAdjust = false;
  }
  private string controllerTransformPrefsKey
  {
    get
    {
      return App_Details.CFG__CONTROLLER_TRANSFORM.Replace("%1", _isLeft ? "Left" : "right");
    }
  }

  ////////////
  // Fields //
  ////////////
  private App_Input _input;
  private float _zPosition;

  // User input
  private bool _isTriggerDown;
  private float _triggerPressure;
  private ThumbState _thumbState = ThumbState.Center;
  private bool _isPenActive = false;
  private bool _isNearFocus { get { return (_focusDistance <= _maxHoverDistance); } }

  // Focus info
  private Interactable _focus;
  private float _focusDistance;
  public PointerEmulation FocusPointerEmulation { get; private set; }
  private Ui_Control_Button _focusButton;
  private Ui_Control_Button _downButton;

  // Copies of values (for efficiency)
  private bool _isLeft;
  private Material _geometryMaterial;
  private static float _maxInteractDistance;
  private static float _TriggerDownPressure;
  private static float _ThumbDownPressure;
  private static float _maxHoverDistance;
  private static float _distanceTouch;
  private static float _distanceFullPressure;


  ////////////////////
  // Initialization //
  ////////////////////
  private void Awake()
  {
    // Copies of values init (for efficiency)
    _isLeft = transform.parent.GetComponent<Vr_Hand>().IsLeft;
    _geometryMaterial = _geometry.material;
    _maxInteractDistance = App_Details.Instance.MAX_INTERACT_DISTANCE;
    _TriggerDownPressure = App_Details.Instance.TRIGGER_DOWN_PRESSURE;
    _ThumbDownPressure = App_Details.Instance.THUMB_DOWN_PRESSURE;
    _maxHoverDistance = App_Details.Instance.CONTROLLER_DISTANCE_NEAR_SCREEN;
    _distanceTouch = App_Details.Instance.CONTROLLER_DISTANCE_TOUCH;
    _distanceFullPressure = App_Details.Instance.CONTROLLER_DISTANCE_FULL_PRESSURE;

    _controllerIndex = (uint)(_isLeft ? 0 : 1);
    _instances[_controllerIndex] = this;

    // Initial mappings
    Mappings[0] = new ControllerMapping(0);
    Mappings[1] = new ControllerMapping(1);
  }
  private void Start()
  {
    // Fields init
    _zPosition = _geometry.transform.localPosition.z;

    // User input init
    _input = new App_Input();
    _input.Enable();
    if (_isLeft)
    {
      _mainController = this;
      _input.VrLeftHandActions.Grip.performed += OnGripButtonStart;
      _input.VrLeftHandActions.Grip.canceled += OnGripButtonEnd;
      _input.VrLeftHandActions.ButtonTop.performed += OnTopButtonStart;
      _input.VrLeftHandActions.ButtonTop.canceled += OnTopButtonEnd;
      _input.VrLeftHandActions.ButtonBottom.performed += OnBottomButtonStart;
      _input.VrLeftHandActions.ButtonBottom.canceled += OnBottomButtonEnd;
      _input.VrLeftHandActions.ThumbDown.performed += OnThumbButtonStart;
      _input.VrLeftHandActions.ThumbDown.canceled += OnThumbButtonEnd;
    }
    else
    {
      _input.VrRightHandActions.Grip.performed += OnGripButtonStart;
      _input.VrRightHandActions.Grip.canceled += OnGripButtonEnd;
      _input.VrRightHandActions.ButtonTop.performed += OnTopButtonStart;
      _input.VrRightHandActions.ButtonTop.canceled += OnTopButtonEnd;
      _input.VrRightHandActions.ButtonBottom.performed += OnBottomButtonStart;
      _input.VrRightHandActions.ButtonBottom.canceled += OnBottomButtonEnd;
      _input.VrRightHandActions.ThumbDown.performed += OnThumbButtonStart;
      _input.VrRightHandActions.ThumbDown.canceled += OnThumbButtonEnd;
    }

    // Grip adjust init
    GetComponent<TransformSerializer>().SerializedTransform =
      PlayerPrefs.GetString(controllerTransformPrefsKey);
  }

  //////////////////////////
  // Focus and draw logic //
  //////////////////////////
  private void Update()
  {
    // Input
    HandleTrigger();
    HandleThumbPressure();
    HandleEmulatedButtons();

    // Logic
    HandleFocus();
    HandleNearControl();
  }
  private void HandleFocus()
  {
    var hitInfo = new RaycastHit();
    if (Physics.Raycast(transform.position, transform.forward, out hitInfo) &&
        hitInfo.distance < _maxInteractDistance)
    {
      _focusDistance = hitInfo.distance;
      _focus = hitInfo.transform.GetComponent<Interactable>();
      var focusParent = _focus?.transform?.parent;

      // Screen focus
      var newFocusPointerEmulation = focusParent?.GetComponent<PointerEmulation>();
      if (newFocusPointerEmulation != FocusPointerEmulation)
      {
        if (FocusPointerEmulation) { FocusPointerEmulation.OnLostFocus(); }
        FocusPointerEmulation = newFocusPointerEmulation;
        // Send focus position (if screen is in focus)
        if (FocusPointerEmulation) { FocusPointerEmulation.Position = hitInfo.textureCoord; }
      }

      // Button focus
      var newFocusButton = focusParent?.GetComponent<Ui_Control_Button>();
      if (newFocusButton != _focusButton)
      {
        if (_focusButton) { _focusButton.State = Ui_Control_Button.ButtonState.Idle; }
        _focusButton = newFocusButton;
        if (_focusButton)
        {
          if (!_downButton)
          {
            _focusButton.State = Ui_Control_Button.ButtonState.Hovered;
          }
          else if (_focusButton == _downButton)
          {
            _focusButton.State = Ui_Control_Button.ButtonState.Down;
          }
        }
      }
    }
    else
    {
      _focusDistance = _maxInteractDistance;
      _focus = null;
      FocusPointerEmulation = null;
      if (_focusButton) { _focusButton.State = Ui_Control_Button.ButtonState.Idle; }
      _focusButton = null;
    }

    _rayVisual.SetPosition(1, new Vector3(0, 0, 1) * _focusDistance);
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
//JDEV Debug.Log("Clearing pen state");
        _isPenActive = false;
        FocusPointerEmulation?.ClearPenState();
        _geometryMaterial.color = Color.white;
        _rayVisual.gameObject.SetActive(true);
      }
      return;
    }

    _rayVisual.gameObject.SetActive(false);

    // Calc pen pressure
    var penPressure =
      (_focusDistance > _distanceTouch) ? 0.0f :
      (_focusDistance < _distanceFullPressure) ? 1.0f :
      1.0f - (_focusDistance - _distanceFullPressure) / (_distanceTouch - _distanceFullPressure);

    // Calc pen rotation
    var rotation =
      (uint)((int)(transform.eulerAngles.z - FocusPointerEmulation.transform.eulerAngles.z + 360)
      % 360);

    // Calc pen pressure trigger adjust
    var triggerAdjust = 1.0f;
    var geometryOpacity = 1.0f;
    if      (_triggerPressure >  0.9f) { }
    else if (_triggerPressure <= 0.0f) { triggerAdjust = 0.0f; geometryOpacity = 1.00f; }
    else if (_triggerPressure >  0.3f) { triggerAdjust = 0.6f; geometryOpacity = 0.60f; }
    else                               { triggerAdjust = 0.3f; geometryOpacity = 0.20f; }

    // Calc tilt
    var forward = FocusPointerEmulation.transform.InverseTransformDirection(transform.forward);
    var tilt = new Vector2(
      Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg * -1.0f,
      Mathf.Atan2(forward.y, forward.z) * Mathf.Rad2Deg);

    // Visualize trigger adjust on geometry
    _geometryMaterial.color = new Color(1.0f, 1.0f, 1.0f, geometryOpacity);

    // Update pen
#if UNITY_EDITOR
    if (!App_Details.Instance.UseEmulatedControls)
#endif
    {
//JDEV Debug.Log("Setting pen state: " + (penPressure * triggerAdjust) + " :: " + tilt.x + "," + tilt.y + " isFlipped=" + Controller.IsFlipped);
      FocusPointerEmulation.SetPenState(
        penPressure * triggerAdjust, rotation, tilt, Controller.IsFlipped);
      _isPenActive = true;
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
    if (App_Details.Instance.UseEmulatedControls)
    {
      _triggerPressure = _trigger;
    }
#endif

    // Get value (Binary)
    var hasTriggerDownChanged = false;
    if (_triggerPressure >= _TriggerDownPressure && !_isTriggerDown)
    {
      _isTriggerDown = true;
      hasTriggerDownChanged = true;
    }
    else if (_triggerPressure < _TriggerDownPressure && _isTriggerDown)
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
        // Button
        if (_focusButton)
        {
          _downButton = _focusButton;
          _downButton.State = Ui_Control_Button.ButtonState.Down;
        }
        // Screen
        else if (FocusPointerEmulation)
        {
          FocusPointerEmulation.IsEmulating = true;
          if (!_isNearFocus) { FocusPointerEmulation.MouseLeftButton = true; }
        }
      }
      // Is up
      else
      {
        // Button
        if (_downButton && _downButton == _focusButton) { _downButton.DoClick(); }
        _downButton = null;
        if (_focusButton) { _focusButton.State = Ui_Control_Button.ButtonState.Hovered; }
        // Screen
        else if (FocusPointerEmulation)
        {
          if (!_isNearFocus) { FocusPointerEmulation.MouseLeftButton = false; }
        }
      }
      // Trigger action for non-main controller
      if (_controllerIndex > 0)
      {
        Mappings[_controllerIndex].TriggerButtonAction.Run(this, _isTriggerDown);
      }
    }
  }
  private void HandleThumbPressure()
  {
    // Early out
    if (this != _mainController) { return; }
    if (_isHolding) { return; }

    // Get value
    var pressure =
      _isLeft ?
      _input.VrLeftHandActions.ThumbPressure.ReadValue<Vector2>() :
      _input.VrRightHandActions.ThumbPressure.ReadValue<Vector2>();
#if UNITY_EDITOR
    if (App_Details.Instance.UseEmulatedControls)
    {
      pressure.x = _thumbLR;
      pressure.x = _thumbTB;
    }
#endif
    // Analogue to binary
    ThumbState newThumbState;
    newThumbState =
      (pressure.x > +_ThumbDownPressure && Mathf.Abs(pressure.x) > Mathf.Abs(pressure.y)) ? ThumbState.Right :
      (pressure.x < -_ThumbDownPressure && Mathf.Abs(pressure.x) > Mathf.Abs(pressure.y)) ? ThumbState.Left :
      (pressure.y > +_ThumbDownPressure) ? ThumbState.Up :
      (pressure.y < -_ThumbDownPressure) ? ThumbState.Down :
      ThumbState.Center;

    // React to value
    if (newThumbState != _thumbState)
    {
      if (_thumbState != ThumbState.Center)
      {
        Mappings[_controllerIndex].ThumbDirectionActions[(uint)_thumbState].Run(this, false);
      }
      _thumbState = newThumbState;
      if (_thumbState != ThumbState.Center)
      {
        Mappings[_controllerIndex].ThumbDirectionActions[(uint)_thumbState].Run(this, true);
      }
    }
  }
#if UNITY_EDITOR
  private void HandleEmulatedButtons()
  {
    if (App_Details.Instance.UseEmulatedControls)
    {
      var dummy = new CallbackContext();
      if (_topButton && !_prevTopButton) { OnTopButtonStart(dummy); }
      if (!_topButton && _prevTopButton) { OnTopButtonEnd(dummy); }
      if (_bottomButton && !_prevBottomButton) { OnBottomButtonStart(dummy); }
      if (!_bottomButton && _prevBottomButton) { OnBottomButtonEnd(dummy); }
      if (_gripButton && !_prevGripButton) { OnGripButtonStart(dummy); }
      if (!_gripButton && _prevGripButton) { OnGripButtonEnd(dummy); }
      if (_thumbButton && !_prevThumbButton) { OnThumbButtonStart(dummy); }
      if (!_thumbButton && _prevThumbButton) { OnThumbButtonEnd(dummy); }
      _prevTopButton = _topButton;
      _prevBottomButton = _bottomButton;
      _prevGripButton = _gripButton;
      _prevThumbButton = _thumbButton;
    }
  }
#endif
  private void OnGripButtonStart(CallbackContext obj)
  {
    Mappings[_controllerIndex].GripButtonAction.Run(this, true);
  }
  private void OnGripButtonEnd(CallbackContext obj)
  {
    Mappings[_controllerIndex].GripButtonAction.Run(this, false);
  }
  private void OnTopButtonStart(CallbackContext obj)
  {
    Mappings[_controllerIndex].TopButtonAction.Run(this, true);
  }
  private void OnTopButtonEnd(CallbackContext obj)
  {
    Mappings[_controllerIndex].TopButtonAction.Run(this, false);
  }
  private void OnBottomButtonStart(CallbackContext obj)
  {
    Mappings[_controllerIndex].BottomButtonAction.Run(this, true);
  }
  private void OnBottomButtonEnd(CallbackContext obj)
  {
    Mappings[_controllerIndex].BottomButtonAction.Run(this, false);
  }
  private void OnThumbButtonStart(CallbackContext obj)
  {
    Mappings[_controllerIndex].ThumbButtonAction.Run(this, true);
  }
  private void OnThumbButtonEnd(CallbackContext obj)
  {
    Mappings[_controllerIndex].ThumbButtonAction.Run(this, false);
  }
}
