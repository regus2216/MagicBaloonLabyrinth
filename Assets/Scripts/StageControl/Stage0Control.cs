using MBL.Charactor;
using MBL.Charactor.Player;
using MBL.Charactor.Speaker;
using MBL.UI.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace MBL.StageControl
{
  public class Stage0Control : MonoBehaviour
  {
    [SerializeField, Tooltip("最初のメッセージ")]
    private Speak startMessage = null;
    [SerializeField, Tooltip("最初のアニメーション")]
    private Animator startAnim = null;

    public void Start()
    {
      startAnim.SetTrigger("Start");
      Invoke("StartMessage", 3f);
    }

    private void StartMessage()
    {
      startMessage.StartChat(Direction.None);
    }

    public void OnTriggerEnter(Collider other)
    {
      if(other.tag == "Player")
      {
        FadeManager.Instance.LoadLevel("STAGE1", 3f);
      }
    }
  }
}
