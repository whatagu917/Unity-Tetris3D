using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // ✅ シングルトンパターン (ゲーム全体で `GameManager` を共有)

    public GameObject[] tetrominoPrefabs; // ✅ `Tetromino` のプレハブ配列 (複数のブロックパターンを管理)
    public Vector3 spawnPosition = new Vector3(0, 10, 0); // ✅ `Tetromino` のスポーン位置 (フィールドの上部)

    private void Awake()
    {
        // ✅ シングルトンの設定 (ゲーム内で `GameManager` のインスタンスを1つにする)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SpawnNewBlock(); // ✅ ゲーム開始時に最初の `Tetromino` をスポーン
    }

    /// <summary>
    /// ✅ 新しい `Tetromino` を生成
    /// </summary>
    public void SpawnNewBlock()
{
    if (Instance == null)
    {
        Debug.LogError("❌ GameManager.Instance が `null` です！");
        return;
    }

    if (tetrominoPrefabs.Length == 0)
    {
        Debug.LogError("❌ tetrominoPrefabs が空です！");
        return;
    }

    int index = Random.Range(0, tetrominoPrefabs.Length);
    GameObject newBlock = Instantiate(tetrominoPrefabs[index], spawnPosition, Quaternion.identity);

    newBlock.transform.position = spawnPosition; // ✅ スポーン位置を再適用

    Debug.Log($"🟢 新しい Tetromino をスポーン: {newBlock.name}, 位置: {newBlock.transform.position}");
}

}

