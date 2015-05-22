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
[RequireComponent(typeof(Fall))]
public class TestEvent : MonoBehaviour, IBalloonEvent
{
  [SerializeField]
  private float speed = 3f;
  private bool eventStart = false;

  private Fall _fall = null;
  private Fall FallScript
  {
    get
    {
      if(_fall == null)
        _fall = GetComponent<Fall>();
      return _fall;
    }
  }

  //1度しか取り付けられない
  private bool possibleSet = true;

  public bool WhetherPossibleSet()
  {
    //取付可能かどうか
    return possibleSet;
  }

  public void EventAction()
  {
    eventStart = true;
    possibleSet = false;
    FallScript.enabled = false;
  }

  public void MissingBalloon()
  {
    eventStart = false;
    FallScript.enabled = true;
  }

  public void Update()
  {
    if(eventStart)
      transform.Translate(speed * Vector3.up * Time.deltaTime, Space.World);
  }
}
