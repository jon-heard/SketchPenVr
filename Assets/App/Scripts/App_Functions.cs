using Common.Vr.Ui.Controls;
using System.Collections;
using UnityEngine;

public class App_Functions : Common.SingletonComponent<App_Functions>
{
  [SerializeField] private Camera _camera;
  public Screen MyScreen;
  [SerializeField] private Controller _leftController;
  [SerializeField] private Controller _rightController;
  public Material Background;
  public Renderer ScreenRenderer;
  public InputManager MyInputManager;

  // Lock/unlock entire ui
  public void SetFullUiLock(bool isLocked)
  {
    foreach (var button in Button.Instances)
    {
      button.Locker.SetLock(App_Details.LOCK__ALL_UI, isLocked);
    }
  }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
  private UiInputHandler_Mouse _mouseHandler;
#endif

  private void Awake()
  {
    Background = new Material(Background);
    RenderSettings.skybox = Background;
  }

  private void Start()
  {
    // Logic for the literal mouse clicking virtual buttons
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    var input = new App_Input();
    input.Enable();
    if (App_Details.Instance.MyInputType == App_Details.InputType.Mouse)
    {
      _mouseHandler = new UiInputHandler_Mouse();
      StartCoroutine(_mouseHandler.Update(
        _camera, input.Mouse.Position, input.Mouse.LeftButton));
    }
    // Left
    MyInputManager.AddBooleanInput("left_grip", input.VrLeftHandActions.Grip);
    MyInputManager.AddBooleanInput("left_high", input.VrLeftHandActions.HighButton);
    MyInputManager.AddBooleanInput("left_low", input.VrLeftHandActions.LowButton);
    MyInputManager.AddBooleanInput("left_thumbstick_down", input.VrLeftHandActions.ThumbstickDown);
    MyInputManager.AddNumericalInput(
      "left_trigger", input.VrLeftHandActions.TriggerPressure,
      App_Details.Instance.TRIGGER_ACTIVATE_PRESSURE, InputManager.NumericalActionType.scalar1);
    MyInputManager.AddNumericalInput(
      "left_thumbstick_direction", input.VrLeftHandActions.ThumbstickDirection,
      App_Details.Instance.THUMBSTICK_ACTIVATE_PRESSURE, InputManager.NumericalActionType.scalar2);
    // Right
    MyInputManager.AddBooleanInput("right_grip", input.VrRightHandActions.Grip);
    MyInputManager.AddBooleanInput("right_high", input.VrRightHandActions.HighButton);
    MyInputManager.AddBooleanInput("right_low", input.VrRightHandActions.LowButton);
    MyInputManager.AddBooleanInput("right_thumbstick_down", input.VrRightHandActions.ThumbstickDown);
    MyInputManager.AddNumericalInput(
      "right_trigger", input.VrRightHandActions.TriggerPressure,
      App_Details.Instance.TRIGGER_ACTIVATE_PRESSURE, InputManager.NumericalActionType.scalar1);
    MyInputManager.AddNumericalInput(
      "right_thumbstick_direction", input.VrRightHandActions.ThumbstickDirection,
      App_Details.Instance.THUMBSTICK_ACTIVATE_PRESSURE, InputManager.NumericalActionType.scalar2);
#endif

#if UNITY_EDITOR
    if (App_Details.Instance.MyInputType == App_Details.InputType.VrSimulation)
    {
      StartCoroutine(Controller.SeparateControllers());
    }
    else
#endif
    {
      // Logic for repositioning screen to match height of user's head
      StartCoroutine(SetupScreenHeight(input));
    }

#if !UNITY_EDITOR
    // Logic to minimize this window
    OsHook_Window.Minimize();
#endif
  }

  private IEnumerator SetupScreenHeight(App_Input input)
  {
    yield return new WaitForSeconds(App_Details.Instance.TIMESPAN_BEFORE_SETTING_SCREEN_HEIGHT);
    var initialHeadPosition = input.VrHeadTracking.Position.ReadValue<Vector3>();
    if (initialHeadPosition != Vector3.zero)
    {
      var t = App_Functions.Instance.MyScreen.transform.position;
      t.y = initialHeadPosition.y;
      App_Functions.Instance.MyScreen.transform.position = t;
    }
  }

#if !UNITY_EDITOR
  // Logic to keep window minimized
  void OnApplicationFocus(bool hasFocus)
  {
    if (hasFocus)
    {
      OsHook_Window.Minimize();
    }
  }
#endif
}
