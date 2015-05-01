using MBL.Balloon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestEvent : MonoBehaviour, IBalloonEvent
{
  private bool eventStart = false;

  public void EventAction()
  {
    eventStart = true;
  }

  public void Update()
  {
    if(eventStart)
      transform.Translate(Vector3.up * Time.deltaTime, Space.World);
  }
}
