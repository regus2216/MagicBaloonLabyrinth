using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Balloon
{
  public class BalloonEventCaller : MonoBehaviour
  {
    public void SetBalloonEvent(IBalloonEvent balloonEvent)
    {
      //イベントを呼び出す
      if(balloonEvent != null)
        balloonEvent.EventAction();
    }
  }
}
