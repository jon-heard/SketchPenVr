using UnityEngine;

public class BillBoard : MonoBehaviour
{
  [SerializeField] private bool _reversed = false;

  private void OnEnable()
  {
    Camera.onPreRender += OnCameraPreRender;
  }

  private void OnDisable()
  {
    Camera.onPreRender -= OnCameraPreRender;
  }

  private void OnCameraPreRender(Camera c)
  {
    if (_reversed)
    {
      var awayDirection = transform.position - c.transform.position;
      var awayRotation = Quaternion.LookRotation(awayDirection);
      transform.rotation = awayRotation;
    }
    else
    {
      transform.LookAt(c.transform);
    }
  }
}
