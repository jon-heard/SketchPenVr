using Common.Vr.Ui.Controls;
using System.Collections;
using UnityEngine;

public class App_Functions : Common.SingletonComponent<App_Functions>
{
  [SerializeField] private Camera _camera;
  public Screen MyScreen;
  [SerializeField] private Controller _leftController;
  [SerializeField] private Controller _rightController;
  public Material Backdrop;
  public Renderer ScreenRenderer;

  // Lock/unlock entire ui
  public void SetFullUiLock(bool isLocked)
  {
    foreach (var button in Button.Instances)
    {
      button.Locker.SetLock(App_Details.LOCK__ALL_UI, isLocked);
    }
  }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
  private InputHandler_Mouse _mouseHandler;
#endif

  private void Awake()
  {
    Backdrop = new Material(Backdrop);
    RenderSettings.skybox = Backdrop;
  }

  private void Start()
  {
    // Logic for the literal mouse clicking virtual buttons
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    var input = new App_Input();
    input.Enable();
    if (App_Details.Instance.MyInputType == App_Details.InputType.Mouse)
    {
      _mouseHandler = new InputHandler_Mouse();
      StartCoroutine(_mouseHandler.Update(
        _camera, input.Mouse.Position, input.Mouse.LeftButton));
    }
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
