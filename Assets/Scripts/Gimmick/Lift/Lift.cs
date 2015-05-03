using MBL.Charactor.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Gimmick.Lift
{
  public class Lift : MonoBehaviour
  {
    [SerializeField]
    private Transform liftPos = null;

    public void Update()
    {
      transform.position = liftPos.position;
    }
  }
}
