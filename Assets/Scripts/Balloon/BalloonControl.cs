using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Balloon
{
  public class BalloonControl : MonoBehaviour
  {
    [SerializeField, Tooltip("浮く速度")]
    private float flowSpeed = 1f;
    [SerializeField, Tooltip("風船の生存時間")]
    private float lifeTime = 10f;

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

    /// <summary>
    /// 生存時間を設定し、targetを取らずに浮いていく
    /// </summary>
    public void SetFlow()
    {
      Invoke("SetDestroy", lifeTime);
      flow = true;
    }

    /// <summary>
    /// 追従するTransformを設定する
    /// 同じ場所にセットされた風船がある場合はそのまま浮いていく
    /// </summary>
    public void SetTraceTarget(Transform target, bool setDestroyLifeTime = false)
    {
      traceTarget = target;

      //生存時間がセットされた場合
      if(setDestroyLifeTime)
        Invoke("SetDestroy", lifeTime);

      //ターゲットの位置に既に別の風船が存在したらターゲットをロストして浮いていく
      if(Physics.OverlapSphere(traceTarget.position, 0.01f)
        .FirstOrDefault(coll => coll.tag == "Balloon" && coll != GetComponent<Collider>()) != null)
      {
        target = null;
        SetFlow();
      }
    }

    /// <summary>
    /// 風船消滅時の動作
    /// </summary>
    private void SetDestroy()
    {
      if(traceTarget != null && traceTarget.GetComponent<IBalloonEvent>() != null)
        traceTarget.SendMessage("MissingBalloon");
      Destroy(this.gameObject);
    }
  }
}
