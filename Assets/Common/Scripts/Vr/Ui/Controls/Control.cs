using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Control : MonoBehaviour
  {
    public static List<Control> Instances = new List<Control>();

    public ObjectLocker Locker { get; private set; }

    public static bool WasPrimaryController = false;
    public static Action<Control> OnControlClicked;
    public static Action<Control> OnControlDown;
    public static Action<Control> OnControlUp;
    public static Action<Control> OnControlHovered;
    public static Action<Control> OnControlUnhovered;
    public static Action<Vector3> OnPointerMoved;

    protected virtual void Awake()
    {
      Locker = new ObjectLocker();
      Instances.Add(this);
    }
  }
}
