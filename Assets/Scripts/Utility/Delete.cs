using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  public class Delete : MonoBehaviour
  {
    public void DeleteAnimator()
    {
      Destroy(GetComponent<Animator>());
    }
  }
}
