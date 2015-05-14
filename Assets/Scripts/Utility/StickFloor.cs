using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  /// <summary>
  /// プレイヤーが乗った際にプレイヤーが横軸の移動に対して吸い付くようにする
  /// 動く床を表現する際に取り付ける
  /// </summary>
  public class StickFloor : MonoBehaviour
  {
    [SerializeField, Tooltip("プレイヤーの足元")]
    private Transform playerGroundCheck = null;
    private bool onPlayer = false;

    private Transform playerTransform;
    private Vector3 previous;

    public void Update()
    {
      //プレイヤーの移動
      if(onPlayer)
      {
        //横にぶつかっただけでも少し移動するのを防ぐ
        if(playerGroundCheck.position.y > transform.position.y)
        {
          playerTransform.Translate(transform.position - previous, Space.World);
          previous = transform.position;
        }
      }
    }

    public void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.tag == "Player")
      {
        playerTransform = collision.gameObject.transform;

        //移動させるようにする
        previous = transform.position;
        onPlayer = true;
      }
    }

    public void OnCollisionExit(Collision collision)
    {
      if(collision.gameObject.tag == "Player")
        onPlayer = false;
    }
  }
}
