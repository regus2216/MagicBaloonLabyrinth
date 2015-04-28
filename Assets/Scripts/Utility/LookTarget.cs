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
    [SerializeField, Tooltip("ターゲットの指定がない場合メインカメラに設定するか")]
    private bool autoCam = true;
    [SerializeField, Tooltip("方向転換の際のターゲット")]
    private Transform target;
    [SerializeField, Tooltip("縦軸方向の回転を有効にするか")]
    private bool enableVerticalRotation = false;

    public void Start()
    {
      if(target == null && autoCam)
        target = Camera.main.transform;
    }

    public void Update()
    {
      if(enableVerticalRotation)
      {
        this.transform.LookAt(target.position);
      }
      else
      {
        Vector3 target_exceptY = this.target.position;
        target_exceptY.y = this.transform.position.y;
        this.transform.LookAt(target_exceptY);
      }
    }
  }
}
