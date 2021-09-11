using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

public class App_Details : Common.SingletonComponent<App_Details>
{
  public Common.CommonDetails MyCommonDetails;

  public enum InputType { Mouse, Vr, VrSimulation }
  public InputType MyInputType;

  public float MAX_INTERACT_DISTANCE = 100.0f; // How near controller needs to be to interact with something at all
  public float TRIGGER_ACTIVATE_PRESSURE = 0.5f; // How much pressure user puts on trigger before we consider it pressed
  public float THUMBSTICK_ACTIVATE_PRESSURE = 0.75f; // How much directional pressure user puts on thumbstick before we consider it pressed
  public float CONTROLLER_DISTANCE_NEAR_SCREEN = 0.232f; // How near controller needs to be to hover over screen
  public float CONTROLLER_DISTANCE_TIP_POINT = 0.129f; // How near controller needs to be to touch screen
  public float CONTROLLER_DISTANCE_TIP_BASE = 0.107f; // How near controller needs to be to put full pressure on screen
  public float CONTROLLER_DISTANCE_ERASER_BASE = 0.008f; // How near controller needs to be to put full pressure on screen
  public float PANEL_X_MARGIN = 0.16f; // How big of a margin to put at edge of panels
  public float DISTANCE_BETWEEN_PANELS = 0.0085f; // How much space to put between panels
  public float TIMESPAN_BEFORE_SETTING_SCREEN_HEIGHT = 1.0f; // How long to wait before setting screen to user's eye level
  public float MIN_SCALE_SIZE = 0.25f; // How small to allow the screen to be sized
  public float CONTROLLER_EMULATED_SEPARATION = 0.2f; // How opaque the controller visuals are when not highlighted
  public float ALIGN_DATA_DISTANCE = 0.1f; // How close together to take the data points for calculating alignment to a real-life plane
  public float TIMESPAN_POINTER_CHANGEOVER = 0.08f; // How long to delay after changing the pointer from pen to something else
  public float HAPTICS_STRENGTH_HARD = 1.0f; // Rumble amplitude when set to "hard"
  public float HAPTICS_STRENGTH_MEDIUM = 0.35f; // Rumble amplitude when set to "medium"
  public float HAPTICS_STRENGTH_LIGHT = 0.15f; // Rumble amplitude when set to "light"
  public Vector3 KEYBOARD_POSITION = new Vector3(-0.36f, 0.35f, -0.02f);
  public Vector3 KEY_SELECT_KEYBOARD_POSITION = new Vector3(0.685f, 0.568075f, 0.0f);
  public float VOLUME_PEN_HIT = 10.0f;
  public float VOLUME_PEN_SCRAPE = 2.0f;
  public float VOLUME_PEN_RUMBLE = 3.0f;
  public uint PANEL_SIZE_PRESET_COUNT = 5;
  public uint[] PressureCurves;
  public float[] PressureLengths;
  public float[] VolumeLevels;
  public float[] PanelSizePresetDefaults;

  // Key constants
  public const string CFG__BACKGROUND = "setting:background";
  public const string CFG__MAPPINGS = "setting:mappings";
  public const string CFG__CONTROLLER_TRANSFORM = "setting:controller%1Transform";
  public const string CFG__PRESSURE_CURVE_INDEX = "setting:PressureCurveIndex";
  public const string CFG__PRESSURE_LENGTH_INDEX = "setting:PressureLengthIndex";
  public const string CFG__VOLUME_INDEX = "setting:VolumeIndex";
  public const string CFG__PEN_PHYSICS = "setting:penPhysics";
  public const string CFG__HAPTICS_STRENGTH = "setting:HapticsStrength";
  public const string CFG__IS_LEFT_HANDED = "setting:isLeftHanded";
  public const string CFG__PANEL_SIZE_PRESET = "setting:panelSizePreset";
  public const string LOCK__DIRECT = "lock:direct";
  public const string LOCK__SKETCH_CONTROLLER = "lock:controller";
  public const string LOCK__SKETCH_IS_LOCKED = "lock:sketchLocked";
  public const string LOCK__ALL_UI = "lock:allUi";
  public const string LOCK__OTHER_IS_LOCKED_DOWN = "lock:otherIsLockedDown";

  [NonSerialized] public MappingCollection MyControllerMappings = new MappingCollection();

  public void SaveControllerMappings()
  {
    PlayerPrefs.SetString(App_Details.CFG__MAPPINGS, JsonUtility.ToJson(MyControllerMappings));
    foreach (var mapping in MyControllerMappings.Mappings)
    {
      mapping.OnMappingChanged?.Invoke(mapping);
    }
  }

  public string Background
  {
    get { return _background; }
    set
    {
      if (value == _background) { return; }
      _background = value;
      App_Functions.Instance.Background.SetTexture(
        "_Tex", Resources.Load<Cubemap>("Backgrounds/" + _background));
      PlayerPrefs.SetString(App_Details.CFG__BACKGROUND, _background);
    }
  }
  private string _background;

  public enum PenPhysicsType { Full, Shaft, None }
  public PenPhysicsType PenPhysics
  {
    get { return (PenPhysicsType)_penPhysics; }
    set
    {
      if (value == (PenPhysicsType)_penPhysics) { return; }
      _penPhysics = (int)value;
      PlayerPrefs.SetInt(App_Details.CFG__PEN_PHYSICS, _penPhysics);
      Controller.Const_penPhysics = value;
    }
  }
  private int _penPhysics;

