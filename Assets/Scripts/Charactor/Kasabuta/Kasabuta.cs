using MBL.Charactor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Charactor.Kasabuta
{
  public class Kasabuta : MonoBehaviour
  {
    [SerializeField, Tooltip("スプライトに取り付けられたAnimator")]
    private Animator anim = null;
    [SerializeField]
    private float repeatTime = 5f;

    public void Start()
    {
      InvokeRepeating("Wind", 0, repeatTime);
    }

    private void Wind()
    {
      anim.SetTrigger("Wind");
    }
  }
}
