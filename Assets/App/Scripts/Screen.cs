using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
  public enum ScreenLockType { None, Plane, Full }

  [SerializeField] private Transform[] Geometries;

  [NonSerialized] public ScreenLockType LockType;
  public PointerEmulation Pointer { get; private set; }

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
}
