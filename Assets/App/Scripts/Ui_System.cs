using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_System : MonoBehaviour
{
  [SerializeField] private Transform screen;

  private void Start()
  {
    transform.parent = screen;
  }
}
