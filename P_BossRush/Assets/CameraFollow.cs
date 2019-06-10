using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    Transform playerPos;

    public float followSpeed;

    void Start()
    {
        playerPos = player.transform;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerPos.position, followSpeed * Time.deltaTime);

        transform.Translate(0, 0, -10);
    }
}
