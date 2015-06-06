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

    public void StartChat()
    {
      chatControl.StartChat(charactorName, text);
    }

    public void ChatNext()
    {
      chatControl.ChatNext();
    }
  }
}
