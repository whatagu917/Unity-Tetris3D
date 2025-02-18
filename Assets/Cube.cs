using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public GameObject cubePrefab; // ✅ `Tetromino` を構成する `Cube` のプレハブ

    private CubeMovement movement;   // ✅ 移動処理を担当
    private CubeCollision collision; // ✅ 衝突判定を担当
    private CubeGravity gravity;     // ✅ 落下処理を担当
    private Rigidbody rb;            // ✅ `Rigidbody` を管理
    private bool hasSpawnedNextBlock = false; // ✅ 次の `Tetromino` を生成したか判定

    // ✅ `Tetromino` の形を定義
    private List<Vector3[]> shapes = new List<Vector3[]>
{
    // I型
    new Vector3[] { new Vector3(-1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0) },

    // O型
    new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) },

    // L型
    new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 2, 0), new Vector3(1, 2, 0) },

    // J型 (逆L)
    new Vector3[] { new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0), new Vector3(0, 2, 0) }
};

    void Start()
{
    Debug.Log("🟢 Start() 実行: " + gameObject.name);
    movement = GetComponent<CubeMovement>();  // ✅ 移動スクリプト取得
    collision = GetComponent<CubeCollision>(); // ✅ 衝突判定スクリプト取得
    gravity = GetComponent<CubeGravity>();    // ✅ 落下処理スクリプト取得
    rb = GetComponent<Rigidbody>();           // ✅ Rigidbody取得

    if (rb != null)
    {
        rb.isKinematic = false; // ✅ 物理演算を有効化
        rb.useGravity = false;  // ✅ `CubeGravity` で落下管理
    }

    StartCoroutine(EnableGroundCheckDelayed()); // ✅ 0.2秒後に落下判定を開始
    InitializeBlock(); // ✅ `Tetromino` を生成
}


    /// <summary>
    /// ✅ `Tetromino` のブロックを初期化し、形をランダムに決定
    /// </summary>
    private void InitializeBlock()
{
    Vector3[] shape = shapes[Random.Range(0, shapes.Count)]; // ✅ ランダムに形を選択
    transform.localPosition = Vector3.zero; // ✅ `Pivot` を `(0,0,0)` に設定

    foreach (Vector3 offset in shape)
    {
        GameObject cube = Instantiate(cubePrefab, transform); // ✅ `CubePrefab` を生成
        cube.transform.localPosition = offset;
    }

    transform.position = GameManager.Instance.spawnPosition; // ✅ `GameManager` のスポーン位置を適用

    Debug.Log($"🟢 Tetromino スポーン位置: {transform.position}");
}


    /// <summary>
    /// ✅ 0.2秒後に `EnableGroundCheck()` を実行 (落下判定の遅延)
    /// </summary>
    private IEnumerator EnableGroundCheckDelayed()
    {
        yield return new WaitForSeconds(0.2f);
        gravity.EnableGroundCheck();
    }

    void Update()
{
    Debug.Log("🟢 Update() 実行中");

    bool isFalling = gravity.IsFalling();
    bool hasLanded = gravity.HasLanded(); // ✅ `isGrounded` を直接使用

    Debug.Log($"🔍 isFalling: {isFalling}, hasLanded: {hasLanded}, hasSpawnedNextBlock: {hasSpawnedNextBlock}");

    if (!isFalling) 
    {
        Debug.Log("⛔ 落下停止: Update() をスキップ");
        return;
    }

    movement.HandleMovement(collision);
    movement.HandleRotation(collision);

    if (!hasSpawnedNextBlock && hasLanded) // ✅ `isGrounded` を `hasLanded` の代わりに使用
    {
        hasSpawnedNextBlock = true;
        Debug.Log("🟢 FixBlock() を実行: hasSpawnedNextBlock を true に設定");
        FixBlock();
    }
}
    /// <summary>
    /// ✅ `Tetromino` が着地したら固定 & `GridManager` に登録
    /// </summary>
    private void FixBlock()
{
    Debug.Log("🟢 FixBlock: 実行開始");

    foreach (Transform child in transform)
    {
        GridManager.AddToGrid(child);
    }

    GridManager.RemoveFullLines();

    if (GridManager.IsGameOver())
    {
        Debug.Log("❌ ゲームオーバー！");
        return;
    }

    transform.SetParent(null);
    Debug.Log("🟢 FixBlock: `isKinematic = true;` を遅らせる");
    
    // ✅ `isKinematic = true;` を遅らせる
    StartCoroutine(SetKinematicDelayed());
    GameManager.Instance.SpawnNewBlock();
}

/// <summary>
/// ✅ `isKinematic = true;` を 0.1秒遅らせる
/// </summary>
private IEnumerator SetKinematicDelayed()
{
    yield return new WaitForSeconds(0.1f);
    if (rb != null) rb.isKinematic = true;
    Debug.Log("🟢 Rigidbody 固定: `isKinematic = true;`");
}
}
