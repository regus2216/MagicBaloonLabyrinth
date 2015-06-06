using System.Collections;
using UnityEngine;

public class TakeBase : MonoBehaviour
{
  [SerializeField]
  private Transform takedPos = null;

  private bool isTaked;

  public virtual void Taked()
  {
    isTaked = true;
  }

  public virtual void Releace()
  {
    isTaked = false;
  }

  public void Update()
  {
    if(isTaked)
      transform.position = takedPos.position;
  }
}
