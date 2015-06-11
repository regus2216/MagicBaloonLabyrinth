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
    [SerializeField, Tooltip("デフォルトで向いている方向を設定")]
    private Direction dir;

    public void StartChat(Direction playerDir)
    {
      if(playerDir == dir)
      {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        switch(dir)
        {
          case Direction.Left:
            dir = Direction.Right;
            break;

          case Direction.Right:

            dir = Direction.Left;
            break;

          default:
            break;
        }
      }
      chatControl.StartChat(charactorName, text);
    }

    public void ChatNext()
    {
      chatControl.ChatNext();
    }

    public void OnGUI()
    {
      GUILayout.Label("SpeakerDir : " + dir);
    }
  }
}
