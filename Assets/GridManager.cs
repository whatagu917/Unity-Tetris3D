using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance; // ✅ シングルトン (ゲーム全体で `GridManager` を共有)
    public static int width = 10;  // ✅ `X` 軸のサイズ (横幅)
    public static int height = 20; // ✅ `Y` 軸のサイズ (高さ)
    public static Transform[,,] grid = new Transform[width, height, width]; // ✅ `3D` グリッド (`x, y, z`)

    private void Awake()
    {
        // ✅ `GridManager` のシングルトン設定
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// ✅ `Vector3` の座標を整数値に丸める
    /// </summary>
    public static Vector3 RoundVector3(Vector3 pos)
{
    return new Vector3(
        Mathf.Max(0, Mathf.Round(pos.x)), // ✅ 最小値を `0` に設定
        Mathf.Max(0, Mathf.Round(pos.y)),
        Mathf.Max(0, Mathf.Round(pos.z))
    );
}


    /// <summary>
    /// ✅ 指定した座標が `Grid` 内に収まっているか判定
    /// </summary>
    public static bool InsideGrid(Vector3 pos)
    {
        return pos.x >= 0 && pos.x < width && pos.z >= 0 && pos.z < width && pos.y >= 0;
    }

    /// <summary>
    /// ✅ ブロック (`Cube`) を `Grid` に登録
    /// </summary>
    public static void AddToGrid(Transform block)
{
    Vector3 pos = RoundVector3(block.position);

    // ✅ 座標が `grid` の範囲外ならエラー回避
    if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height || pos.z < 0 || pos.z >= width)
    {
        Debug.LogError($"❌ `AddToGrid()` で範囲外エラー！ pos = {pos}");
        return;
    }

    grid[(int)pos.x, (int)pos.y, (int)pos.z] = block;
    Debug.Log($"🟢 Grid に登録: {block.name}, 位置: {pos}");
}


    /// <summary>
    /// ✅ グリッド内のラインをチェックし、埋まっていたら削除
    /// </summary>
    public static void RemoveFullLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y)) // ✅ `y` のラインが埋まっているかチェック
            {
                DeleteLine(y); // ✅ ラインを削除
                MoveDown(y); // ✅ 上のブロックを下に移動
            }
        }
    }

    /// <summary>
    /// ✅ `y` のラインが埋まっているか判定
    /// </summary>
    private static bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < width; z++)
            {
                if (grid[x, y, z] == null) return false; // ✅ 空きがある場合 `false`
            }
        }
        return true; // ✅ ラインが埋まっている場合 `true`
    }

    /// <summary>
    /// ✅ `y` のラインを削除
    /// </summary>
    private static void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < width; z++)
            {
                if (grid[x, y, z] != null)
                {
                    Destroy(grid[x, y, z].gameObject); // ✅ ブロックを削除
                    grid[x, y, z] = null;
                }
            }
        }
    }

    /// <summary>
    /// ✅ 削除したラインの上にあるブロックを下に移動
    /// </summary>
    private static void MoveDown(int y)
    {
        for (int i = y; i < height - 1; i++) // ✅ `y` から上のブロックを下に移動
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    if (grid[x, i + 1, z] != null)
                    {
                        grid[x, i, z] = grid[x, i + 1, z]; // ✅ 上のブロックを `1マス` 下へ
                        grid[x, i, z].position += Vector3.down;
                        grid[x, i + 1, z] = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ✅ 最上部 (`height - 1`) にブロックがある場合、ゲームオーバー
    /// </summary>
    public static bool IsGameOver()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < width; z++)
            {
                if (grid[x, height - 1, z] != null)
                {
                    return true; // ✅ `最上部にブロックあり → ゲームオーバー`
                }
            }
        }
        return false;
    }
}

