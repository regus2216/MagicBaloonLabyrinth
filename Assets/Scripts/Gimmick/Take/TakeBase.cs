using System.Collections;
using UnityEngine;

namespace MBL.Utility
{
  public class TakeBase : MonoBehaviour
  {
    [SerializeField, Tooltip("持ち上げたときの場所(プレイヤーの頭の上のオブジェクトを指定)")]
    private Transform takedPos = null;

    protected bool isTaked;

    /// <summary>
    /// 持ち上げ時の動作
    /// </summary>
    public virtual void Taked()
    {
      isTaked = true;
    }

    /// <summary>
    /// 手放すときの動作
    /// </summary>
    public virtual void Releace()
    {
      isTaked = false;
    }

    /// <summary>
    ///isTakeがtrueのとき、takePosにオブジェクトをセットする
    /// </summary>
    public virtual void Update()
    {
      if(isTaked)
        transform.position = takedPos.position;
    }
  }
}
