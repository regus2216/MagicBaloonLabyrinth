using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 入力に応じて移動する
/// </summary>
public class MoveWithInput : MonoBehaviour
{
  [SerializeField]
  private float moveSpeed = 1f;
  [SerializeField, Tooltip("上下キーでのZ軸方向移動を許可するかどうか")]
  private bool allowZAxis = true;
  [SerializeField, Tooltip("入力キーに応じてスプライトを反転させるのに使用")]
  private GameObject playerSprite;

  public void Update()
  {
    //移動
    if(allowZAxis)
      transform.Translate(moveSpeed * Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime);
    transform.Translate(moveSpeed * Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime);

    //反転処理
    InverseSprite();

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
}
