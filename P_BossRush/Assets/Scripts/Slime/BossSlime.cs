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

    [Header("Move")]
    public bool destinedGoal = false;
    public Vector3 playerPos;

    Rigidbody2D rigid;

    public bool canMove = true;
    public int jumpCount;
    public float waitTime;
    public int spawnCount;

    public bool monsterSpawned = false;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spawn = GetComponent<SpawnMiniSlime>();
        rigid = GetComponent<Rigidbody2D>();

        //StartCoroutine("ChangeMovement");

        //StartCoroutine(Movement(jumpCount, waitTime,spawnCount));
        StartCoroutine(BossPattern());
    }

    private bool oneCycleCompleted = false;
    public void SetPlayerPos()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        destinedGoal = true;
    }

    public void DeletePlayerPos()
    {
        playerPos = Vector2.zero;
        destinedGoal = false;
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

    public test1 test;
    public PlayerMovement player;

    public bool onMiddle = false;

    public bool secondMove = false;

    IEnumerator BossPattern()
    {
        int line = player.currentLine;

        StartCoroutine(MoveToNewLine(line, 1.0f));

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(MoveMiddleToSide(20, 8, 3.0f));

        //yield return new WaitForSeconds(2.0f);

        //StartCoroutine(MoveToMiddle(movePower, 2.0f));

        //yield return new WaitForSeconds(2.0f);

        yield return new WaitUntil(() => secondMove);
        
        StartCoroutine(MoveToMiddle(20, 0.1F));

        yield return new WaitUntil(() => onMiddle);

        StartCoroutine(BossPattern());
    }

    IEnumerator MoveToNewLine(int lineNum, float waitTime)
    {
        Debug.Log("MoveToNewLine 실행.");
        // 페이드 효과.

        yield return new WaitForSeconds(waitTime);
        transform.position = test.lines[lineNum].transform.position;        

        // 페이드 효과.
    }

    IEnumerator MoveToMiddle(float speed, float waitTime)
    {
        onMiddle = false;
        Debug.Log("MoveToMiddle 실행.");

        if (transform.position.x == 0)
            yield return null;
        
        onMiddle = true;
        Vector3 pos = new Vector3(0, transform.position.y, 0);
        while (Vector3.Distance(transform.position, pos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, 0), speed * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }        

        transform.position = new Vector3(0, transform.position.y, 0);        

        onMiddle = true; 
    }

    IEnumerator MoveMiddleToSide(float speed, float halfRange, float waitTime)
    {
        secondMove = false;

        Debug.Log("MoveMiddleToSide 실행.");

        Vector3 leftEnd  = transform.position - new Vector3(halfRange, 0, 0);
        Vector3 rightEnd = transform.position + new Vector3(halfRange, 0, 0);
        
        while (Vector3.Distance(transform.position, leftEnd) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftEnd, speed * Time.deltaTime);      
            yield return new WaitForSeconds(0.1f);  
        }        

        yield return new WaitForSeconds(waitTime);

        while (Vector3.Distance(transform.position, rightEnd) > 0.5f)
        {
             transform.position = Vector3.MoveTowards(transform.position, rightEnd, speed * Time.deltaTime);   
             yield return new WaitForSeconds(0.1f);      
        }       

        secondMove = true;
    }

    private void Jump(int count)
    {
        
    }

    private void Spawn(int count)
    {
        test.SpawnSlime(count);
    }


    IEnumerator Movement(int jumpCount, float time, int count)
    {
        oneCycleCompleted = false;

        Debug.Log("01. Jump");
        for (int i = 0; i < jumpCount; ++i)
        {
            isJumping = true;
            yield return new WaitForSeconds(time);
        }

        Debug.Log("02. Set Player Position & Move");
        SetPlayerPos();

        yield return new WaitUntil(() => oneCycleCompleted);

        Debug.Log("3. Spawn Mini Monsters");
        canMove = false;
        Spawn(count);
        yield return new WaitForSeconds(1.0f);
        canMove = true;

        yield return new WaitUntil(() => monsterSpawned == false);

        StartCoroutine(Movement(jumpCount, time, count));
    }

    private void Spawn1(int count)
    {
        DeletePlayerPos();      

        spawn.SpawnMiniMonster(count);

        monsterSpawned = true;
    }

    IEnumerator PauseAndSpawnMiniSlime()
    {
        DeletePlayerPos();
        canMove = false;

        // 퐁퐁 뛰는 애니메이션.
        
        spawn.SpawnMiniMonster(3);
        yield return new WaitForSeconds(1.0f);
        canMove = true;
    }

    void Update()
    {
        //Debug.Log(Vector3.Distance(transform.position, playerPos));
        float temp = transform.position.x - playerPos.x;
        float distance = 0f;
        
        if (temp < 0f)
            distance = temp * -1;
        else 
            distance = temp;

        //if (destinedGoal && Vector3.Distance(transform.position, playerPos) <= 2)
        if (destinedGoal && distance < 2)
        {
            //StartCoroutine(CorJump(2, 2.0f));
            DeletePlayerPos();
            oneCycleCompleted = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            spawn.SpawnMiniMonster(3);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetPlayerPos();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            DeletePlayerPos();
        }
    }

    void FixedUpdate()
    {
        if (canMove)
            Move();

        Jump();
    }

    public bool isJumping = false;
    public float jumpPower;

    public IEnumerator CorJump(int count, float waitTime)
    {
        //DeletePlayerPos();
        canMove = false;

        for (int i = 0; i < count; ++i)
        {
            isJumping = true;

            yield return new WaitForSeconds(waitTime);
        }

        //DeletePlayerPos();
        canMove = true;
    }

    void Jump()
    {
        if (!isJumping)
            return;

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
    }

        

    

    void Move()
    {
        if (!destinedGoal)
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
        else if (destinedGoal)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, movePower * Time.deltaTime);
        }
    }
}
