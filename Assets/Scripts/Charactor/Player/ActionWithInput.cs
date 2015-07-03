using MBL.Balloon;
using MBL.UI.Chat;
using MBL.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace MBL.Charactor.Player
{
  /// <summary>
  /// 入力に応じて移動する
  /// </summary>
  [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
  public class ActionWithInput : MonoBehaviour
  {
    [SerializeField, Tooltip("Actionボタン以外の操作を禁止する")]
    private bool allowInput = true;
    [SerializeField, Range(1, 10)]
    private float moveSpeed = 1f;
    [SerializeField, Range(0, 1), Tooltip("しゃがみ移動時のmoveSpeedの係数")]
    private float squatSpeedRate = 0.5f;
    [SerializeField, Tooltip("スプライトのアニメーションをコントロールするアニメーター")]
    private Animator anim = default(Animator);
    [SerializeField, Tooltip("土埃アニメーションを管理するアニメーター")]
    private Animator dust_anim = default(Animator);
    [SerializeField, Tooltip("接地判定オブジェクト")]
    private Transform[] groundCheckPositions = null;
    [SerializeField, Tooltip("頭上判定オブジェクト")]
    private Transform[] headCheckPositions = null;
    [SerializeField, Tooltip("回転可能判定オブジェクト")]
    private Transform[] rotateCheckPositions = null;
    [SerializeField, Tooltip("しゃがみ時回転可能判定オブジェクト")]
    private Transform[] squatRotateCheckPositions = null;
    [SerializeField, Range(0, 100)]
    private float jumpSpeed = 10f;
    [SerializeField, Range(0, 2), Tooltip("上昇時と下降時に速度を一定とする時間(頂点期は含まれない)")]
    private float jumpTimeUpDown = 0.2f;
    [SerializeField, Range(0, 100), Tooltip("ジャンプ時に頂点付近で序々落としていく速度の大きさ")]
    private float jumpTopSlowly = 30f;
    [SerializeField, Range(0, 1), Tooltip("風船を持っている時のjumpTopSlowlyの係数")]
    private float balloonJumpTopSlowly = 0.3f;
    [SerializeField, Range(0, 1), Tooltip("壁めり込み判定ジャンプ等使用しようとした場合にバグ利用とみなしてジャンプを禁止する時間")]
    private float jumpBanTime = 0.5f;
    [SerializeField, Tooltip("立ち状態等でのコライダーのセンター座標")]
    private Vector3 normalColliderCenter = default(Vector3);
    [SerializeField, Tooltip("立ち状態でのコライダーのサイズ")]
    private Vector3 normalColliderSize = default(Vector3);
    [SerializeField, Tooltip("しゃがみ状態でのコライダーのセンター座標")]
    private Vector3 squatColliderCenter = default(Vector3);
    [SerializeField, Tooltip("しゃがみ状態でのコライダーのサイズ")]
    private Vector3 squatColliderSize = default(Vector3);
    [SerializeField, Tooltip("風船")]
    private GameObject balloonPrefab = null;
    [SerializeField, Tooltip("手に持っている時風船の位置を決めるオブジェクト")]
    private Transform balloonPos = null;
    [SerializeField, Tooltip("風船セットイベントを管理するスクリプトをもつオブジェクト")]
    private GameObject eventScript = null;
    [SerializeField, Tooltip("風船を設置する位置を決めるオブジェクト")]
    private Transform setBalloonPos = null;
    [SerializeField, Range(0, 3), Tooltip("風船を設置する際に調べる範囲の大きさ")]
    private float setBalloonRadius = 0.3f;
    [SerializeField, Tooltip("風船をギミックにセットする際に入力を禁止する時間")]
    private float setBanTime = 0.5f;
    [SerializeField]
    private ChatControl chatControl = null;
    [SerializeField, Tooltip("このコンポーネントが付いているオブジェクトからの半径距離で会話出来るかを制御")]
    private float chatRadius = 0.5f;
    [SerializeField, Tooltip("持ち動作の位置")]
    private Transform takePos = null;
    [SerializeField, Tooltip("持ち動作の半径")]
    private float takeRadius = 0.3f;

    public Direction Direction { get; private set; }

    private Vector3 mover = new Vector3();
    private bool isJumpping = false;
    private bool isRotate = false;
    private bool isSquatting = false;
    private bool takeBalloon = false;

    private int verticalInput;
    private int horizontalInput;
    private bool squatInput;
    private bool balloonInput;

    //ジャンプ処理が終了しているかどうか
    private bool jumpEndFlag = true;

    //現在手に持っているバルーン
    private GameObject takeBalloonObject;

    //現在手に持っているオブジェクト
    private TakeBase takeObject;

    public bool IsAllowInput { get { return allowInput; } }

    private BalloonEventCaller bal_eve_cache;
    private BalloonEventCaller BalloonEvent
    {
      get
      {
        if(bal_eve_cache == null)
          bal_eve_cache = eventScript.GetComponent<BalloonEventCaller>();
        return bal_eve_cache;
      }
    }

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
    public bool IsGrounded
    {
      get
      {
        return
          CheckCollisionsAny(groundCheckPositions,
          colls => colls.FirstOrDefault() != null);
      }
    }

    public bool IsJumpping
    {
      get { return isJumpping; }
    }

    /// <summary>
    /// 頭上にコライダーオブジェクトが存在するか
    /// </summary>
    public bool ExistColliderOverhead
    {
      get
      {
        return

          CheckCollisionsAny(headCheckPositions,
          colls => colls.FirstOrDefault() != null);
      }
    }

    /// <summary>
    /// 回転可能かどうか
    /// </summary>
    public bool Rotateble
    {
      get
      {
        if(isRotate || isJumpping)
          return false;
        if(!squatInput)
          return

            //!CheckCollisions(rotateCheckPositions,
            //colls => colls.FirstOrDefault(coll => !coll.isTrigger) != null);
            CheckCollisionCount(rotateCheckPositions) <= Math.Ceiling(rotateCheckPositions.Length / 2f);
        else
          return

            //!CheckCollisions(squatRotateCheckPositions,
            //colls => colls.FirstOrDefault(coll => !coll.isTrigger) != null);
            CheckCollisionCount(squatRotateCheckPositions) <= Math.Ceiling(squatRotateCheckPositions.Length / 2f);
      }
    }

    private AutoCam cam_cache;
    private AutoCam AutoCamera
    {
      get
      {
        if(cam_cache == null)
          cam_cache = Camera.main.GetComponent<AutoCam>();
        return cam_cache;
      }
    }

    /// <summary>
    /// 指定した位置にあるオブジェクト群が
    /// 自身以外の各コライダーに対して各点が1つでも条件を満たしている場合はtrueを返す
    /// </summary>
    /// <param name="predicate">各点取得したコライダーの配列に対して行う真偽値判定</param>
    private bool CheckCollisionsAny(Transform[] positions, Func<IEnumerable<Collider>, bool> predicate)
    {
      List<Collider[]> collsList = new List<Collider[]>();
      foreach(var pos in positions)
        collsList.Add(Physics.OverlapSphere(pos.position, 0.01f));

      foreach(var colls in collsList)
      {
        var colls_player_delete = colls.Where(coll => coll.tag != "Player");
        if(predicate(colls_player_delete))
          return true;
      }
      return false;
    }

    /// <summary>
    /// 各点が衝突しているコライダー(プレイヤー除く)の総数を返す
    /// 同じコライダーに衝突している2点がある場合2つともカウントされる
    /// </summary>
    private int CheckCollisionCount(Transform[] positions)
    {
      List<Collider[]> collsList = new List<Collider[]>();
      foreach(var pos in positions)
        collsList.Add(Physics.OverlapSphere(pos.position, 0.01f));

      int count = 0;
      foreach(var colls in collsList)
      {
        var colls_player_delete = colls.Where(coll => coll.tag != "Player" && coll.isTrigger == false);
        count += colls_player_delete.Count();
      }
      return count;
    }

    public void AllowInput()
    {
      allowInput = true;
    }

    public void DisallowInput()
    {
      anim.SetBool("WalkInput", false);
      dust_anim.SetBool("Dust", false);
      allowInput = false;
    }

    public void Update()
    {
      if(allowInput)
      {
        Move();

        Jump();

        Rotate();
      }

      ActionInput();
      FixedVelocity();
      FlyGravity();
    }

    private void ActionInput()
    {
      if(anim.GetBool("TakeBalloonInput"))
        return;

      if(Input.GetButtonDown("Action"))
      {
        #region 持ち動作

        //何も持っていない状態なら、TakeInputをfalseにする
        if(!takeObject)
          anim.SetBool("TakeInput", false);

        //何かを持っている状態なら、リリース処理
        if(anim.GetBool("TakeInput") && takeObject)
        {
          takeObject.Releace();
          anim.SetBool("TakeInput", false);
          return;
        }

        var take = Physics.OverlapSphere(takePos.position, takeRadius)
          .FirstOrDefault(c => c.GetComponent<TakeBase>());

        //持ちが可能なら持ち上げる
        if(take)
        {
          takeObject = take.GetComponent<TakeBase>();
          takeObject.Taked();
          anim.SetBool("TakeInput", true);
          return;
        }
        #endregion 持ち動作

        #region 会話処理

        //会話中なら、次の会話
        if(chatControl.IsChatting)
        {
          chatControl.ChatNext();
          return;
        }
        var speaker = Physics.OverlapSphere(takePos.position, chatRadius)
          .FirstOrDefault(c => c.GetComponent<Speaker.Speak>());

        //会話可能ならば会話する
        if(speaker && !anim.GetBool("TakeInput"))
        {
          speaker.GetComponent<Speaker.Speak>().StartChat(Direction);
          return;
        }
        #endregion 会話処理
      }
    }

    private void Move()
    {
      //しゃがみ状態の検出
      if(!isJumpping && !takeBalloon)
        squatInput = Input.GetButton("Squat");

      //しゃがんでいるかつ頭上にコライダーが存在する場合は強制でしゃがみが押された状態にする(立てない状態)
      if(anim.GetBool("SquatInput") && ExistColliderOverhead)
        squatInput = true;
      isSquatting = squatInput;

      //移動状態の検出
      horizontalInput = (int)Input.GetAxisRaw("Horizontal");
      verticalInput = (int)Input.GetAxisRaw("Vertical");
      mover.x = horizontalInput;
      mover.z = verticalInput;

      //向きの更新
      if(horizontalInput > 0)
        Direction = Direction.Right;
      if(horizontalInput < 0)
        Direction = Direction.Left;

      if(!isRotate)
      {
        //普通の移動
        if(!squatInput)
        {
          transform.Translate(moveSpeed * mover.normalized * Time.deltaTime);

          //コライダーの大きさ変更
          Collider.center = normalColliderCenter;
          Collider.size = normalColliderSize;
        }

        //しゃがみ移動
        else
        {
          transform.Translate(moveSpeed * squatSpeedRate * mover.normalized * Time.deltaTime);

          //コライダーの大きさ変更
          Collider.center = squatColliderCenter;
          Collider.size = squatColliderSize;
        }
      }

      //風船入力処理
      if(!isJumpping && !isSquatting && !anim.GetBool("TakeInput"))
        balloonInput = Input.GetButtonDown("Balloon");

      //アニメーション処理
      //Walk
      if(!isJumpping && (verticalInput != 0 || horizontalInput != 0))
        anim.SetBool("WalkInput", true);
      else
        anim.SetBool("WalkInput", false);

      //Squat
      if(squatInput)
        anim.SetBool("SquatInput", true);
      else
        anim.SetBool("SquatInput", false);

      //Dust
      if((verticalInput != 0 || horizontalInput != 0) && !squatInput && !isJumpping)
        dust_anim.SetBool("Dust", true);
      else
        dust_anim.SetBool("Dust", false);

      //Balloon
      if(!squatInput && !isJumpping)
        if(!takeBalloon && balloonInput)
        {
          anim.SetBool("TakeBalloonInput", true);
          takeBalloon = true;
          TakeBalloon();
        }
        else if(takeBalloon && balloonInput)
        {
          SetBalloon();
        }
      if(!takeBalloon)
        anim.SetBool("TakeBalloonInput", false);

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
      if(Input.GetButtonDown("Jump") && jumpEndFlag && !isJumpping && IsGrounded && !isRotate /*&& !anim.GetBool("TakeInput")*/)
        StartCoroutine_Auto(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
      isJumpping = true;
      jumpEndFlag = false;
      Rigidbody.useGravity = false;

      //カメラのY軸移動禁止
      AutoCamera.SetAllowYAxisMove(false);

      //ジャンプ中に壁などに接地した場合trueになる
      bool extraFlag = false;

      //ジャンプアニメーション開始
      if(!squatInput)
        anim.SetTrigger("JumpInput");

      //上がる処理
      Rigidbody.velocity = new Vector3(0, jumpSpeed, 0);

      //しばらく一定速度で上昇
      yield return new WaitForSeconds(jumpTimeUpDown);

      //速度を下げる処理
      while(Rigidbody.velocity.y > -jumpSpeed)
      {
        if(IsGrounded && Rigidbody.velocity.y <= 0)
        {
          extraFlag = true;

          isJumpping = false;
          break;
        }

        if(!takeBalloon)
          Rigidbody.velocity -= Vector3.up * jumpTopSlowly * Time.deltaTime;
        else
          Rigidbody.velocity -= Vector3.up * jumpTopSlowly * balloonJumpTopSlowly * Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }

      //着地まで下降
      while(!IsGrounded)
      {
        if(!takeBalloon)
          Rigidbody.velocity -= Vector3.up * jumpTopSlowly * Time.deltaTime;
        else
          Rigidbody.velocity -= Vector3.up * jumpTopSlowly * balloonJumpTopSlowly * Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }

      //カメラを戻す
      AutoCamera.SetAllowYAxisMove(true);

      //ジャンプアニメーション終了処理
      if(!squatInput)
        anim.SetTrigger("JumpEnd");

      Rigidbody.useGravity = true;

      //途中でジャンプが中断された(足場に乗れた判定になった)場合すこしの間ジャンプ不可
      if(extraFlag)
        yield return new WaitForSeconds(jumpBanTime);
      isJumpping = false;
      jumpEndFlag = true;
    }

    private void Rotate()
    {
      if(Rotateble)
      {
        if(Input.GetButtonDown("LeftRotate"))
          StartCoroutine_Auto(RotateCoroutine(Vector3.up));
        if(Input.GetButtonDown("RightRotate"))
          StartCoroutine_Auto(RotateCoroutine(-Vector3.up));
      }
    }

    private IEnumerator RotateCoroutine(Vector3 axis)
    {
      isRotate = true;

      //数値が小さいほど早く回転
      const int LOOP_COUNT = 20;
      for(int i = 0; i < LOOP_COUNT; ++i)
      {
        transform.Rotate(axis * 90 / LOOP_COUNT);
        yield return new WaitForEndOfFrame();
      }

      isRotate = false;
    }

    /// <summary>
    /// 接地時の速度を直す
    /// </summary>
    private void FixedVelocity()
    {
      if(!isJumpping && IsGrounded && jumpEndFlag)
        Rigidbody.velocity = Vector3.down * jumpSpeed;
    }

    /// <summary>
    /// ジャンプ中以外で空中に存在するときのみかかる重力
    /// 風船を持っている場合は呼ばれない
    /// </summary>
    private void FlyGravity()
    {
      //空中での重力をなくす
      if(!IsGrounded)
        Rigidbody.useGravity = false;
      else if(!isJumpping)
        Rigidbody.useGravity = true;

      //ジャンプ以外でただ落っこちる場合は重力を手動でかける
      if(!isJumpping && !IsGrounded)
      {
        Rigidbody.useGravity = false;
        if(!takeBalloon)
          Rigidbody.velocity += Vector3.up * Physics.gravity.y * Time.deltaTime;
        else
          Rigidbody.velocity += Vector3.up * Physics.gravity.y * balloonJumpTopSlowly * Time.deltaTime;
      }
    }

    /// <summary>
    /// 風船を手に取り出すときの処理
    /// </summary>
    private void TakeBalloon()
    {
      takeBalloonObject = GameObject.Instantiate(balloonPrefab, balloonPos.position, Quaternion.identity) as GameObject;

      //balloonPosを風船に登録して手に持ってる間その位置にいくようにする
      takeBalloonObject.GetComponent<BalloonControl>().SetTraceTarget(balloonPos);
    }

    /// <summary>
    /// 風船をセットするときの挙動
    /// </summary>
    private void SetBalloon()
    {
      //風船をセットすべき場所を調べる(IBalloonEventを継承するスクリプトがついたコライダーオブジェクトが存在するか調べる)
      var coll = Physics.OverlapSphere(setBalloonPos.position, setBalloonRadius)
        .Where(c => c.GetComponent<IBalloonEvent>() != null).FirstOrDefault();

      takeBalloon = false;

      //もしセットすべき場所でない場合は風船を手放す
      if(coll == null)
      {
        //手放す
        takeBalloonObject.GetComponent<BalloonControl>().SetFlow();
        anim.SetBool("TakeBalloonInput", false);
      }

      //もしセット出来るならばIBalloonEventのメソッドを呼び出しアニメーションする
      else
      {
        //設置場所が設置できないものと判断した場合、風船を手放して終了
        if(!coll.GetComponent<IBalloonEvent>().WhetherPossibleSet())
        {
          takeBalloonObject.GetComponent<BalloonControl>().SetFlow();
          anim.SetBool("TakeBalloonInput", false);
          return;
        }

        //設置
        takeBalloonObject.GetComponent<BalloonControl>().SetTraceTarget(coll.transform, true);

        //アニメーション
        anim.SetTrigger("SetBalloonInput");

        //アニメーション中の入力禁止
        DisallowInput();
        Invoke("AllowInput", setBanTime);
        anim.SetBool("WalkInput", false);

        //イベント呼び出し
        BalloonEvent.SetBalloonEvent(coll.GetComponent<IBalloonEvent>());
      }
    }

    //public void OnGUI()
    //{
    //  GUILayout.Label("Pos : " + transform.position);
    //  GUILayout.Label("Rot : " + transform.rotation.eulerAngles);

    //  GUILayout.Label("H : " + horizontalInput);
    //  GUILayout.Label("V : " + verticalInput);
    //  GUILayout.Label("Verocity : " + Rigidbody.velocity);
    //  GUILayout.Label("UseGravity : " + Rigidbody.useGravity);

    //  GUILayout.Label("IsGrounded : " + IsGrounded);
    //  GUILayout.Label("ExistColliderOverhead : " + ExistColliderOverhead);
    //  GUILayout.Label("Rotateble : " + Rotateble);

    //  GUILayout.Label("Jumpping : " + isJumpping);
    //  GUILayout.Label("TakeBalloon : " + takeBalloon);

    //GUILayout.Label("PlayerDir : " + Direction);
    //}
  }
}
