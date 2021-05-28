using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App_Functions : Singleton<App_Functions>
{
  [SerializeField] private Camera _camera;
  public PointerEmulation Screen;
  [SerializeField] private Controller _leftController;
  [SerializeField] private Controller _rightController;
  public Material Backdrop;

  // Lock/unlock entire ui
  public void SetFullUiLock(bool isLocked)
  {
    foreach (var button in Ui_Control_Button.Instances)
    {
      button.Locker.SetLock(App_Details.LOCK__ALL_UI, isLocked);
    }
  }

  private void Awake()
  {
    Backdrop = new Material(Backdrop);
    RenderSettings.skybox = Backdrop;
  }
  private void Start()
  {
    // Logic for the literal mouse clicking virtual buttons
#if UNITY_EDITOR
    _input = new App_Input();
    _input.Mouse.LeftButton.performed += OnLeftMouseDown;
    _input.Mouse.LeftButton.canceled += OnLeftMouseUp;
#else
    var _input = new App_Input();
#endif
    _input.Enable();

    // Logic for repositioning screen to match height of user's head
#if UNITY_EDITOR
    if (App_Details.Instance.UseEmulatedControls)
    {
      StartCoroutine(Controller.SeparateControllers());
    }
    else
#endif
    {
      StartCoroutine(SetupScreenHeight(_input));
    }

#if !UNITY_EDITOR
    OsHook_Window.Minimize();
#endif
  }

  private IEnumerator SetupScreenHeight(App_Input input)
  {
    yield return new WaitForSeconds(App_Details.Instance.TIMESPAN_BEFORE_SETTING_SCREEN_HEIGHT);
    var initialHeadPosition = input.VrHeadTracking.Position.ReadValue<Vector3>();
    if (initialHeadPosition != Vector3.zero)
    {
      var t = App_Functions.Instance.Screen.transform.position;
      t.y = initialHeadPosition.y;
      App_Functions.Instance.Screen.transform.position = t;
    }
  }

// Logic for the literal mouse clicking virtual buttons
#if UNITY_EDITOR
  private App_Input _input;
  private bool _isMouseDown = false;
  private void OnLeftMouseDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    if (!_isMouseDown)
    {
      _isMouseDown = true;
      var ray = _camera.ScreenPointToRay(_input.Mouse.Position.ReadValue<Vector2>());
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit))
      {
        hit.collider.GetComponent<Ui_Control_ButtonGeometry>()?.OnMouseDown();
      }
    }
  }
  private void OnLeftMouseUp(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    _isMouseDown = false;
  }
#endif

#if !UNITY_EDITOR
  void OnApplicationFocus(bool hasFocus)
  {
    if (hasFocus)
    {
      OsHook_Window.Minimize();
    }
  }
#endif
}
