using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// アタッチしたオブジェクトのRigidbodyをSleepするボタンを表示させる
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SleepRigidbody : MonoBehaviour
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
    if(GUI.Button(buttonPos, name + "のRigidbodyをSleep"))
    {
      rig.Sleep();
    }
  }
}
