using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WakeUpRigidbody : MonoBehaviour
{
  private Rigidbody rig;
  [SerializeField]
  private Rect buttonPos = new Rect(0, 0, 100, 50);

  public void Awake()
  {
    rig = GetComponent<Rigidbody>();
  }

  public void OnGUI()
  {
    if(GUI.Button(buttonPos, name + "のRigidbodyをWakeUp"))
    {
      rig.WakeUp();
    }
  }
}
