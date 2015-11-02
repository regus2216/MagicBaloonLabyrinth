using MBL.Balloon;
using MBL.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Gimmick.Plane
{
  [RequireComponent(typeof(Fall))]
  public class FlowPlane : MonoBehaviour, IBalloonEvent
  {
    [SerializeField]
    private float speed = 3f;
    private bool eventStart = false;

    private Fall _fall = null;
    private bool possibleSet = true;
    private Fall FallScript
    {
      get
      {
        if(_fall == null)
          _fall = GetComponent<Fall>();
        return _fall;
      }
    }

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
      Destroy(gameObject);
    }

    public void Update()
    {
      if(eventStart)
        transform.Translate(speed * Vector3.up * Time.deltaTime, Space.World);
    }
  }
}
