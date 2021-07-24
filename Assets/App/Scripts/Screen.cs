using System.Collections;
using UnityEngine;

public class Screen : MonoBehaviour
{
  public enum ScreenLockType { None, Plane, Full }

  [SerializeField] private Transform[] Geometries;

  public PointerEmulation Pointer { get; private set; }

  public ScreenLockType LockType
  {
    get { return _lockType; }
    set
    {
      if (value == _lockType) { return; }
      _lockType = value;
      if (_lockType == ScreenLockType.Plane)
      {
        _lockRotation = transform.rotation;
        _lockPosition = transform.position;
        _planeLockCoroutine = StartCoroutine(PlaneLockCoroutine());
      }
      else if (_planeLockCoroutine != null)
      {
        StopCoroutine(_planeLockCoroutine);
        _planeLockCoroutine = null;
      }
    }
  }
  public ScreenLockType _lockType;

  private Vector3 _lockPosition;
  private Quaternion _lockRotation;
  private Coroutine _planeLockCoroutine;

  private void Start()
  {
    Pointer = GetComponent<PointerEmulation>();
    var yScale = (float)Pointer.Resolution.y / Pointer.Resolution.x * -1.0f;
    foreach (var geometry in Geometries)
    {
      var t = geometry.localScale;
      t.y = yScale;
      geometry.localScale = t;
    }
  }

  private IEnumerator PlaneLockCoroutine()
  {
    while (true)
    {
      yield return new WaitForEndOfFrame();
      // Lock rotation, except for local Z rotation
      var zRot = transform.localEulerAngles.z;
      transform.rotation = _lockRotation;
      var t = transform.localEulerAngles;
      t.z = zRot;
      transform.localEulerAngles = t;
      var distance = Vector3.Dot(transform.position - _lockPosition, transform.forward);
      transform.position -= transform.forward * distance;
    }
  }
}
