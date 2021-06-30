using System.Collections.Generic;
using UnityEngine;

namespace Common.Vr.Ui.Controls
{
  public class Control : MonoBehaviour
  {
    public static List<Control> Instances = new List<Control>();

    public ObjectLocker Locker { get; private set; }

    public void DoClick()
    {
      if (!Locker.IsLocked)
      {
        DoClickInternal();
      }
    }

    protected virtual void Awake()
    {
      Locker = new ObjectLocker();
      Instances.Add(this);
    }

    protected virtual void DoClickInternal() { }
  }
}
