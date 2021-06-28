using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Control_ButtonGeometry : MonoBehaviour
{
#if UNITY_EDITOR
  public void OnMouseDown()
  {
    transform.parent.GetComponent<Ui_Control_Button>().DoClick();
  }
#endif
}
