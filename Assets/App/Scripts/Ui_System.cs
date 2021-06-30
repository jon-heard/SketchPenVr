using UnityEngine;

public class Ui_System : MonoBehaviour
{
  [SerializeField] private Transform screen;

  private void Start()
  {
    transform.parent = screen;
  }
}
