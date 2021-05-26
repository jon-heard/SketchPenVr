
using System.Collections;
using UnityEngine;

public class App_Details : Singleton<App_Details>
{
#if UNITY_EDITOR
  public bool UseEmulatedControls = false;
#endif

  public float MAX_INTERACT_DISTANCE = 100.0f; // How near controller needs to be to interact with something at all
  public float TRIGGER_DOWN_PRESSURE = 0.5f; // How much pressure user puts on trigger before we consider it pressed
  public float THUMB_DOWN_PRESSURE = 0.75f; // How much directional pressure user puts on thumb before we consider it pressed
  public float CONTROLLER_DISTANCE_NEAR_SCREEN = 0.232f; // How near controller needs to be to hover over screen
  public float CONTROLLER_DISTANCE_TOUCH = 0.129f; // How near controller needs to be to touch screen
  public float CONTROLLER_DISTANCE_FULL_PRESSURE = 0.107f; // How near controller needs to be to put full pressure on screen
  public float PANEL_X_MARGIN = 0.16f; // How big of a margin to put at edge of panels
  public float DISTANCE_BETWEEN_PANELS = 0.0085f; // How much space to put between panels
  public float TIMESPAN_BEFORE_SETTING_SCREEN_HEIGHT = 1.0f; // How long to wait before setting screen to user's eye level

  // Key constants
  public const string CFG__IS_LEFT_HANDED = "setting:isLeftHanded";
  public const string CFG__CONTROLLER_TRANSFORM = "setting:controller%1Transform";
  public const string CFG__MAPPINGS = "setting:mappings";
  public const string LOCK__DIRECT = "lock:direct";
  public const string LOCK__SKETCH_CONTROLLER = "lock:controller";
  public const string LOCK__SKETCH_IS_LOCKED = "lock:sketchLocked";
  public const string LOCK__ALL_UI = "lock:allUi";
  public const string LOCK__OTHER_IS_LOCKED_DOWN = "lock:otherIsLockedDown";

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

  private void Start()
  {
    // Handedness
    IsLeftHanded = (PlayerPrefs.GetInt(App_Details.CFG__IS_LEFT_HANDED, 0) != 0);

    // Mappings
    //try
    //{
    //  var mappingsString = PlayerPrefs.GetString(App_Details.CFG__MAPPINGS, null);
    //  var mappings = JsonUtility.FromJson<MappingCollection>(mappingsString);
    //  Controller.Mappings = mappings.Mappings ?? Controller.Mappings;
    //}
    //catch
    //{
    //  Debug.LogError("Unable to load controller mappings.  Using defaults.");
    //}
  }
  private void OnDestroy()
  {
    var mappings = new MappingCollection();
    mappings.Mappings = Controller.Mappings;
    PlayerPrefs.SetString(App_Details.CFG__MAPPINGS, JsonUtility.ToJson(mappings));
  }

  private struct MappingCollection
  {
    public ControllerMapping[] Mappings;
  }
}
