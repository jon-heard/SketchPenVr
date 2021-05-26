using System;
using UnityEngine;

public class TransformSerializer : MonoBehaviour
{
  public string SerializedTransform
  {
    get
    {
      return JsonUtility.ToJson(UnserializedTransform);
    }
    set
    {
      if (value != "")
      {
        UnserializedTransform = JsonUtility.FromJson<SerializableTransform>(value);
      }
      else
      {
        transform.localPosition = transform.localEulerAngles = Vector3.zero;
      }
    }
  }
  public SerializableTransform UnserializedTransform
  {
    get
    {
      var result = new SerializableTransform();
      result.position = transform.localPosition;
      result.rotation = transform.localEulerAngles;
      return result;
    }
    set
    {
      transform.localPosition = value.position;
      transform.localEulerAngles = value.rotation;
    }
  }

  [Serializable]
  public struct SerializableTransform
  {
    public Vector3 position;
    public Vector3 rotation;
  }
}
