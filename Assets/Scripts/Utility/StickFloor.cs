using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  /// <summary>
  /// プレイヤーが乗った際にプレイヤーが吸い付くようにする
  /// 動く床を表現する際に取り付ける
  /// </summary>
  public class StickFloor : MonoBehaviour
  {
    private bool onPlayer = false;

    private Transform PlayerTransform { get; set; }
    private Vector3 previous;

    public void Update()
    {
      //プレイヤーの移動
      if(onPlayer)
      {
        PlayerTransform.Translate(transform.position - previous, Space.World);
        previous = transform.position;
      }
    }

    public void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.tag == "Player")
      {
        //プレイヤーの位置情報をキャッシュ
        if(PlayerTransform == null)
          PlayerTransform = collision.transform;

        //横にぶつかっただけでも少し移動するのを防ぐ
        if(PlayerTransform.position.y > transform.position.y)
        {
          //移動させるようにする
          previous = transform.position;
          onPlayer = true;
        }
      }
    }

    public void OnCollisionExit(Collision collision)
    {
      if(collision.gameObject.tag == "Player")
        onPlayer = false;
    }
  }
}
