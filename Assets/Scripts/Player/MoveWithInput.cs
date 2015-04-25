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
  [SerializeField, Range(1, 10)]
  private float moveSpeed = 1f;
  [SerializeField, Range(0, 1), Tooltip("しゃがみ移動時のmoveSpeedの係数")]
  private float squatSpeedRate = 0.5f;
  [SerializeField, Tooltip("上下キーでのZ軸方向移動を許可するかどうか")]
  private bool allowZAxis = true;
  [SerializeField, Tooltip("スプライトのアニメーションをコントロールするアニメーター")]
  private Animator anim = default(Animator);
  [SerializeField, Tooltip("土埃アニメーションを管理するアニメーター")]
  private Animator dust_anim = default(Animator);
  [Tooltip("接地判定オブジェクト")]
  public Transform[] groundCheckPositions;
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
  private bool squatInput;

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
      //接地判定オブジェクト周辺のコライダーの取得
      List<Collider[]> collsList = new List<Collider[]>();
      foreach(var pos in groundCheckPositions)
        collsList.Add(Physics.OverlapSphere(pos.position, 0.01f));

      //タグがPlayer(自身)以外のコライダーにマッチした場合は接地しているとする
      foreach(var colls in collsList)
        if(colls.Any(coll => coll.tag != "Player"))
          return true;
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
    //しゃがみ状態の検出
    if(!isJumpping)
      squatInput = Input.GetButton("Squat");

    //移動状態の検出
    verticalInput = (int)Input.GetAxisRaw("Vertical");
    horizontalInput = (int)Input.GetAxisRaw("Horizontal");

    //普通の移動
    if(!squatInput)
    {
      if(allowZAxis)
        transform.Translate(moveSpeed * Vector3.forward * verticalInput * Time.deltaTime);
      transform.Translate(moveSpeed * Vector3.right * horizontalInput * Time.deltaTime);
    }

    //しゃがみ移動
    else
    {
      if(allowZAxis)
        transform.Translate(moveSpeed * squatSpeedRate * Vector3.forward * verticalInput * Time.deltaTime);
      transform.Translate(moveSpeed * squatSpeedRate * Vector3.right * horizontalInput * Time.deltaTime);
    }

    //アニメーション処理
    if(!isJumpping && (verticalInput != 0 || horizontalInput != 0))
      anim.SetBool("WalkInput", true);
    else
      anim.SetBool("WalkInput", false);
    if(squatInput)
      anim.SetBool("SquatInput", true);
    else
      anim.SetBool("SquatInput", false);
    if((verticalInput != 0 || horizontalInput != 0) && !squatInput && !isJumpping)
      dust_anim.SetBool("Dust", true);
    else
      dust_anim.SetBool("Dust", false);

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
    if(Input.GetButtonDown("Jump") && !isJumpping && IsGround)
      StartCoroutine_Auto(JumpCoroutine());
  }

  private IEnumerator JumpCoroutine()
  {
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
  }
}
