using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
  public void Start()
  {
    var movie = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
    movie.loop = true;
    movie.Play();
  }
}
