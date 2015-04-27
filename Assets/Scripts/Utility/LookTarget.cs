using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  /// <summary>
  /// 設定した方向を常に向くようにする
  /// </summary>
  public class LookTarget : MonoBehaviour
  {
    [Tooltip("方向転換の際のターゲット")]
    public GameObject target;
    [Tooltip("縦軸方向の回転を有効にするか")]
    private bool enableVerticalRotation = false;

    public void Update()
    {
      if(enableVerticalRotation)
      {
        this.transform.LookAt(this.target.transform.position);
      }
      else
      {
        Vector3 target = this.target.transform.position;
        target.y = this.transform.position.y;
        this.transform.LookAt(target);
      }
    }
  }
}
