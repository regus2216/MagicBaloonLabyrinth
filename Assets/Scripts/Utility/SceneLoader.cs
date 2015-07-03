using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  public class SceneLoader : MonoBehaviour
  {
    [SerializeField]
    private float fadeTime = 3f;

    public void LoadScene(string sceneName)
    {
      FadeManager.Instance.LoadLevel(sceneName, fadeTime);
    }

    public void LoadThisScene()
    {
      FadeManager.Instance.LoadLevel(Application.loadedLevelName, fadeTime);
    }
  }
}
