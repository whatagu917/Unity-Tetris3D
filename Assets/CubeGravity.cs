using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGravity : MonoBehaviour
{
    public float fallInterval = 0.5f; // ✅ `Tetromino` の落下速度 (秒)
    private float fallTimer;          // ✅ 落下タイマー
    private bool isFalling = true;    // ✅ `Tetromino` が落下中かどうか
    private CubeCollision collision;  // ✅ `CubeCollision` (衝突判定) への参照
    private bool hasLanded = false;   // ✅ `Tetromino` が着地したかどうか

    /// <summary>
    /// ✅ `Tetromino` の初期設定
    /// </summary>
    void Start()
    {
        Debug.Log("CubeGravity: Start() 実行！"); // ✅ `Start()` の確認
        collision = GetComponent<CubeCollision>(); // ✅ `CubeCollision` を取得
        fallTimer = fallInterval; // ✅ `fallTimer` を `fallInterval` に設定
        Invoke(nameof(EnableGroundCheck), 0.2f); // ✅ `EnableGroundCheck()` を 0.2秒後に実行
    }

    /// <summary>
    /// ✅ `Tetromino` の落下を有効化
    /// </summary>
    public void EnableGroundCheck()
    {
        Debug.Log("CubeGravity: EnableGroundCheck() が呼ばれた！"); // ✅ デバッグ確認
        isFalling = true; // ✅ `Tetromino` の落下開始
    }

    /// <summary>
    /// ✅ `Tetromino` がまだ落下中かどうかを取得
    /// </summary>
    public bool IsFalling() => isFalling;

    /// <summary>
    /// ✅ `Tetromino` が着地したかどうかを取得
    /// </summary>
    public bool HasLanded()
{
    return collision.IsGrounded();
}
    /// <summary>
    /// ✅ `Tetromino` の落下処理 (`Update()` 内で実行)
    /// </summary>
    void Update()
    {
        if (isFalling) HandleFall(); // ✅ `isFalling` が `true` の場合、落下処理を実行
    }

    /// <summary>
    /// ✅ `Tetromino` を `fallInterval` ごとに 1マス (`Vector3.down`) 落とす
    /// </summary>
    private void HandleFall()
{
    fallTimer -= Time.deltaTime;

    if (fallTimer <= 0)
    {
        bool isGrounded = collision.IsGrounded();
        bool isColliding = collision.CheckCollision(Vector3.down);

        Debug.Log($"落下処理: isGrounded = {isGrounded}, isColliding = {isColliding}, 位置: {transform.position}");

        if (isGrounded || isColliding) // ✅ ここで着地判定の原因を特定
        {
            isFalling = false;
            hasLanded = true;
            Debug.Log("着地判定: Tetromino の落下を停止");
            return;
        }

        transform.position += Vector3.down;
        fallTimer = fallInterval;
    }
}
}

