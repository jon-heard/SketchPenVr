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
