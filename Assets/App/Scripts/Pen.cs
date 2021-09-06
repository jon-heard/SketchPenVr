using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
  private const uint PRIOR_DISTANCE_COUNT = 3;
  private const uint PRIOR_FOCUS_POSITION_COUNT = 3;
  private const float TOP_PEN_HIT_SPEED = 0.5f;
  private const float TOP_PEN_MOVE_SPEED = 0.01f;
  private const float InitialVertexPosition_Tip = 0.054462f;
  private const float InitialVertexPosition_Shaft1 = 0.053359f;
  private const float InitialVertexPosition_Shaft2 = 0.054870f;
  private const float InitialVertexPosition_Shaft3 = 0.106106f;
  private const float InitialVertexPosition_InnerShaft = 0.054105f;
  private const float Shaft2Offset = InitialVertexPosition_Shaft2 - InitialVertexPosition_Shaft1;

  [SerializeField] private MeshFilter _penTip;
  [SerializeField] private MeshFilter _penShaft;
  [SerializeField] private MeshFilter _penInnerShaft;
  [SerializeField] private Renderer[] _renderers;
  [SerializeField] private AudioSource Audio_Bump;
  [SerializeField] private AudioSource Audio_Scratch;
  [SerializeField] private AudioSource Audio_Rumble;

  [NonSerialized] public App_Details.PenPhysicsType PenPhysics;

  public float Pressure { get; private set; }

  public bool IsFlipped
  {
    get { return _isFlipped; }
    set
    {
      if (value == _isFlipped) { return; }
      _isFlipped = value;
      // Rotate 180 (on y)
      var t = transform.localEulerAngles;
      t.y = _isFlipped ? 180.0f : 0.0f;
      transform.localEulerAngles = t;
      // Adjust Z-position
      t = transform.localPosition;
      t.z =_currentZPosition = (_isFlipped ? 0.0f : _defaultZPosition);
      transform.localPosition = t;
    }
  }
  private bool _isFlipped;

  public float TipLength
  {
    get { return _tipLength; }
    set
    {
      if (value == _tipLength) { return; }
      _tipLength = value;
      _const_distance_tipBase = _const_distance_tipPoint - _tipLength;

      Vector3[] v;
      int[] vertexIndices;
      float yPos;

      // Tip
      vertexIndices = _vertexIndices_tip;
      yPos = _tipLength;
      v = _penTip.mesh.vertices;
      foreach (var index in vertexIndices)
      {
        v[index].y = yPos;
      }
      _penTip.mesh.vertices = v;

      // Shaft 1
      vertexIndices = _vertexIndices_shaft1;
      yPos = _tipLength;
      v = _penShaft.mesh.vertices; // Do shaft1, shaft2 and shaft3 together
      foreach (var index in vertexIndices)
      {
        v[index].y = yPos;
      }
      // _penShaft.mesh.vertices = v; // Do shaft1, shaft2 and shaft3 together

      // Shaft 2
      vertexIndices = _vertexIndices_shaft2;
      yPos = _tipLength + Shaft2Offset;
      //v = _penShaft.mesh.vertices; // Do shaft1, shaft2 and shaft3 together
      foreach (var index in vertexIndices)
      {
        v[index].y = yPos;
      }
      //_penShaft.mesh.vertices = v; // Do shaft1, shaft2 and shaft3 together

      // Shaft 3
      vertexIndices = _vertexIndices_shaft3;
      yPos = _tipLength * 2;
      //v = _penShaft.mesh.vertices; // Do shaft1, shaft2 and shaft3 together
      foreach (var index in vertexIndices)
      {
        v[index].y = yPos;
      }
      _penShaft.mesh.vertices = v; // Do shaft1, shaft2 and shaft3 together

      // Inner shaft
      vertexIndices = _vertexIndices_innerShaft;
      yPos = _tipLength;
      v = _penInnerShaft.mesh.vertices;
      foreach (var index in vertexIndices)
      {
        v[index].y = yPos;
      }
      _penInnerShaft.mesh.vertices = v;

      RecalculateCachedValues();
    }
  }
  private float _tipLength = 0.0f; // Start with invalid number for triggering logic on start

  public uint PressureCurve
  {
    get { return _pressureCurve; }
    set
    {
      if (value == _pressureCurve) { return; }
      _pressureCurve = value;
      RecalculateCachedValues();
    }
  }
  private uint _pressureCurve;

  public float Distance
  {
    get { return _distance; }
    set
    {
      if (value == _distance) { return; }
      for (var i = PRIOR_DISTANCE_COUNT - 1; i > 0; i--)
      {
        _priorDistances[i] = _priorDistances[i - 1];
      }
      _priorDistances[0] = _distance;
      _distance = value;
      // Pen embed adjust
      if (PenPhysics != App_Details.PenPhysicsType.None)
      {
        var t = transform.localPosition;
        if (_isFlipped)
        {
          // Beyond the pen
          if (_distance > _const_distance_tipPoint)
          {
            Pressure = 0.0f;
            t.z = _currentZPosition + 0.0f;
          }
          // Within the back end
          else if (_distance < _const_distance_eraserBase)
          {
            Pressure = 0.0f;
            t.z =
              _currentZPosition +
              (_distance - _const_distance_flippedEraserBase * _distance / _const_distance_eraserBase);
          }
          // Within the shaft
          else if (_distance < _const_distance_flippedEraserBase)
          {
            if (Pressure == 0.0f)
            {
              Audio_Bump.volume =
                (1.0f - Mathf.Pow(1.0f - (_priorDistances[PRIOR_DISTANCE_COUNT - 1] - _distance) / TOP_PEN_HIT_SPEED, 2)) * _const_volume_penHit * App_Details.Instance.Volume;
              Audio_Bump.Play(0);
            }
            Pressure = 1.0f;
            t.z = _currentZPosition + (_distance - _const_distance_flippedEraserBase);
          }
          // Within the tip
          else
          {
            if (Pressure == 0.0f)
            {
              Audio_Bump.volume =
                (1.0f - Mathf.Pow(1.0f - (_priorDistances[PRIOR_DISTANCE_COUNT - 1] - _distance) / TOP_PEN_HIT_SPEED, 2)) * _const_volume_penHit * App_Details.Instance.Volume;
              Audio_Bump.Play(0);
            }
            Pressure =
              1.0f - (Distance - _const_distance_flippedEraserBase) /
              (_const_distance_tipPoint - _const_distance_flippedEraserBase);
            t.z = _currentZPosition + 0.0f;
          }
        }
        else
        {
          // Beyond the pen
          if (_distance > _const_distance_tipPoint)
          {
            Pressure = 0.0f;
            t.z = _currentZPosition;
          }
          // Within the back end
          else if (_distance < _eraserBaseAdjust)
          {
            Pressure = 0.0f;
            t.z =
              _currentZPosition +
              _distance - _const_distance_tipBase * (_distance + _curveOffset) / _const_distance_eraserBase;
          }
          // Within the shaft
          else if (_distance < _tipBaseAdjust)
          {
            if (Pressure == 0.0f)
            {
              Audio_Bump.volume =
                (1.0f - Mathf.Pow(1.0f - (_priorDistances[PRIOR_DISTANCE_COUNT - 1] - _distance) / TOP_PEN_HIT_SPEED, 2)) * _const_volume_penHit * App_Details.Instance.Volume;
              Audio_Bump.Play(0);
            }
            Pressure = 1.0f;
            t.z = _currentZPosition + _distance - _const_distance_tipBase;
          }
          // Within the tip
          else
          {
            if (PressureCurve == 0)
            {
              Pressure =
                1.0f - (Distance - _const_distance_tipBase) /
                (_const_distance_tipPoint - _const_distance_tipBase);
              t.z = _currentZPosition;
            }
            else
            {
              var normalizedDistance =
                1.0f - (_distance - _tipBaseAdjust) / (_const_distance_tipPoint - _tipBaseAdjust);
              var adjustedNormalizedDistance =
                (1.0f - Mathf.Pow(normalizedDistance - 1.0f, PressureCurve * 2))
                * _rlCurvedTipLength - 1.0f;
              t.z = _currentZPosition + _distance + _const_distance_tipPoint * adjustedNormalizedDistance;
              if (Pressure == 0.0f && gameObject.activeInHierarchy)
              {
                Audio_Bump.volume =
                  (1.0f - Mathf.Pow(1.0f - (_priorDistances[PRIOR_DISTANCE_COUNT - 1] - _distance) / TOP_PEN_HIT_SPEED, 2)) * _const_volume_penHit * App_Details.Instance.Volume;
                Audio_Bump.Play(0);
              }
              if (App_Details.Instance.PressureLengthIndex == 0)
              {
                Pressure = 1.0f;
              }
              else
              {
                Pressure =
                  (1.0f + adjustedNormalizedDistance) / (App_Details.Instance.PressureLengthIndex == 2 ? 0.385f : 0.1694f);
              }
            }
          }
        }
        transform.localPosition = t;
      }

      // Tip embed adjust
      if (PenPhysics == App_Details.PenPhysicsType.Full)
      {
        var t = _penTipTransform.localPosition;
        t.y = _unphysicsedTipZPosition + (
          _isFlipped ? 0 :
          (_distance > _const_distance_tipPoint) ?
            0 :
          (_distance < _const_distance_tipBase) ?
            (_const_distance_tipPoint - _const_distance_tipBase)
          :
            (_const_distance_tipPoint - _distance)
        );
        _penTipTransform.localPosition = t;
      }
    }
  }
  private float _distance;
  private float[] _priorDistances = new float[PRIOR_DISTANCE_COUNT];

  public Vector3 FocusPosition
  {
    get { return _focusPosition; }
    set
    {
      if (value == _focusPosition) { return; }
      if (!gameObject.activeInHierarchy) { return; }
      for (var i = PRIOR_FOCUS_POSITION_COUNT - 1; i > 0; i--)
      {
        _priorFocusPositions[i] = _priorFocusPositions[i - 1];
      }
      _priorFocusPositions[0] = _focusPosition;
      _focusPosition = value;
      if (_focusPosition == Global.NullVec3 ||
          _priorFocusPositions[PRIOR_FOCUS_POSITION_COUNT - 1] == Global.NullVec3)
      {
        Audio_Scratch.volume = Audio_Rumble.volume = 0.0f;
      }
      else
      {
        var volume = App_Details.Instance.Volume;
        var speed =
          1.0f - (float)Math.Pow(1.0f - (_focusPosition - _priorFocusPositions[PRIOR_FOCUS_POSITION_COUNT - 1]).sqrMagnitude / TOP_PEN_MOVE_SPEED, 2.0f);
        Audio_Scratch.volume = (Pressure == 0.0f) ? 0.0f : (speed * _const_volume_penScrape * volume);
        Audio_Rumble.volume = speed * (Math.Max(0.0f, Pressure - 0.25f) / 0.75f) * _const_volume_penRumble * volume;
      }
    }
  }
  private Vector3 _focusPosition;
  private Vector3[] _priorFocusPositions = new Vector3[PRIOR_FOCUS_POSITION_COUNT];

  public float Opacity
  {
    set
    {
      if (value == _opacity) { return; }
      _opacity = value;
      foreach (var renderer in _renderers)
      {
        renderer.material.SetFloat("_Opacity", value);
      }
    }
  }
  private float _opacity;

  private static float _const_distance_tipPoint;
  private static float _const_distance_tipBase;
  private static float _const_distance_eraserBase;
  private static float _const_distance_flippedEraserBase;
  private static float _const_volume_penHit;
  private static float _const_volume_penScrape;
  private static float _const_volume_penRumble;

  private float _defaultZPosition;
  private float _currentZPosition;
  private float _unphysicsedTipZPosition;
  private Transform _penTipTransform;

  private int[] _vertexIndices_tip;
  private float[] _vertexYOrigins_tip;
  private int[] _vertexIndices_shaft1;
  private int[] _vertexIndices_shaft2;
  private int[] _vertexIndices_shaft3;
  private int[] _vertexIndices_innerShaft;

  private float _curveOffset;
  private float _tipBaseAdjust;
  private float _eraserBaseAdjust;
  private float _rlCurvedTipLength;

  private void RecalculateCachedValues()
  {
    var rlTipLengthModifier = 1.0f + PressureCurve / 8.0f;
    _curveOffset =
      (_const_distance_tipPoint - _const_distance_tipBase) * PressureCurve * rlTipLengthModifier;
    _tipBaseAdjust = _const_distance_tipBase - _curveOffset;
    _eraserBaseAdjust = _const_distance_eraserBase - _curveOffset;
    _rlCurvedTipLength = _curveOffset * 7.7f / PressureCurve / rlTipLengthModifier;
  }

  private void Awake()
  {
    // Duplicated values (for efficiency)
    _penTipTransform = _penTip.transform;
    _const_distance_tipPoint = App_Details.Instance.CONTROLLER_DISTANCE_TIP_POINT;
    _const_distance_tipBase = App_Details.Instance.CONTROLLER_DISTANCE_TIP_BASE;
    _const_distance_eraserBase = App_Details.Instance.CONTROLLER_DISTANCE_ERASER_BASE;
    _const_distance_flippedEraserBase = _const_distance_tipPoint - _const_distance_eraserBase;
    _const_volume_penHit = App_Details.Instance.VOLUME_PEN_HIT;
    _const_volume_penScrape = App_Details.Instance.VOLUME_PEN_SCRAPE;
    _const_volume_penRumble = App_Details.Instance.VOLUME_PEN_RUMBLE;

  // Setup default values
  _defaultZPosition = transform.localPosition.z;
    _unphysicsedTipZPosition = _penTipTransform.localPosition.z;
    _currentZPosition = _defaultZPosition;

    Mesh mesh;
    float vertexPosition;
    List<int> vertexIndexList = new List<int>();

    mesh = _penTip.mesh;
    vertexPosition = InitialVertexPosition_Tip;
    vertexIndexList.Clear();
    for (var i = 0; i < mesh.vertexCount; i++)
    {
      if (mesh.vertices[i].y != 0) // Take ALL except single vertex at origin
      {
        vertexIndexList.Add(i);
      }
    }
    _vertexIndices_tip = vertexIndexList.ToArray();

    mesh = _penShaft.mesh;
    vertexPosition = InitialVertexPosition_Shaft1;
    vertexIndexList.Clear();
    for (var i = 0; i < mesh.vertexCount; i++)
    {
      if (mesh.vertices[i].y == vertexPosition)
      {
        vertexIndexList.Add(i);
      }
    }
    _vertexIndices_shaft1 = vertexIndexList.ToArray();

    mesh = _penShaft.mesh;
    vertexPosition = InitialVertexPosition_Shaft2;
    vertexIndexList.Clear();
    for (var i = 0; i < mesh.vertexCount; i++)
    {
      if (mesh.vertices[i].y == vertexPosition)
      {
        vertexIndexList.Add(i);
      }
    }
    _vertexIndices_shaft2 = vertexIndexList.ToArray();

    mesh = _penShaft.mesh;
    vertexPosition = InitialVertexPosition_Shaft3;
    vertexIndexList.Clear();
    for (var i = 0; i < mesh.vertexCount; i++)
    {
      if (mesh.vertices[i].y == vertexPosition)
      {
        vertexIndexList.Add(i);
      }
    }
    _vertexIndices_shaft3 = vertexIndexList.ToArray();

    mesh = _penInnerShaft.mesh;
    vertexPosition = InitialVertexPosition_InnerShaft;
    vertexIndexList.Clear();
    for (var i = 0; i < mesh.vertexCount; i++)
    {
      if (mesh.vertices[i].y == vertexPosition)
      {
        vertexIndexList.Add(i);
      }
    }
    _vertexIndices_innerShaft = vertexIndexList.ToArray();

    // Get y-origins for all tip vertices that aren't the tip's point vertex
    _vertexYOrigins_tip = new float[_vertexIndices_tip.Length];
    for (var i = 0; i < _vertexYOrigins_tip.Length; i++)
    {
      _vertexYOrigins_tip[i] =
        _penTip.mesh.vertices[_vertexIndices_tip[i]].y - InitialVertexPosition_Tip;
    }

    // Give default tip length HERE to trigger logic (now that needed data is collected for logic)
    TipLength = InitialVertexPosition_Tip;

    for (var i = 0; i < PRIOR_FOCUS_POSITION_COUNT; i++)
    {
      _priorFocusPositions[i] = Global.NullVec3;
    }
  }
}
