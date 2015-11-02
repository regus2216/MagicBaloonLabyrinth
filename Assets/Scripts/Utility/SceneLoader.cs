using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MBL.Utility
{
  public class SceneLoader : MonoBehaviour
  {
    private const float fadeTime = 1f;
    readonly string saveFilePath;

    public SceneLoader()
    {
      saveFilePath = System.IO.Directory.GetCurrentDirectory();
    }


    public void LoadScene(string sceneName)
    {
      FadeManager.Instance.LoadLevel(sceneName, fadeTime);
    }

    public void LoadThisScene()
    {
      FadeManager.Instance.LoadLevel(Application.loadedLevelName, fadeTime);
    }

    public void LoadSceneWithSaveFile()
    {
      LoadScene(SaveFileManager.Load(saveFilePath));
    }

    public void SaveSceneToSaveFile()
    {
      SaveFileManager.Save(saveFilePath, Application.loadedLevelName);
    }

    //public void OnGUI()
    //{
    //  GUILayout.Label(saveFilePath);
    //  if(GUILayout.Button("Load"))
    //  {
    //    LoadSceneWithSaveFile();
    //  }

    //  if(GUILayout.Button("Save"))
    //  {
    //    SaveSceneToSaveFile();
    //  }
    //}
  }
}
