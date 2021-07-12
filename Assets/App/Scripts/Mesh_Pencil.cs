using System.Collections.Generic;
using UnityEngine;

public class Mesh_Pencil : MonoBehaviour
{
  private const float Tip1InitialPosition = 0.022f;
  private const float Tip2InitialPosition = 0.029672f;
  private const float DistanceFromTip1ToTip2 = Tip2InitialPosition - Tip1InitialPosition;

  public float TipLength = 0.022f;
  private float _tipLength;

  private static List<Mesh_Pencil> _instances = new List<Mesh_Pencil>();
  public static void SetAllTipLengths(float length)
  {
    foreach (var instance in _instances)
    {
      instance.SetTipLength(length);
    }
  }

  public void SetTipLength(float Length)
  {
    var v = _mesh.vertices;
    foreach (var index in _tip1VerticexIndices)
    {
      v[index].y = Length;
    }
    var tip2Position = Length + DistanceFromTip1ToTip2;
    foreach (var index in _tip2VerticexIndices)
    {
      v[index].y = tip2Position;
    }
    _mesh.vertices = v;
  }

  private Mesh _mesh;
  private int[] _tip1VerticexIndices;
  private int[] _tip2VerticexIndices;

  private void Start()
  {
    _instances.Add(this);
    _mesh = GetComponent<MeshFilter>().mesh;
    _tipLength = Tip1InitialPosition;
    var tip1VertexIndices = new List<int>();
    var tip2VertexIndices = new List<int>();
    for (var i = 0; i < _mesh.vertexCount; i++)
    {
      if (_mesh.vertices[i].y == Tip1InitialPosition)
      {
        tip1VertexIndices.Add(i);
      }
      if (_mesh.vertices[i].y == Tip2InitialPosition)
      {
        tip2VertexIndices.Add(i);
      }
    }
    _tip1VerticexIndices = tip1VertexIndices.ToArray();
    _tip2VerticexIndices = tip2VertexIndices.ToArray();
  }

  private void Update()
  {
    if (TipLength != _tipLength)
    {
      _tipLength = TipLength;
      SetTipLength(TipLength);
    }
  }
}
