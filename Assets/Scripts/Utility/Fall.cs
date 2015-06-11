using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  public class Fall : MonoBehaviour
  {
    [SerializeField]
    private float gravityScale = 3f;
    [SerializeField]
    private Transform groundCheckPos = null;

    private float acc = 0f;

    public bool IsGround
    {
      get { return Physics.OverlapSphere(groundCheckPos.position, 0.01f).Any(c => c.tag == "Ground"); }
    }

    public void Update()
    {
      //接地していない場合
      if(!IsGround)
      {
        //重力加速度の追加
        acc += gravityScale * Time.deltaTime;

        transform.Translate(Vector3.down * acc * Time.deltaTime, Space.World);
      }
      else
        acc = 0;
    }
  }
}
