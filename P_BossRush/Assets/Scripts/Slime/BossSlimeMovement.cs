using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeMovement : MonoBehaviour
{
    public test1 test;
    public PlayerMovement player;

    public float movePower;
    public float yPadding;
    public float halfRange;

    public float jumpPower;

    // End Flags
    public bool moveToNewLine = false;
    public bool moveToMiddle = false;
    public bool moveFromMiddleToSide = false;

    public bool jumping = false;

    public bool isJumping = false;

    private Rigidbody2D rigid;

    public bool canDamaged = false; // 공격을 받을 수 있는 상태인가.

    public bool pause = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(BossPattern());
    }

    void Update()
    {
        spawned = test.miniSlimeIs();

        if (pause)
        {
            PauseManager.instance.Pause(this.gameObject, "Monster");
        }        
    }

    void FixedUpdate()
    {
        Jump();
    }

    // Main Coroutine
    IEnumerator BossPattern()
    {
        StartCoroutine(MoveToNewLine(player.currentLine, 1.0f, yPadding));

        yield return new WaitUntil(() => moveToNewLine);        

        StartCoroutine(MoveFromMiddleToSide(movePower, halfRange, 1.0f));

        yield return new WaitUntil(() => moveFromMiddleToSide);

        StartCoroutine(MoveToMiddle(movePower, 1.0f));
    
        yield return new WaitUntil(() => moveToMiddle);

        StartCoroutine(Jump(2, 2.0f));

        yield return new WaitUntil(() => jumping);

        StartCoroutine(Spawn(2.0f));

        yield return new WaitUntil(() => spawned);

        StartCoroutine(BossPattern());
    }

    // Move Parts
    IEnumerator MoveToNewLine(int lineNum, float waitTime, float yPadding)
    {
        Debug.Log("MoveToNewLine 실행중.");
        moveToNewLine = false;
        // Fade Effect

        yield return new WaitForSeconds(waitTime);
        transform.position = test.lines[lineNum].transform.position;

        // Fade Effect
        moveToNewLine = true;
    }

    IEnumerator MoveToMiddle(float movePower, float waitTime)
    {
        Debug.Log("MoveToMiddle 실행중.");
        moveToMiddle = false;

        Vector3 end = new Vector3(0, transform.position.y, 0);

        while (Vector3.Distance(transform.position, end) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, movePower * Time.deltaTime);
            yield return new WaitForSeconds(0.05f);
        }        
        transform.position = new Vector3(0, transform.position.y, 0);     

        moveToMiddle = true;
    }

    IEnumerator MoveFromMiddleToSide(float movePower, float halfRange, float waitTime)
    {
        Debug.Log("MoveFromMiddleToSide 실행중.");
        moveFromMiddleToSide = false;

        Vector3 leftEnd = transform.position - new Vector3(halfRange, 0, 0);
        Vector3 rightEnd = transform.position + new Vector3(halfRange, 0, 0);

        Debug.Log(leftEnd + " " + rightEnd);

        while (Vector3.Distance(transform.position, leftEnd) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftEnd, movePower * Time.deltaTime);      
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(waitTime);

        while (Vector3.Distance(transform.position, rightEnd) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightEnd, movePower * Time.deltaTime);   
            yield return new WaitForSeconds(0.05f);      
        }

        moveFromMiddleToSide = true;
    }

    IEnumerator Jump(int count, float waitTime)
    {
        Debug.Log("점프");
        jumping = false;

        for (int i = 0; i < count; ++i)
        {
            isJumping = true;

            yield return new WaitForSeconds(waitTime);
        }
        isJumping = false;
        jumping = true;
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

    public bool spawned = false;

    IEnumerator Spawn(float waitTime)
    {
        spawned = false;
        test.SpawnSlime(2);
        yield return new WaitForSeconds(waitTime);
    }
}
