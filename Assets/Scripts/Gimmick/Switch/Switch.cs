using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Switch : MonoBehaviour
{
  Animator anim = null;
  public Transform playerGroundCheckPos;
  public Transform switchUpperObjectPos;
  public string colliderTag = "Player";
  public bool autoUp = false;

  public void Awake()
  {
    anim = GetComponentInParent<Animator>();
  }

  public void OnCollisionEnter(Collision collision)
  {
    if(collision.collider.tag == colliderTag && playerGroundCheckPos.position.y > switchUpperObjectPos.position.y)
      anim.SetBool("Switch", true);
  }

  public void OnCollisionExit(Collision collision)
  {
    if(autoUp && collision.collider.tag == colliderTag)
      anim.SetBool("Switch", false);
  }


  

}
