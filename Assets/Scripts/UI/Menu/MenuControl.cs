using MBL.Charactor.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MBL.UI.Menu
{
  /// <summary>
  /// Behaviourをもつオブジェクトを全て停止させる
  /// 今後Rigidbodyを持つキャラを追加することがある場合
  /// ・キャラクター全てにGUIDを設定
  /// ・GUIDをキーとして、velocity/angularvelocityを保存する辞書を作成
  /// ・メニュー解除時に元に戻す
  /// 等の対応が必要
  /// </summary>
  public class MenuControl : MonoBehaviour
  {
    [SerializeField, Tooltip("描画するキャンバスオブジェクト")]
    private GameObject renderCanvasObject = null;
    [SerializeField, Tooltip("プレイヤーの入力操作処理を行うスクリプト")]
    private ActionWithInput playerScript = null;
    [SerializeField, Tooltip("メニューOPEN時の選択オブジェクト")]
    private GameObject openSelectedItem = null;

    private Animator anim_cache;
    public Animator Anim
    {
      get
      {
        if(anim_cache == null)
          anim_cache = GetComponent<Animator>();
        return anim_cache;
      }
    }

    //[SerializeField, Tooltip("PlayerのRigidbody")]
    //private Rigidbody playerRigidbody;

    //private Vector3 velocity_cache;
    //private Vector3 angular_velocity_cache;

    private IEnumerable<Behaviour> allBehaviour;

    public void Start()
    {
      renderCanvasObject.SetActive(false);
    }

    public void Update()
    {
      //メニューボタンが押されたら
      if(Input.GetButtonDown("Menu") && playerScript.IsGrounded && !playerScript.IsJumpping && playerScript.IsAllowInput)
      {
        //MenuControlとEventSystemのついてないBehaviourオブジェクトを取得
        allBehaviour = FindObjectsOfType<Behaviour>()
          .Where(obj => obj.GetComponent<MenuControl>() == null && obj.GetComponent<EventSystem>() == null);
        renderCanvasObject.SetActive(!renderCanvasObject.activeInHierarchy);

        //メニュー画面表示時の動作
        if(renderCanvasObject.activeInHierarchy)
        {
          foreach(var obj in allBehaviour)
            obj.enabled = false;

          //アニメーション
          Anim.SetTrigger("EnterMenu");

          //velocity_cache = playerRigidbody.velocity;
          //angular_velocity_cache = playerRigidbody.angularVelocity;
          //playerRigidbody.Sleep();
        }

        //メニュー終了時の動作
        else
        {
          foreach(var obj in allBehaviour)
            obj.enabled = true;

          //アニメーション
          Anim.SetTrigger("LeaveMenu");

          //playerRigidbody.WakeUp();
          //playerRigidbody.velocity = velocity_cache;
          //playerRigidbody.angularVelocity = angular_velocity_cache;
        }
      }
    }

    public void OpenMenu()
    {
      allBehaviour = FindObjectsOfType<Behaviour>().Where(obj => obj.GetComponent<MenuControl>() == null);
      renderCanvasObject.SetActive(true);

      //メニュー画面表示時の動作
      foreach(var obj in allBehaviour)
        obj.enabled = false;

      var e = FindObjectOfType<EventSystem>();
      e.SetSelectedGameObject(openSelectedItem);

      //アニメーション
      Anim.SetTrigger("EnterMenu");
    }

    public void CloseMenu()
    {
      allBehaviour = FindObjectsOfType<Behaviour>()
        .Where(obj => obj.GetComponent<MenuControl>() == null);
      renderCanvasObject.SetActive(false);

      foreach(var obj in allBehaviour)
        obj.enabled = true;

      //アニメーション
      Anim.SetTrigger("LeaveMenu");
    }
  }
}
