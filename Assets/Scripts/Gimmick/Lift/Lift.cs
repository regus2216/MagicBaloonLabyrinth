using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Gimmick.Lift
{
  public class Lift : MonoBehaviour
  {
    [SerializeField, Tooltip("プレイヤーがリフトにのっているか")]
    private bool onPlayer = false;

    public void Update()
    {
      //リフトの角度を変更
      transform.rotation = Quaternion.Euler(Vector3.zero);
      if(onPlayer)
      {
        //リフトに回転するよう通知
      }
    }
  }
}
