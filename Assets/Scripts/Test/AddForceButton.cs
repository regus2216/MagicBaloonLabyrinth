using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddForceButton : MonoBehaviour
{
  private Rigidbody rig;

  [SerializeField]
  private Vector3 direction = Vector3.forward;
  [SerializeField]
  private float power = 10f;
  [SerializeField]
  private Rect buttonPos = new Rect(0, 0, 100, 50);

  public void Awake()
  {
    rig = GetComponent<Rigidbody>();
  }

  public void OnGUI()
  {
    if(GUI.Button(buttonPos, name + "に力を加える"))
    {
      rig.AddForce(direction.normalized * power);
    }
  }
}
