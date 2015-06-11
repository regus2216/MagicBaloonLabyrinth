using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Gimmick.Explor
{
  /// <summary>
  /// Animatorの"Explor"トリガーをセットし、指定時間後にゲームオブジェクトを消滅させる
  /// </summary>
  [RequireComponent(typeof(Animator))]
  public class Explor : MonoBehaviour
  {
    [SerializeField, Tooltip("爆発アニメーション開始からゲームオブジェクトを消滅させるまでの時間")]
    private float destroyTime = 1f;
    private Animator anim;

    public void Awake()
    {
      anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 爆発アニメーションを開始し、自動的にゲームオブジェクトを消滅させる
    /// </summary>
    public void DoExplor()
    {
      anim.SetTrigger("Explor");
      Invoke("Dispose", destroyTime);
    }

    private void Dispose()
    {
      Destroy(gameObject);
    }
  }
}
