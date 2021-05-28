using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOriented : MonoBehaviour
{
  private void Update()
  {
    var c = Camera.current;
    if (c)
    {
//      transform.LookAt(c.transform);
    }
  }
}
