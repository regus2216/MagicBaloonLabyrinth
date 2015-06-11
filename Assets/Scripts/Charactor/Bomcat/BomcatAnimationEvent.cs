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

    private void Destroy()
    {
      bomcat.Releace();
      Destroy(bomcat.gameObject);
    }
  }
}
