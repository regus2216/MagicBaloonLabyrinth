using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Gimmick.Lift
{
  /// <summary>
  /// リングリフトのリングの回転
  /// </summary>
  public class RotateRingLift : MonoBehaviour
  {
    public void Update()
    {
      Rotate();
    }

    /// <summary>
    /// プレイヤーの位置からリングを回転する
    /// </summary>
    private void Rotate()
    {
      transform.Rotate(Vector3.up, 1);
    }
  }
}
