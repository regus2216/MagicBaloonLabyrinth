using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 入力に応じて移動する
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class MoveWithInput : MonoBehaviour
{
  [SerializeField]
  private float moveSpeed = 1f;
  [SerializeField, Tooltip("上下キーでのZ軸方向移動を許可するかどうか")]
  private bool allowZAxis = true;
  [SerializeField, Tooltip("入力キーに応じてスプライトを反転させるのに使用")]
  private GameObject playerSprite;
  [SerializeField, Tooltip("スプライトのアニメーションをコントロールするアニメーター")]
  private Animator anim;
  [SerializeField, Tooltip("接地判定オブジェクト")]
  private Transform groundCheck;
  [SerializeField, Range(0, 100)]
  private float jumpSpeed = 10f;
  [SerializeField, Range(0, 2), Tooltip("上昇時と下降時に速度を一定とする時間(頂点期は含まれない)")]
  private float jumpTimeUpDown = 0.2f;
  [SerializeField, Range(0, 10), Tooltip("ジャンプ時に頂点で序々落としていく速度の大きさ")]
  private float jumpTopSlowly = 1f;

  //ジャンプしているかどうか
  private bool isJumpping = false;
  private int verticalInput;
  private int horizontalInput;

  private Rigidbody rig_cache;
  private Rigidbody Rigidbody
  {
    get
    {
      if(rig_cache == null)
        rig_cache = GetComponent<Rigidbody>();
      return rig_cache;
    }
  }

  private bool IsGround
  {
    get
    {
      var colls = Physics.OverlapSphere(groundCheck.position, 0.1f);

      //CanRideタグが付いているもののみ乗れる
      if(colls.Any(col => col.tag == "CanRide"))
        return true;
      else
        return false;
    }
  }

  public void Update()
  {
    Move();

    Jump();

    Rotate();
  }

  private void Move()
  {
    //移動
    verticalInput = (int)Input.GetAxisRaw("Vertical");
    horizontalInput = (int)Input.GetAxisRaw("Horizontal");
    if(allowZAxis)
      transform.Translate(moveSpeed * Vector3.forward * verticalInput * Time.deltaTime);
    transform.Translate(moveSpeed * Vector3.right * horizontalInput * Time.deltaTime);

    //アニメーション処理
    if(!isJumpping && ((int)verticalInput != 0 || (int)horizontalInput != 0))
      anim.SetBool("WalkInput", true);
    else
      anim.SetBool("WalkInput", false);

    //反転処理
    var scale = transform.localScale;
    if(scale.x > 0 && horizontalInput > 0)
      scale.x = -1;
    if(scale.x < 0 && horizontalInput < 0)
      scale.x = 1;
    transform.localScale = scale;
  }

  private void Jump()
  {
    //ジャンプ処理
    if(Input.GetButtonDown("Jump"))
      StartCoroutine_Auto(JumpCoroutine());
  }

  private IEnumerator JumpCoroutine()
  {
    if(isJumpping)
      yield break;

    isJumpping = true;
    Rigidbody.useGravity = false;

    //ジャンプアニメーション開始
    anim.SetTrigger("JumpInput");

    //上がる処理
    Rigidbody.velocity = new Vector2(0, jumpSpeed);

    //しばらく一定速度で上昇
    yield return new WaitForSeconds(jumpTimeUpDown);

    //速度を下げる処理
    while(Rigidbody.velocity.y > -jumpSpeed)
    {
      if(IsGround)
        break;

      Rigidbody.velocity -= Vector3.up * jumpTopSlowly;
      yield return new WaitForEndOfFrame();
    }

    //ジャンプアニメーション終了処理
    anim.SetTrigger("JumpEnd");

    //しばらく一定速度で下降
    if(!IsGround)
      yield return new WaitForSeconds(jumpTimeUpDown);

    Rigidbody.useGravity = true;
    isJumpping = false;
  }

  private void Rotate()
  {
    //回転
    if(Input.GetButtonDown("LeftRotate"))
      transform.Rotate(new Vector3(0, 90, 0));
    if(Input.GetButtonDown("RightRotate"))
      transform.Rotate(new Vector3(0, -90, 0));
  }

  public void OnGUI()
  {
    GUILayout.Label("H:" + horizontalInput);
    GUILayout.Label("V:" + verticalInput);
    GUILayout.Label("IsGround:" + IsGround);
    GUILayout.Label("GroundCheckPos:" + groundCheck.position);
  }
}
