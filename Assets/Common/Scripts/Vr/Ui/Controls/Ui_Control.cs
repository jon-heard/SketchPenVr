using System.Collections.Generic;
using UnityEngine;

public class Ui_Control : MonoBehaviour
{
  public static List<Ui_Control> Instances = new List<Ui_Control>();

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

  protected virtual void DoClickInternal() {}
}
