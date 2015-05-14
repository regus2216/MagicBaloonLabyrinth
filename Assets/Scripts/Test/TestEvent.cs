using MBL.Balloon;
using MBL.Utility;
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
  [SerializeField]
  private float speed = 3f;

  private bool eventStart = false;

  //1度しか取り付けられない
  private bool possibleSet = true;

  public bool WhetherPossibleSet()
  {
    return possibleSet;
  }

  public void EventAction()
  {
    eventStart = true;
    possibleSet = false;
  }

  public void MissingBalloon()
  {
    eventStart = false;
  }

  public void Update()
  {
    if(eventStart)
      transform.Translate(speed * Vector3.up * Time.deltaTime, Space.World);
  }
}
