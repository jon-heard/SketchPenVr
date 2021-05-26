using UnityEngine;

public static class App_Global
{
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
}
