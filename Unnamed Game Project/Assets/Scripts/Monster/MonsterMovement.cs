﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float movePower = 1.0f;

    [HideInInspector]
    public Animator animator;
    private Vector3 movement;
    [HideInInspector]
    public int movementFlag = 0; // 0:Idle, 1:Left, 2:Right

    public int creatureType;
    [HideInInspector]
    public bool isTracing = false;
    public GameObject traceTarget; 

    public bool canMove = true;

    private Rigidbody2D rigidbody;
    public SpriteRenderer renderer;

    public int maxHealth = 6;
    public int health = 6;
    private bool isUnBeatTime = false;

    // Change Movement Coroutine
    private IEnumerator ChangeMovement()
    {
        Debug.Log("ChangeMovement 시작합니다.");
        // Random Change Movement
        movementFlag = Random.Range (0, 3);

        // Mapping Animation
        if (movementFlag == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        // Wait 3 Seconds
        yield return new WaitForSeconds(3.0f);
        
        // Restart Logic
        StartCoroutine("ChangeMovement");
    }

    // Attack Coroutine
    private IEnumerator Attack()
    {
        canMove = false;
        StopCoroutine("ChangeMovement");
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(1.5f);

        animator.SetBool("isAttacking", false);
        canMove = true;
        //StartCoroutine("ChangeMovement");

    }

    // UnBeatTime Coroutine
    private IEnumerator UnBeatTime()
    {
        int countTime = 0;

        while (countTime < 10)
        {
            // Alpha Effect
            if (countTime % 2 == 0)
            {
                renderer.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                renderer.color = new Color32(255, 255, 255, 180);
            }

            // Wait Update Frame
            yield return new WaitForSeconds(0.05f);

            countTime++;
        }

        // Alpha Effect End
        renderer.color = new Color32(255, 255, 255, 255);

        // UnBeatTime Off
        isUnBeatTime = false;
        //canMove = true;
        yield return null;
    }

    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        StartCoroutine("ChangeMovement");
    }

    void Update()
    {        
        
    }

    void FixedUpdate()
    {
        if (canMove) Move();
    }

    // Action Function
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        // Trace or Random
        if (!isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;

            if (playerPos.x < transform.position.x)
            {
                dist = "Left";
            }
            else if (playerPos.x > transform.position.x)
            {
                dist = "Right";
            }

            else
            {
                if (movementFlag == 1)
                {
                    dist = "Left";
                }
                else if (movementFlag == 2)
                {
                    dist = "Right";
                }
            }
        }

        // Movement Assign
        if (movementFlag == 1)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3 (-1, 1, 1);
        }
        else if (movementFlag == 2)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }    
    
    void OnTriggerEnter2D (Collider2D other)
    {
        // Attacked by Creature
        if (other.gameObject.tag == "PlayerAttack" && !isUnBeatTime)
        {
            Debug.Log(other.gameObject.name);
            //canMove = false;
            //Camera.main.GetComponent<CameraShake>().StartShake(0.2f, 0.2f);
            // Bouncing
            Vector2 attackedVelocity = Vector2.zero;

            if (other.gameObject.transform.position.x > transform.position.x)
            {
                attackedVelocity = new Vector2(-2f, 2f);
            }
            else
            {
                attackedVelocity = new Vector2(2f, 2f);
            }

            rigidbody.AddForce(attackedVelocity, ForceMode2D.Impulse);

            // Health Down
            health--;

            // UnBeatTime On
            if (health > 0)
            {
                isUnBeatTime = true;
                StartCoroutine(UnBeatTime());
            }
        }
    }
}