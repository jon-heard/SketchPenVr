using System;
using Common.Vr.Ui.Controls;
using UnityEngine;

namespace Common.Vr.Ui.Popups
{
  public class Confirm : SingletonComponent<Confirm>
  {
    [SerializeField] private Transform _geometry;
    [SerializeField] private TextMesh _label;
    [SerializeField] private Button[] _buttons;

    public static void ShowOnButton(
      Button button, string message, Action<bool> onFinished)
    {
      Show(message, button, button.transform, button.Geometry.transform, onFinished);
    }

    public static void ShowOnButtonParent(
      Button button, string message, Action<bool> onFinished)
    {
      var buttonParent = button.transform.parent;
      var parentGeometry = buttonParent.Find("Geometry").transform;
      Show(message, button, buttonParent, parentGeometry, onFinished);
    }

    public static void Show(
      string message, Button source, Transform parent, Transform geometry,
      Action<bool> onFinished)
    {
      Instance.ShowMe(message, source, parent, geometry, onFinished);
    }

    public void ShowMe(
      string message, Button source, Transform parent, Transform geometry,
      Action<bool> onFinished)
    {
      // Lock full Ui (except for this popup && src button)
      source.State = Button.ButtonState.LockedDown;
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
      _source.State = Button.ButtonState.NotLockedDown;
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
    private Button _source;

    private void Awake()
    {
      Confirm.InitializeSingletonComponent();
    }
    private void Start()
    {
      gameObject.SetActive(false);
    }
  }
}
