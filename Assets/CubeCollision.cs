using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    /// <summary>
    /// ✅ 指定された `direction` に `Tetromino` が移動できるかを判定
    /// </summary>
    /// <param name="direction">移動方向 (例: `Vector3.left`, `Vector3.right`, `Vector3.down`)</param>
    /// <returns>`true` (衝突あり) / `false` (衝突なし)</returns>
    public bool CheckCollision(Vector3 direction)
{
    foreach (Transform child in transform)
    {
        RaycastHit hit;
        Vector3 rayStart = child.position + direction * 0.5f; // ✅ 誤判定を防ぐために `0.1f` に変更
        Debug.DrawRay(rayStart, direction * 1.0f, Color.red, 0.1f); // ✅ `Raycast` を可視化

        if (Physics.Raycast(rayStart, direction, out hit, 1.0f))
        {
            Debug.Log($"衝突検出: {hit.collider.name} ({gameObject.name})");
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Block") || hit.collider.CompareTag("Ground"))
            {
                return true;
            }
        }
    }
    return false;
}

    /// <summary>
    /// ✅ `Tetromino` が地面 (`Ground`) または他の `Block` に着地しているかを判定
    /// </summary>
    /// <returns>`true` (着地) / `false` (空中)</returns>
    public bool IsGrounded() => CheckCollision(Vector3.down);
}
