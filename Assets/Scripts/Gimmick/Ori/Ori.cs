using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ori : MonoBehaviour
{
  public SwitchEventCaller eventCaller;
  Animator anim;

  public void Awake()
  {
    anim = GetComponent<Animator>();
  }




  public void Start()
  {
    eventCaller.switchOnAction += () =>
    {
      anim.SetTrigger("Open");
    };

    eventCaller.switchOffAction += () =>
    {
      anim.SetTrigger("Close");
    };
  }


}
