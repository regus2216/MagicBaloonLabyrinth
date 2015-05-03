using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Balloon
{
  /// <summary>
  /// 風船システム使用時のオブジェクトの挙動
  /// </summary>
  public interface IBalloonEvent
  {
    /// <summary>
    /// 風船取り付け時に発生するイベント
    /// </summary>
    void EventAction();

    /// <summary>
    /// 取り付けられていた風船をロストした際の挙動
    /// </summary>
    void MissingBalloon();
  }
}
