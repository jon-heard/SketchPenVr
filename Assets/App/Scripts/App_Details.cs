using Common;
using System.Collections.Generic;
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
  public float CONTROLLER_DISTANCE_TIP_POINT = 0.129f; // How near controller needs to be to touch screen
  public float CONTROLLER_DISTANCE_TIP_BASE = 0.107f; // How near controller needs to be to put full pressure on screen
  public float PANEL_X_MARGIN = 0.16f; // How big of a margin to put at edge of panels
  public float DISTANCE_BETWEEN_PANELS = 0.0085f; // How much space to put between panels
  public float TIMESPAN_BEFORE_SETTING_SCREEN_HEIGHT = 1.0f; // How long to wait before setting screen to user's eye level
  public float MIN_SCALE_SIZE = 0.25f; // How small to allow the screen to be sized
  public float CONTROLLER_EMULATED_SEPARATION = 0.2f; // How opaque the controller visuals are when not highlighted
  public float ALIGN_DATA_DISTANCE = 0.1f; // How close together to take the data points for calculating alignment to a real-life plane
  public float TIMESPAN_POINTER_CHANGEOVER = 0.04f; // How long to delay after changing the pointer from pen to something else
  public List<string> PressureLengthTitles;
  public float[] PressureLengths;

  // Key constants
  public const string CFG__IS_LEFT_HANDED = "setting:isLeftHanded";
  public const string CFG__CONTROLLER_TRANSFORM = "setting:controller%1Transform";
  public const string CFG__MAPPINGS = "setting:mappings";
  public const string CFG__BACKDROP = "setting:backdrop";
  public const string CFG__PRESSURE_LENGTH_INDEX = "setting:PressureLengthIndex";
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

  public uint PressureLengthIndex
  {
    get { return _pressureLengthIndex; }
    set
    {
      _pressureLengthIndex = value;
      PressureLength = PressureLengths[_pressureLengthIndex];
      PlayerPrefs.SetInt(App_Details.CFG__PRESSURE_LENGTH_INDEX, (int)_pressureLengthIndex);
    }
  }
  private uint _pressureLengthIndex = Global.NullUint;

  public float PressureLength
  {
    get { return _pressureLength; }
    set
    {
      if (value == _pressureLength) { return; }
      _pressureLength = value;
      CONTROLLER_DISTANCE_TIP_BASE = CONTROLLER_DISTANCE_TIP_POINT - _pressureLength;
      Controller.SetPressureLength(_pressureLength);
      Mesh_Pencil.SetAllTipLengths(_pressureLength);
    }
  }
  private float _pressureLength;

  private void Awake()
  {

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

  private void Start()
  {
    // Handedness
    IsLeftHanded = (PlayerPrefs.GetInt(App_Details.CFG__IS_LEFT_HANDED, 0) != 0);

    // Pressure length
    PressureLengthIndex = (uint)PlayerPrefs.GetInt(App_Details.CFG__PRESSURE_LENGTH_INDEX, 1);
  }
}
