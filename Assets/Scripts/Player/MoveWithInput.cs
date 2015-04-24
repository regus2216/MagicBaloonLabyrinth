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
  [SerializeField, Range(0, 100)]
  private float jumpSpeed = 10f;
  [SerializeField, Range(0, 2), Tooltip("上昇時と下降時に速度を一定とする時間(頂点期は含まれない)")]
  private float jumpTimeUpDown = 0.2f;
  [SerializeField, Range(0, 10), Tooltip("ジャンプ時に頂点で序々落としていく速度の大きさ")]
  private float jumpTopSlowly = 1f;

  private bool isJumpping = false;

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

  public void Update()
  {
    //移動
    if(allowZAxis)
      transform.Translate(moveSpeed * Vector3.forward * Input.GetAxisRaw("Vertical") * Time.deltaTime);
    transform.Translate(moveSpeed * Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime);

    //反転処理
    InverseSprite();

    //ジャンプ処理
    if(Input.GetButtonDown("Jump"))
    {
      StartCoroutine_Auto(Jump());
    }

    //回転
    if(Input.GetButtonDown("LeftRotate"))
      transform.Rotate(new Vector3(0, 90, 0));
    if(Input.GetButtonDown("RightRotate"))
      transform.Rotate(new Vector3(0, -90, 0));
  }

  /// <summary>
  /// 入力からスプライトの方向を決定し、反転する
  /// </summary>
  private void InverseSprite()
  {
    var scale = transform.localScale;
    if(scale.x > 0 && Input.GetAxis("Horizontal") > 0)
      scale.x = -1;
    if(scale.x < 0 && Input.GetAxis("Horizontal") < 0)
      scale.x = 1;
    transform.localScale = scale;
  }

  private IEnumerator Jump()
  {
    if(isJumpping)
      yield break;

    isJumpping = true;

    Rigidbody.useGravity = false;

    //上がる処理
    Rigidbody.velocity = new Vector2(0, jumpSpeed);

    //しばらく一定速度で上昇
    yield return new WaitForSeconds(jumpTimeUpDown);

    //速度を下げる処理
    while(Rigidbody.velocity.y > -jumpSpeed)
    {
      Rigidbody.velocity -= Vector3.up * jumpTopSlowly;
      yield return new WaitForEndOfFrame();
    }

    //しばらく一定速度で下降
    yield return new WaitForSeconds(jumpTimeUpDown);

    Rigidbody.useGravity = true;
    isJumpping = false;
  }

  public void OnGUI()
  {
    GUILayout.Label(Rigidbody.velocity.ToString());
  }
}
