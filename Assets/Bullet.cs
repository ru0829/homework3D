using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 10); // 子彈預設十秒後會自動刪除自己
    }

    // 碰撞偵測：如果碰到一個物件帶有「Target」標籤，則刪除自己
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
            Destroy(gameObject);
    }
}