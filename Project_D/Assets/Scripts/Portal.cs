using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject spawnPos; 
    public GameObject sameTypePortal;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            // 충돌한 박스 오브젝트를 지정된 위치로 순간이동 시킨다.
            collision.gameObject.transform.position = sameTypePortal.GetComponent<Portal>().spawnPos.transform.position;
            // 포탈과 충돌할 때마다 박스의 이동속도를 증가시킨다.
            //collision.gameObject.GetComponent<Box>().AccelBoxSpeed();
        }
    }
}
