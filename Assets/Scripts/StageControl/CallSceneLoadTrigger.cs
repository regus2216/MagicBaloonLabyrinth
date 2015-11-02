using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CallSceneLoadTrigger : MonoBehaviour
{
  public string sceneName = string.Empty;
  public void Awake()
  {
    if(!GetComponent<Collider>().isTrigger)
      Debug.LogWarning("Not Trigger.");
  }


  public void OnTriggerEnter(Collider other)
  {
    if(other.tag == "Player")
      FadeManager.Instance.LoadLevel(sceneName, 3f);
  }
}
