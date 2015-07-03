using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwitchEventCaller : MonoBehaviour
{

  public event Action switchAction;

  public void DoAction()
  {
    if(switchAction != null)
      switchAction();
  }


}
