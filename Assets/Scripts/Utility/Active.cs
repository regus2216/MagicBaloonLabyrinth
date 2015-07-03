using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  public class Active : MonoBehaviour
  {
    [SerializeField]
    private Behaviour behaviour = null;

    public void EnableActive()
    {
      behaviour.enabled = true;
    }

    public void DisableActive()
    {
      behaviour.enabled = false;
    }
  }
}
