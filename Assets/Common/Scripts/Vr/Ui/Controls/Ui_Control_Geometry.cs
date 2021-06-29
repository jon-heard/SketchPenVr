using UnityEngine;

public class Ui_Control_Geometry : MonoBehaviour
{
#if UNITY_EDITOR
  public void OnMouseDown()
  {
    transform.parent.GetComponent<Ui_Control>().DoClick();
  }
#endif
}
