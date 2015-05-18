using MBL.Charactor.Player;
using MBL.UI.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Charactor.Speaker
{
  /// <summary>
  /// 会話コントローラを参照して会話システムを起動する
  /// </summary>
  public class Speak : MonoBehaviour
  {
    [SerializeField]
    private string charactorName = null;
    [SerializeField, Multiline]
    private string text = string.Empty;
    [SerializeField]
    private ChatControl chatControl = null;
    [SerializeField]
    private ActionWithInput playerInput = null;
    [SerializeField, Tooltip("このコンポーネントが付いているオブジェクトからの半径距離で会話出来るかを制御")]
    private float radius = 2f;

    public void Update()
    {
      //会話システム起動
      //プレイヤーの位置とかで条件追加する
      if(Input.GetButtonDown("Chat"))

        //会話開始
        if(!chatControl.IsChatting && !playerInput.IsJumpping && playerInput.IsGrounded)
        {
          if(Physics.OverlapSphere(transform.position, radius).FirstOrDefault(c => c.tag == "Player") != null)
            chatControl.StartChat(charactorName, text);
        }

        //会話ボタンの次の動作
        else if(chatControl.IsChatting)
        {
          chatControl.ChatNext();
        }
    }
  }
}
