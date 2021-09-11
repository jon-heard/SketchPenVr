using System.Collections.Generic;
using UnityEngine;

namespace Common
{
  public static class Global
  {
    public static readonly Vector2 NullVec2 = new Vector2(float.MaxValue, float.MaxValue);
    public static readonly Vector3 NullVec3 =
      new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    public static readonly Vector3 MaxVec3 =
      new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    public static readonly Vector3 MinVec3 =
      new Vector3(float.MinValue, float.MinValue, float.MinValue);

    public const uint NullUint = uint.MaxValue;
    public const float NullFloat = float.MaxValue;

    public static Vector2Int ToIntVector(this Vector2 source)
    {
      return new Vector2Int((int)source.x, (int)source.y);
    }

    public static float GetTextWidth(this TextMesh label)
    {
      var anglesBuffer = label.transform.eulerAngles;
      label.transform.eulerAngles = Vector3.zero;
      var result = label.GetComponent<MeshRenderer>().bounds.extents.x * 2;
      label.transform.eulerAngles = anglesBuffer;
      return result;
    }

    public static Vector3 ClampComponents(this Vector3 source, Vector3 low, Vector3 high)
    {
      return new Vector3(
        Mathf.Clamp(source.x, low.x, high.x),
        Mathf.Clamp(source.y, low.y, high.y),
        Mathf.Clamp(source.z, low.z, high.z));
    }

    public static Vector3 GetScaled(this Vector3 source, Vector3 scale)
    {
      return new Vector3(source.x * scale.x, source.y * scale.y, source.z * scale.z);
    }

    public static Vector3 GetAverageVector(List<Vector3> vectors)
    {
      if (vectors.Count == 0) { return Vector3.zero; }
      if (vectors.Count == 1) { return vectors[0]; }

      Vector3 result = Vector3.zero;
      foreach (var vector in vectors)
      {
        result += vector;
      }
      return result / vectors.Count;
    }
  }
}