  public bool IsLeftHanded
  {
    get { return Controller.IsLeftHanded; }
    set
    {
      Controller.IsLeftHanded = value;
      PlayerPrefs.SetInt(App_Details.CFG__IS_LEFT_HANDED, value ? 1 : 0);
    }
  }

  public uint PressureCurveIndex
  {
    get { return _pressureCurveIndex; }
    set
    {
      _pressureCurveIndex = value;
      Controller.PrimaryController.Pen.PressureCurve = PressureCurves[_pressureCurveIndex];
      Controller.SecondaryController.Pen.PressureCurve = PressureCurves[_pressureCurveIndex];
      PlayerPrefs.SetInt(App_Details.CFG__PRESSURE_LENGTH_INDEX, (int)_pressureCurveIndex);
    }
  }
  private uint _pressureCurveIndex = Global.NullUint;

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
      _pressureLength = value;
      CONTROLLER_DISTANCE_TIP_BASE = CONTROLLER_DISTANCE_TIP_POINT - _pressureLength;
      Controller.SetPressureLength(_pressureLength);
    }
  }
  private float _pressureLength;

  public uint VolumeIndex
  {
    get { return _volumeIndex; }
    set
    {
      _volumeIndex = value;
      PlayerPrefs.SetInt(App_Details.CFG__VOLUME_INDEX, (int)_volumeIndex);
    }
  }
  private uint _volumeIndex = 2;

  public float Volume
  {
    get { return VolumeLevels[VolumeIndex]; }
  }


  public enum HapticsStrengthType { Hard, Medium, Light, None }
  public HapticsStrengthType HapticsStrength
  {
    get { return (HapticsStrengthType)_hapticsStrength; }
    set
    {
      if (value == (HapticsStrengthType)_hapticsStrength) { return; }
      _hapticsStrength = (uint)value;
      PlayerPrefs.SetInt(App_Details.CFG__HAPTICS_STRENGTH, (int)_hapticsStrength);
    }
  }
  private uint _hapticsStrength = 1;

  public float[] PanelSizePresets;
  public void StorePanelSizePresets()
  {
    for (var i = 0; i < PANEL_SIZE_PRESET_COUNT; i++)
    {
      PlayerPrefs.SetFloat(App_Details.CFG__PANEL_SIZE_PRESET + i, PanelSizePresets[i]);
    }
  }

  public void ResetSettings()
  {
    PlayerPrefs.DeleteAll();
    Controller.ResetGripAdjust();
    Awake();
    Start();
  }

  private void Awake()
  {
    // Background
    Background = PlayerPrefs.GetString(App_Details.CFG__BACKGROUND, "artStudio");

    // Mappings
    var mappingsString = PlayerPrefs.GetString(App_Details.CFG__MAPPINGS, null);
    if (mappingsString != null)
    {
      try
      {
        MyControllerMappings = JsonUtility.FromJson<MappingCollection>(mappingsString);
        if (MyControllerMappings == null)
        {
          mappingsString = null;
        }
        else if (MyControllerMappings.Mappings.Length != MappingCollection.MappingsCount)
        {
          var mappings = MyControllerMappings.Mappings;
          MyControllerMappings.Mappings = new ControllerMapping[MappingCollection.MappingsCount];
          for (var i = 0; i < MappingCollection.MappingsCount; i++)
          {
            if (mappings.Length > i)
            {
              MyControllerMappings.Mappings[i] = mappings[i];
            }
            else
            {
              MyControllerMappings.Mappings[i] = new ControllerMapping();
              MyControllerMappings.Mappings[i].SetupDefault(i);
            }
          }
        }
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
    MyControllerMappings.Mappings[3].OnMappingChanged +=
      App_Functions.Instance.MyButtonActionManager.OnLeftMappingChanged;
    MyControllerMappings.Mappings[4].OnMappingChanged +=
      App_Functions.Instance.MyButtonActionManager.OnRightMappingChanged;
  }

  private void Start()
  {
    // Pressure length
    PressureCurveIndex = (uint)PlayerPrefs.GetInt(App_Details.CFG__PRESSURE_CURVE_INDEX, 2);

    // Pressure length
    PressureLengthIndex = (uint)PlayerPrefs.GetInt(App_Details.CFG__PRESSURE_LENGTH_INDEX, 2);

    // Pressure length
    VolumeIndex = (uint)PlayerPrefs.GetInt(App_Details.CFG__VOLUME_INDEX, 2);

    // Haptics strength
    HapticsStrength = (HapticsStrengthType)PlayerPrefs.GetInt(App_Details.CFG__HAPTICS_STRENGTH, 1);

    // Handedness
    PenPhysics = (PenPhysicsType)PlayerPrefs.GetInt(App_Details.CFG__PEN_PHYSICS, 1);

    // Handedness
    IsLeftHanded = (PlayerPrefs.GetInt(App_Details.CFG__IS_LEFT_HANDED, 0) != 0);

    // Panel size presets
    PanelSizePresets = new float[App_Details.Instance.PANEL_SIZE_PRESET_COUNT];
    for (var i = 0; i < PANEL_SIZE_PRESET_COUNT; i++)
    {
      PanelSizePresets[i] =
        PlayerPrefs.GetFloat(App_Details.CFG__PANEL_SIZE_PRESET + i, PanelSizePresetDefaults[i]);
    }
  }
}
