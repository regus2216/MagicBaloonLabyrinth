using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Balloon
{
  public class BalloonPositionControl : MonoBehaviour
  {
    [SerializeField]
    private float flowSpeed = 1f;

    private bool flow = false;
    private Transform traceTarget;

    public void Update()
    {
      //追跡ターゲットがない場合は浮いていく
      if(flow)
        transform.Translate(Vector3.up * flowSpeed * Time.deltaTime, Space.World);
      else if(traceTarget != null)
        transform.position = traceTarget.position;
    }

    public void SetFlow()
    {
      flow = true;
    }

    /// <summary>
    /// 追従するTransformを設定する
    /// 同じ場所にセットされた風船がある場合は消滅する
    /// </summary>
    public void SetTraceTarget(Transform target)
    {
      traceTarget = target;

      //ターゲットの位置に既に別の風船が存在したら自身を消滅
      if(Physics.OverlapSphere(traceTarget.position, 0.01f)
        .FirstOrDefault(coll => coll.tag == "Balloon" && coll != GetComponent<Collider>()) != null)
        GameObject.Destroy(gameObject);
    }
  }
}
