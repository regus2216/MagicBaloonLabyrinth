using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Charactor.Bomcat
{
  public class BomcatAnimationEvent : MonoBehaviour
  {
    [SerializeField]
    private Bomcat bomcat = null;
    private AudioSource audioSource;

    public void Awake()
    {
      audioSource = GetComponent<AudioSource>();
    }

    public void SoundExplor()
    {
      audioSource.Play();
    }

    private void Destroy()
    {
      bomcat.Releace();
      Destroy(bomcat.gameObject);
    }
  }
}
