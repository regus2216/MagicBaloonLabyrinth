using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkAround : MonoBehaviour
{
  public enum WalkDirection
  {
    Vertical,
    Horizontal
  }

  [Tooltip("歩行方向")]
  public WalkDirection dir;
  public float speed = 1f;
  public float setWalkTime = 3f;


  private Transform moveObject;
  private bool walking;
  private bool @switch;

  private Vector3 vertical = new Vector3(0, 0, 1);
  private Vector3 horizontal = new Vector3(1, 0, 0);
  private Animator anim;

  public void Update()
  {
    if(walking)
      Walk(dir);
  }

  public void Awake()
  {
    moveObject = transform.parent;
    anim = GetComponent<Animator>();
  }

  public void Start()
  {
    InvokeRepeating("SetWalkTimer", 0, setWalkTime);
  }

  private void SetWalkTimer()
  {
    anim.SetBool("Walk", true);
  }

  public void Switch()
  {
    @switch = !@switch;
  }

  public void StopWalk()
  {
    anim.SetBool("Walk", false);
    walking = false;
  }

  public void StartWalk()
  {
    anim.SetBool("Walk", true);
    walking = true;
  }

  private void Walk(WalkDirection dir)
  {
    switch(dir)
    {
      case WalkDirection.Vertical:
        if(@switch)
          moveObject.Translate(vertical * speed * Time.deltaTime);
        else
          moveObject.Translate(-vertical * speed * Time.deltaTime);
        break;

      case WalkDirection.Horizontal:
        if(@switch)
          moveObject.Translate(horizontal * speed * Time.deltaTime);
        else
          moveObject.Translate(-horizontal * speed * Time.deltaTime);
        break;

      default:
        break;
    }
  }
}
