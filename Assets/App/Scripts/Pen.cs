using System;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
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
    }
  }
  private float _tipLength = 0.0f; // Start with invalid number for triggering logic on start

  public float Distance
  {
    get { return _distance; }
    set
    {
      if (value == _distance) { return; }
      _distance = value;
      if (_isFlipped)
      {
        Pressure =
          (Distance > _const_distance_tipPoint) ? 0.0f :
          (Distance < _const_distance_flippedEraserBase) ? 1.0f :
          1.0f - (Distance - _const_distance_flippedEraserBase) /
            (_const_distance_tipPoint - _const_distance_flippedEraserBase);
      }
      else
      {
        Pressure =
          (Distance > _const_distance_tipPoint) ? 0.0f :
          (Distance < _const_distance_tipBase) ? 1.0f :
          1.0f - (Distance - _const_distance_tipBase) /
            (_const_distance_tipPoint - _const_distance_tipBase);
      }
      // Pen embed adjust
      if (PenPhysics != App_Details.PenPhysicsType.None)
      {
        var t = transform.localPosition;
        if (_isFlipped)
        {
          // Outside the pen
          if (_distance > _const_distance_tipPoint)
          {
            t.z = _currentZPosition + 0.0f;
          }
          // Within the back end
          else if (_distance < _const_distance_eraserBase)
          {
            t.z =
              _currentZPosition +
              (_distance - _const_distance_flippedEraserBase * _distance / _const_distance_eraserBase);
          }
          // Within the shaft
          else if (_distance < _const_distance_flippedEraserBase)
          {
            t.z = _currentZPosition + (_distance - _const_distance_flippedEraserBase);
          }
          // Within the tip
          else
          {
            t.z = _currentZPosition + 0.0f;
          }
        }
        else
        {
          // Outside the pen
          if (_distance > _const_distance_tipPoint)
          {
            t.z = _currentZPosition;
          }
          // Within the back end
          else if (_distance < _const_distance_eraserBase)
          {
            t.z =
              _currentZPosition +
              (_distance - _const_distance_tipBase * _distance / _const_distance_eraserBase);
          }
          // Within the shaft
          else if (_distance < _const_distance_tipBase)
          {
            t.z = _currentZPosition + _distance - _const_distance_tipBase;
          }
          // Within the tip
          else
          {
            //var normalizedDistance =
            //  (_distance - _const_distance_tipBase) /
            //  (_const_distance_tipPoint - _const_distance_tipBase);
            //Debug.Log(normalizedDistance);
            //var eased = Mathf.Pow(normalizedDistance, 3);// Mathf.Pow(normalizedDistance - 1, 3) + 1;
            t.z = _currentZPosition + 0.0f;// (_distance - _const_distance_tipBase) - eased * (_const_distance_tipPoint - _const_distance_tipBase);
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

  private void Awake()
  {
    // Duplicated values (for efficiency)
    _penTipTransform = _penTip.transform;
    _const_distance_tipPoint = App_Details.Instance.CONTROLLER_DISTANCE_TIP_POINT;
    _const_distance_tipBase = App_Details.Instance.CONTROLLER_DISTANCE_TIP_BASE;
    _const_distance_eraserBase = App_Details.Instance.CONTROLLER_DISTANCE_ERASER_BASE;
    _const_distance_flippedEraserBase = _const_distance_tipPoint - _const_distance_eraserBase;

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
  }
}
