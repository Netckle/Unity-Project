using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public 변수
    public float moveSpeed = 1.0f;
    //Private 변수
    private Rigidbody2D rigid;
    private Vector3 movement;
    public bool isTalking = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
            }
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
        }

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }

    public void SetTalkFlags(bool isTrue)
    {
        isTalking = isTrue;
    }
}
