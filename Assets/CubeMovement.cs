using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public void HandleMovement(CubeCollision collision)
    {
        // ✅ 左移動 (`←` または `A`)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (!collision.CheckCollision(Vector3.left))
                transform.position += Vector3.left;
        }

        // ✅ 右移動 (`→` または `D`)
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (!collision.CheckCollision(Vector3.right))
                transform.position += Vector3.right;
        }

        // ✅ 前移動 (`↑` または `W`) → `Vector3.forward`
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (!collision.CheckCollision(Vector3.forward))
                transform.position += Vector3.forward;
        }

        // ✅ 後移動 (`↓` または `S`) → `Vector3.back`
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (!collision.CheckCollision(Vector3.back))
                transform.position += Vector3.back;
        }
    }

    public void HandleRotation(CubeCollision collision)
{
    Quaternion originalRotation = transform.rotation; // ✅ 元の回転を保存

    if (Input.GetKeyDown(KeyCode.R))
    {
        transform.Rotate(Vector3.up * 90); // ✅ Y軸回転 (水平)
    }
    else if (Input.GetKeyDown(KeyCode.T))
    {
        transform.Rotate(Vector3.right * 90); // ✅ X軸回転 (前後)
    }
    else if (Input.GetKeyDown(KeyCode.Y))
    {
        transform.Rotate(Vector3.forward * 90); // ✅ Z軸回転 (左右)
    }

    // ✅ 衝突がある場合は元の回転に戻す
    if (collision.CheckCollision(Vector3.zero))
    {
        transform.rotation = originalRotation;
        Debug.Log("回転をキャンセル: 衝突を検出");
    }
}

}
