using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 10); // �ӏ��A�Oʮ������Ԅӄh���Լ�
    }

    // ��ײ�ɜy���������һ��������С�Target���˻`���t�h���Լ�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
            Destroy(gameObject);
    }
}