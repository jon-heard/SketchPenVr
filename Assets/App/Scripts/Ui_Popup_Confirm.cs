using System;
using UnityEngine;

public class Ui_Popup_Confirm : Singleton<Ui_Popup_Confirm>
{
  [SerializeField] private Transform _geometry;
  [SerializeField] private TextMesh _label;
  [SerializeField] private Ui_Control_Button[] _buttons;

  public static void ShowOnButton(Ui_Control_Button button, string message, Action<bool> onFinished)
  {
    Show(message, button, button.transform, button.Geometry.transform, onFinished);
  }

  public static void ShowOnButtonParent(Ui_Control_Button button, string message, Action<bool> onFinished)
  {
    var buttonParent = button.transform.parent;
    var parentGeometry = buttonParent.Find("Geometry").transform;
    Show(message, button, buttonParent, parentGeometry, onFinished);
  }

  public static void Show(
    string message, Ui_Control_Button source, Transform parent, Transform geometry,
    Action<bool> onFinished)
  {
    Instance.ShowMe(message, source, parent, geometry, onFinished);
  }

  public void ShowMe(
    string message, Ui_Control_Button source, Transform parent, Transform geometry,
    Action<bool> onFinished)
  {
    // Lock full Ui (except for this popup && src button)
    source.State = Ui_Control_Button.ButtonState.LockedDown;
    App_Functions.Instance.SetFullUiLock(true);
    foreach (var button in _buttons)
    {
      button.Locker.SetLock(App_Details.LOCK__ALL_UI, false);
    }

    _source = source;
    transform.parent = parent;
    _label.text = message;
    var s = _geometry.localScale;
    s.x = Mathf.Max(0.53f, _label.GetTextWidth() + App_Details.Instance.PANEL_X_MARGIN);
    _geometry.localScale = s;
    gameObject.SetActive(true);
    transform.localPosition =
      new Vector3(
        (geometry.localScale.x + s.x) * 0.5f + App_Details.Instance.DISTANCE_BETWEEN_PANELS,
        0.0f, 0.0f);
    transform.localEulerAngles = Vector3.zero;
    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    _onFinished = onFinished;
  }

  public void Hide()
  {
    App_Functions.Instance.SetFullUiLock(false);
    _source.State = Ui_Control_Button.ButtonState.NotLockedDown;
    gameObject.SetActive(false);
    transform.parent = null;
    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
  }

  public void OnOkButton()
  {
    Hide();
    _onFinished?.Invoke(true);
  }

  public void OnCancelButton()
  {
    Hide();
    _onFinished?.Invoke(false);
  }

  private Action<bool> _onFinished;
  private Ui_Control_Button _source;

  private void Awake()
  {
    Ui_Popup_Confirm.InitializeSingleton();
  }
  private void Start()
  {
    gameObject.SetActive(false);
  }
}
