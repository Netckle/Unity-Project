using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;

    public Text text;

    private Vector3 vector;

    void Update()
    {
        // 1초에 moveSpeed만큼
        vector.Set(text.transform.position.x, text.transform.position.y + (moveSpeed * Time.deltaTime), text.transform.position.z);
        text.transform.position = vector;

        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0)
            Destroy(this.gameObject);
    }
}
