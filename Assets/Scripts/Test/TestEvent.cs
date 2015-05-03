using MBL.Balloon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 風船イベント処理テスト
/// </summary>
public class TestEvent : MonoBehaviour, IBalloonEvent
{
  private bool eventStart = false;
  private bool missing = false;

  public void EventAction()
  {
    eventStart = true;
  }

  public void MissingBalloon()
  {
    eventStart = false;
    missing = true;
  }

  public void Update()
  {
    if(eventStart)
      transform.Translate(Vector3.up * Time.deltaTime, Space.World);
    if(missing)
      transform.Translate(Vector3.down * Time.deltaTime, Space.World);
  }
}
