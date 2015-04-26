using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 入力に応じて移動する
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class ActionWithInput : MonoBehaviour
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
  [SerializeField, Tooltip("接地判定オブジェクト")]
  private Transform[] groundCheckPositions;
  [SerializeField, Tooltip("しゃがみ判定オブジェクト")]
  private Transform[] squatCheckPositions;
  [SerializeField, Tooltip("回転可能判定オブジェクト")]
  private Transform[] rotateCheckPositions;
  [SerializeField, Tooltip("しゃがみ時回転可能判定オブジェクト")]
  private Transform[] squatRotateCheckPositions;
  [SerializeField, Range(0, 100)]
  private float jumpSpeed = 10f;
  [SerializeField, Range(0, 2), Tooltip("上昇時と下降時に速度を一定とする時間(頂点期は含まれない)")]
  private float jumpTimeUpDown = 0.2f;
  [SerializeField, Range(0, 10), Tooltip("ジャンプ時に頂点で序々落としていく速度の大きさ")]
  private float jumpTopSlowly = 1f;
  [SerializeField, Range(0, 1), Tooltip("壁めり込み判定ジャンプ等使用しようとした場合にバグ利用とみなしてジャンプを禁止する時間")]
  private float jumpBanTime = 0.5f;
  [SerializeField, Tooltip("立ち状態等でのコライダーのセンター座標")]
  private Vector3 normalColliderCenter;
  [SerializeField, Tooltip("立ち状態でのコライダーのサイズ")]
  private Vector3 normalColliderSize;
  [SerializeField, Tooltip("しゃがみ状態でのコライダーのセンター座標")]
  private Vector3 squatColliderCenter;
  [SerializeField, Tooltip("しゃがみ状態でのコライダーのサイズ")]
  private Vector3 squatColliderSize;

  private bool isJumpping = false;
  private bool isRotate = false;

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

  private BoxCollider col_cache;
  private BoxCollider Collider
  {
    get
    {
      if(col_cache == null)
        col_cache = GetComponent<BoxCollider>();
      return col_cache;
    }
  }

  /// <summary>
  /// 接地しているか
  /// </summary>
  private bool IsGround
  {
    get
    { return CheckCollisions(groundCheckPositions); }
  }

  /// <summary>
  /// 頭上にコライダーオブジェクトが存在するか
  /// </summary>
  private bool ExistColliderOverhead
  {
    get { return CheckCollisions(squatCheckPositions); }
  }

  /// <summary>
  /// 回転可能かどうか
  /// </summary>
  private bool Rotateble
  {
    get
    {
      if(!squatInput)
        return !CheckCollisions(rotateCheckPositions);
      else
        return !CheckCollisions(squatRotateCheckPositions);
    }
  }

  /// <summary>
  /// 指定した位置にあるオブジェクト群が
  /// 自身以外のコライダーに接触している場合はtrueを返す
  /// </summary>
  private bool CheckCollisions(Transform[] positions)
  {
    List<Collider[]> collsList = new List<Collider[]>();
    foreach(var pos in positions)
      collsList.Add(Physics.OverlapSphere(pos.position, 0.001f));

    foreach(var colls in collsList)
      if(colls.Any(coll => coll.tag != "Player"))
        return true;
    return false;
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

    //前にしゃがんでいてかつ頭上にコライダーが存在する場合は強制でしゃがみが押された状態にする
    if(anim.GetBool("SquatInput") && ExistColliderOverhead)
      squatInput = true;

    //移動状態の検出
    verticalInput = (int)Input.GetAxisRaw("Vertical");
    horizontalInput = (int)Input.GetAxisRaw("Horizontal");

    //普通の移動
    if(!squatInput)
    {
      if(allowZAxis)
        transform.Translate(moveSpeed * Vector3.forward * verticalInput * Time.deltaTime);
      transform.Translate(moveSpeed * Vector3.right * horizontalInput * Time.deltaTime);

      //コライダーの大きさ変更
      Collider.center = normalColliderCenter;
      Collider.size = normalColliderSize;
    }

    //しゃがみ移動
    else
    {
      if(allowZAxis)
        transform.Translate(moveSpeed * squatSpeedRate * Vector3.forward * verticalInput * Time.deltaTime);
      transform.Translate(moveSpeed * squatSpeedRate * Vector3.right * horizontalInput * Time.deltaTime);

      //コライダーの大きさ変更
      Collider.center = squatColliderCenter;
      Collider.size = squatColliderSize;
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

    //ジャンプで速度が一定以下になる前に接地した場合trueになる
    bool extraFlag = false;

    //ジャンプアニメーション開始
    if(!squatInput)
      anim.SetTrigger("JumpInput");

    //上がる処理
    Rigidbody.velocity = new Vector2(0, jumpSpeed);

    //しばらく一定速度で上昇
    yield return new WaitForSeconds(jumpTimeUpDown);

    //速度を下げる処理
    while(Rigidbody.velocity.y > -jumpSpeed)
    {
      if(IsGround)
      {
        extraFlag = true;
        break;
      }

      Rigidbody.velocity -= Vector3.up * jumpTopSlowly;
      yield return new WaitForEndOfFrame();
    }

    //着地まで一定速度で下降
    while(!IsGround)
      yield return new WaitForEndOfFrame();

    //ジャンプアニメーション終了処理
    if(!squatInput)
      anim.SetTrigger("JumpEnd");

    Rigidbody.useGravity = true;

    //途中でジャンプが中断された(足場に乗れた判定になった)場合すこしの間ジャンプ不可
    if(extraFlag)
      yield return new WaitForSeconds(jumpBanTime);
    isJumpping = false;
  }

  private void Rotate()
  {
    if(!isRotate && !isJumpping && Rotateble)
    {
      if(Input.GetButtonDown("LeftRotate"))
        StartCoroutine_Auto(RotateCoroutine(Vector3.up));
      if(Input.GetButtonDown("RightRotate"))
        StartCoroutine_Auto(RotateCoroutine(-Vector3.up));
    }
  }

  private IEnumerator RotateCoroutine(Vector3 eulerAngle)
  {
    isRotate = true;
    for(int i = 0; i < 15; ++i)
    {
      transform.Rotate(eulerAngle * 6);
      yield return new WaitForEndOfFrame();
    }
    isRotate = false;
  }

  public void OnGUI()
  {
    GUILayout.Label("H:" + horizontalInput);
    GUILayout.Label("V:" + verticalInput);
    GUILayout.Label("IsGround:" + IsGround);
    GUILayout.Label("ExistColliderOverhead:" + ExistColliderOverhead);
    GUILayout.Label("Rotateble:" + Rotateble);

    GUILayout.Label("IsJumpping:" + isJumpping);
  }
}
