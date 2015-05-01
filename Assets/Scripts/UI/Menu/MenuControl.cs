using MBL.Charactor.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MBL.UI.Menu
{
  [RequireComponent(typeof(Camera))]
  public class MenuControl : MonoBehaviour
  {
    [SerializeField, Tooltip("描画するキャンバスオブジェクト")]
    private GameObject renderCanvasObject = null;
    [SerializeField, Tooltip("プレイヤーの入力操作処理を行うスクリプト")]
    private ActionWithInput playerScript = null;

    //[SerializeField, Tooltip("PlayerのRigidbody")]
    //private Rigidbody playerRigidbody;

    //private Vector3 velocity_cache;
    //private Vector3 angular_velocity_cache;

    private IEnumerable<Behaviour> allBehaviour;

    public void Update()
    {
      //メニューボタンが押されたら
      if(Input.GetButtonDown("Menu") && playerScript.IsGrounded && !playerScript.IsJumpping)
      {
        allBehaviour = FindObjectsOfType<Behaviour>().Where(obj => obj.GetComponent<Camera>() == null);
        renderCanvasObject.SetActive(!renderCanvasObject.activeInHierarchy);

        //カメラのついてないBehaviourオブジェクトを取得

        //メニュー画面表示時の動作
        if(renderCanvasObject.activeInHierarchy)
        {
          foreach(var obj in allBehaviour)
            obj.enabled = false;

          //velocity_cache = playerRigidbody.velocity;
          //angular_velocity_cache = playerRigidbody.angularVelocity;
          //playerRigidbody.Sleep();
        }

        //メニュー終了時の動作
        else
        {
          foreach(var obj in allBehaviour)
            obj.enabled = true;

          //playerRigidbody.WakeUp();
          //playerRigidbody.velocity = velocity_cache;
          //playerRigidbody.angularVelocity = angular_velocity_cache;
        }
      }
    }
  }
}
