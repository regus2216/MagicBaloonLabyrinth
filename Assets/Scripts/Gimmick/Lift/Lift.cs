using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Gimmick.Lift
{
  public class Lift : MonoBehaviour
  {
    [SerializeField]
    private Transform liftPos = null;

    private bool onPlayer = false;

    private Transform PlayerTransform { get; set; }
    private Vector3 previous;

    public void Update()
    {
      transform.position = liftPos.position;

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
