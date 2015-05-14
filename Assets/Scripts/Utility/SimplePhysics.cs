using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  public class SimplePhysics : MonoBehaviour
  {
    [SerializeField]
    private float gravityScale = 1f;
    [SerializeField]
    private float mass = 1f;
    [SerializeField, Tooltip("接地判定を取る際の位置")]
    private Transform groundCheckPos = null;
    [SerializeField, Tooltip("接地判定オブジェクトのタグ")]
    private string groundTag = "Ground";

    public bool IsGround
    {
      get { return Physics.OverlapSphere(groundCheckPos.position, 0.01f).Any(c => c.tag == groundTag); }
    }

    public void Update()
    {
      if(!IsGround)
        transform.Translate(Vector3.down * gravityScale * mass * Time.deltaTime, Space.World);
    }
  }
}
