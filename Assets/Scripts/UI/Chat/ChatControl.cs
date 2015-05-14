using MBL.Charactor.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MBL.UI.Chat
{
  /// <summary>
  /// チャットを管理する
  /// </summary>
  public class ChatControl : MonoBehaviour
  {
    [SerializeField]
    private Text textField = null;
    [SerializeField]
    private GameObject renderCanvas = null;
    [SerializeField]
    private ActionWithInput playerInput = null;

    private string[] text;//表示させる文字列
    private int index;//表示する会話の分割ナンバー
    private bool textShowing = false;//表示処理実行中かどうか
    private Coroutine showTextCoroutine = null;

    public bool IsChatting
    {
      get { return renderCanvas.activeInHierarchy; }
    }

    public void Start()
    {
      renderCanvas.SetActive(false);
    }

    /// <summary>
    /// 送られてきた会話文を表示
    /// 空行区切りで表示を分割する
    /// </summary>
    public void StartChat(string text)
    {
      //キャラの入力させなくする
      playerInput.DisallowInput();
      playerInput.enabled = false;

      renderCanvas.SetActive(true);

      //会話文の分割(空行で分割)
      this.text = text.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
      index = 0;
      ChatNext();
    }

    /// <summary>
    /// 会話状態で会話ボタンが押された際の挙動
    /// これ以上表示することがない場合は会話を終了
    /// </summary>
    public void ChatNext()
    {
      //ShowChat中に呼ばれたら、コルーチンを中断して現在表示すべきテキスト全表示
      if(textShowing)
      {
        textShowing = false;
        StopCoroutine(showTextCoroutine);
        textField.text = text[index - 1];
        return;
      }

      //空行区切りで文字を表示
      if(index < text.Length)
        showTextCoroutine = StartCoroutine_Auto(ShowChat(text[index++]));

      //これ以上表示するものがない
      else
        EndChat();
    }

    /// <summary>
    /// 会話終了
    /// </summary>
    private void EndChat()
    {
      //キャラの入力許可
      playerInput.AllowInput();
      playerInput.enabled = true;

      renderCanvas.SetActive(false);
    }

    private IEnumerator ShowChat(string message)
    {
      textShowing = true;
      for(int i = 0; i < message.Length; ++i)
      {
        textField.text = message.Substring(0, i);
        yield return new WaitForSeconds(0.1f);
      }
      textShowing = false;
    }
  }
}
