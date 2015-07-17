using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  /// <summary>
  /// プレハブからオブジェクトを生成し
  /// 生成したオブジェクトが消滅したら再度オブジェクトを生成し直す
  /// </summary>
  public class Phoenix : MonoBehaviour
  {
    [SerializeField, Tooltip("フェニックスオブジェクト")]
    private GameObject phoenixPref = null;

    private GameObject instance;

    public void Start()
    {
      instance = Instantiate(phoenixPref, transform.position, Quaternion.identity) as GameObject;
      instance.SetActive(true);
    }

    public void Update()
    {
      //if(!instance)
      if(instance == null)
      {
        instance = Instantiate(phoenixPref, transform.position, Quaternion.identity) as GameObject;
        instance.SetActive(true);
      }
    }
  }
}
