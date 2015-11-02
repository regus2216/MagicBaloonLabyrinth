using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatorEnabled : MonoBehaviour
{
  public Animator anim;

  public void AnimatorOff()
  {
    anim.enabled = false;
  }

}
