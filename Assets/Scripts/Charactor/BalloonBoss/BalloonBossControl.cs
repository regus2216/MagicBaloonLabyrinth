using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MBL.Charactor.Kasabuta;

public class BalloonBossControl : MonoBehaviour
{
  public Animator controlAnim = null;
  public int hp = 5;
  public float rotateSpeed = 3f;
  public Transform target = null;
  public WindZoneControl windCtrl = null;


  public void Update()
  {
    if(!windCtrl.Blowing)
    {
      var diff = (target.position - transform.position).normalized;
      diff.y = 0;
      diff = Vector3.Lerp(transform.forward, diff, Time.deltaTime * rotateSpeed);
      transform.rotation = Quaternion.LookRotation(diff);
    }
  }

  public void OnTriggerEnter(Collider other)
  {
    if(other.tag == "Player")
    {
      --hp;
      if(hp >= 0)
      {
        foreach(var balloon in FindObjectsOfType<MBL.Balloon.BalloonControl>().ToList())
          Destroy(balloon.gameObject);

        controlAnim.enabled = true;
        controlAnim.SetTrigger("Damage");
      }
      if(hp < 0)
      {
        Debug.Log("ボス倒しました");
      }
    }
  }
}
