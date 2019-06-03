using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlime : MonoBehaviour
{
    [Header("State")]
    public int HP;
    public int maxMiniSlime;
    public float scale;

    [Header("Movement")]
    public int movePower;

    [Header("Flag")]
    public bool canAttack;

    private Vector3 move;
    private int movementFlag = 0;    
    private Animator anim;
    private SpawnMiniSlime spawn;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spawn = GetComponent<SpawnMiniSlime>();

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
            anim.SetBool("isMoving", false);
        else
            anim.SetBool("isMoving", true);

        yield return new WaitForSeconds(3.0f);

        StartCoroutine("ChangeMovement");
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            spawn.SpawnMiniMonster(3);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (movementFlag == 1)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1) * scale;
        }
        else if (movementFlag == 2)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3 (-1, 1, 1) * scale;
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
}
