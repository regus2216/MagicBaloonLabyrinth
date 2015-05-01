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
    [SerializeField]
    private bool rotate = true;
    [SerializeField]
    private float rotateSpeed = 15f;
    [SerializeField]
    private bool rotateInverse = false;

    public void Update()
    {
      if(rotate)
        Rotate();
    }

    /// <summary>
    /// リングを回転する
    /// </summary>
    private void Rotate()
    {
      if(!rotateInverse)
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
      else
        transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
    }
  }
}
