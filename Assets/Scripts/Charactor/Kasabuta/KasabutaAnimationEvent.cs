using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Charactor.Kasabuta
{
  public class KasabutaAnimationEvent : MonoBehaviour
  {
    [SerializeField, Tooltip("WindZoneControlスクリプト")]
    private WindZoneControl windCtrl = null;

    /// <summary>
    /// 風を吹かせるフラグをONにする
    /// </summary>
    public void StartBlowing()
    {
      windCtrl.Blowing = true;
    }

    public void StopBlowing()
    {
      windCtrl.Blowing = false;
    }
  }
}
