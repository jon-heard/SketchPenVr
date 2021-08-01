using Common.Vr.Ui.Controls;
using System;
using UnityEngine;

namespace Common.Vr.Ui.Popups
{
  public class Alert : SingletonComponent<Alert>
  {
    public Transform Geometry;
    public TextMesh Label;
    public Button[] Buttons;

    public static void ShowOnButton(Button button, string message, Action onFinished = null)
    {
      Alert.Show(message, button, button.transform, button.Geometry.transform, onFinished);
    }

    public static void ShowOnButtonParent(Button button, string message, Action onFinished = null)
    {
      var buttonParent = button.transform.parent;
      var parentGeometry = buttonParent.Find("Geometry").transform;
      Alert.Show(message, button, buttonParent, parentGeometry, onFinished);
    }

    public static void Show(
      string message, Button source, Transform parent, Transform geometry, Action onFinished)
    {
      Instance.ShowMe(message, source, parent, geometry, onFinished);
    }

    public void ShowMe(
      string message, Button source, Transform parent, Transform geometry, Action onFinished)
    {
      // Lock full Ui (except for this popup && src button)
      source.State = Button.ButtonState.LockedDown;
      App_Functions.Instance.SetFullUiLock(true);
      foreach (var button in Buttons)
      {
        button.Locker.SetLock(App_Details.LOCK__ALL_UI, false);
      }

      _source = source;
      transform.parent = parent;
      Label.text = message;
      var s = Geometry.localScale;
      s.x = Mathf.Max(0.19f, Label.GetTextWidth() + App_Details.Instance.PANEL_X_MARGIN);
      Geometry.localScale = s;
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
      _onFinished?.Invoke();
    }

    private Action _onFinished;
    private Button _source;

    private void Awake()
    {
      Alert.InitializeSingletonComponent();
    }
    private void Start()
    {
      gameObject.SetActive(false);
    }
  }
}
