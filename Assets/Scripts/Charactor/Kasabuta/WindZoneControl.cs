using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Charactor.Kasabuta
{
  public class WindZoneControl : MonoBehaviour
  {
    [SerializeField, Tooltip("風力ベクトル")]
    private Vector3 windPower = new Vector3(-5, 0, 0);

    public bool Blowing { get; set; }

    public void OnTriggerStay(Collider other)
    {
      if(other.tag == "Player" && Blowing)
      {
        other.transform.Translate(windPower * Time.fixedDeltaTime, transform);
      }
    }
  }
}
