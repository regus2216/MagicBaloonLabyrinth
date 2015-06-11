using MBL.Charactor;
using MBL.Charactor.Player;
using MBL.Gimmick.Explor;
using MBL.Utility;
using System.Collections;
using UnityEngine;

namespace MBL.Charactor.Bomcat
{
  public class Bomcat : TakeBase
  {
    [SerializeField]
    private Transform player = null;
    [SerializeField]
    private float throwSpeed = 3f;
    [SerializeField, Tooltip("BomcatのAnimatorComponent")]
    private Animator anim = null;
    [SerializeField]
    private Fall fallScript = null;
    private ActionWithInput playerScript;
    private bool @throw;
    private Vector3 dir;

    public override void Taked()
    {
      base.Taked();
      playerScript = player.GetComponent<ActionWithInput>();
      fallScript.enabled = false;
      anim.SetTrigger("Explor");
    }

    public override void Releace()
    {
      base.Releace();
      fallScript.enabled = true;
      @throw = true;
      dir = playerScript.Direction == Direction.Left ? Vector3.left : Vector3.right;
    }

    public override void Update()
    {
      base.Update();

      //投げ動作
      if(@throw)
      {
        transform.Translate(dir * throwSpeed * Time.deltaTime, player);

        //着地
        if(fallScript.IsGround)
          @throw = false;
      }
    }

    public void OnCollisionEnter(Collision collision)
    {
      Explor explor = collision.gameObject.GetComponent<Explor>();
      if(@throw && explor)
      {
        explor.DoExplor();
        anim.SetTrigger("ForceExplor");
      }
    }
  }
}
