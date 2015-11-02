using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class SwitchEventCaller : MonoBehaviour
{

  public event Action switchOnAction;
  public event Action switchOffAction;
  [SerializeField,Tooltip("イベントによってカメラの位置を変更する場合、カメラを設定するポジション")]
  Transform camPos = null;
  [SerializeField]
  AutoCam cam = null;
  Transform defaultTarget;

  public void Start()
  {
    defaultTarget = cam.Target;
  }




  public void OnAction()
  {
    ChangeCameraTarget(camPos);
    if(switchOnAction != null)
      switchOnAction();
    Invoke("SetCameraTargetDefault", 3.5f);
  }

  public void OffAction()
  {
    if(switchOffAction != null)
      switchOffAction();
  }

  void ChangeCameraTarget(Transform target)
  {
    if(target!=null)
      cam.SetTarget(target);
  }

  void SetCameraTargetDefault()
  {
    ChangeCameraTarget(defaultTarget);
  }

}
