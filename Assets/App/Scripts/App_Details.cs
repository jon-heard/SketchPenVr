using UnityEngine;

public class App_Details : Common.SingletonComponent<App_Details>
{
  public Common.CommonDetails MyCommonDetails;

  public enum InputType { Mouse, Vr, VrSimulation }
  public InputType MyInputType;

  public float MAX_INTERACT_DISTANCE = 100.0f; // How near controller needs to be to interact with something at all
  public float TRIGGER_DOWN_PRESSURE = 0.5f; // How much pressure user puts on trigger before we consider it pressed
  public float THUMB_DOWN_PRESSURE = 0.75f; // How much directional pressure user puts on thumb before we consider it pressed
  public float CONTROLLER_DISTANCE_NEAR_SCREEN = 0.232f; // How near controller needs to be to hover over screen
  public float CONTROLLER_DISTANCE_TOUCH = 0.129f; // How near controller needs to be to touch screen
  public float CONTROLLER_DISTANCE_FULL_PRESSURE = 0.107f; // How near controller needs to be to put full pressure on screen
  public float PANEL_X_MARGIN = 0.16f; // How big of a margin to put at edge of panels
  public float DISTANCE_BETWEEN_PANELS = 0.0085f; // How much space to put between panels
  public float TIMESPAN_BEFORE_SETTING_SCREEN_HEIGHT = 1.0f; // How long to wait before setting screen to user's eye level
  public float MIN_SCALE_SIZE = 0.25f; // How small to allow the screen to be sized
  public float CONTROLLER_EMULATED_SEPARATION = 0.2f; // How opaque the controller visuals are when not highlighted

  // Key constants
  public const string CFG__IS_LEFT_HANDED = "setting:isLeftHanded";
  public const string CFG__CONTROLLER_TRANSFORM = "setting:controller%1Transform";
  public const string CFG__MAPPINGS = "setting:mappings";
  public const string CFG__BACKDROP = "setting:backdrop";
  public const string LOCK__DIRECT = "lock:direct";
  public const string LOCK__SKETCH_CONTROLLER = "lock:controller";
  public const string LOCK__SKETCH_IS_LOCKED = "lock:sketchLocked";
  public const string LOCK__ALL_UI = "lock:allUi";
  public const string LOCK__OTHER_IS_LOCKED_DOWN = "lock:otherIsLockedDown";

  public MappingCollection MyControllerMappings;
  public void SaveControllerMappings()
  {
    PlayerPrefs.SetString(App_Details.CFG__MAPPINGS, JsonUtility.ToJson(MyControllerMappings));
    foreach (var mapping in MyControllerMappings.Mappings)
    {
      mapping.OnMappingChanged.Invoke(mapping);
    }
  }

  public bool IsLeftHanded
  {
    get { return Controller.IsLeftHanded; }
    set
    {
      if (value == Controller.IsLeftHanded) { return; }
      Controller.IsLeftHanded = value;
      PlayerPrefs.SetInt(App_Details.CFG__IS_LEFT_HANDED, IsLeftHanded ? 1 : 0);
    }
  }

  public string Backdrop
  {
    get { return _backdrop; }
    set
    {
      if (value == _backdrop) { return; }
      _backdrop = value;
      App_Functions.Instance.Backdrop.SetTexture(
        "_Tex", Resources.Load<Cubemap>("Backdrops/" + _backdrop));
      PlayerPrefs.SetString(App_Details.CFG__BACKDROP, _backdrop);
    }
  }
  private string _backdrop;

  private void Awake()
  {
    // Handedness
    IsLeftHanded = (PlayerPrefs.GetInt(App_Details.CFG__IS_LEFT_HANDED, 0) != 0);

    // Backdrop
    Backdrop = PlayerPrefs.GetString(App_Details.CFG__BACKDROP, "artStudio");

    // Mappings
    var mappingsString = PlayerPrefs.GetString(App_Details.CFG__MAPPINGS, null);
    if (mappingsString != null)
    {
      try
      {
        MyControllerMappings = JsonUtility.FromJson<MappingCollection>(mappingsString);
        if (MyControllerMappings == null) { mappingsString = null; }
      }
      catch
      {
        mappingsString = null;
      }
    }
    if (mappingsString == null)
    {
      MyControllerMappings = new MappingCollection();
      MyControllerMappings.SetupDefault();
    }
  }
}
