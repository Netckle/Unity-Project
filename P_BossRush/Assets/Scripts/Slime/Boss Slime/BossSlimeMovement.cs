using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeMovement : MonoBehaviour
{
    public MiniSlimesController miniSlimes;
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
    public bool canRelease = false;

    private Animator anim;

    public bool pase02 = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        //StartCoroutine(BossPattern());

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (pase02)
        {
            anim.SetBool("pase02", true);
        }
        spawned = miniSlimes.miniSlimeIs();

        if (pause)
        {
            PauseManager.instance.Pause(this.gameObject, true);
            canRelease = true;
        }       

        if (canRelease && !pause)
        {
            PauseManager.instance.Release(this.gameObject, "BossSlime");
        }
    }

    void FixedUpdate()
    {
        Jump();
    }
    public void BossPattern()
    {
        StartCoroutine(CoBossPattern());
    }
    // Main Coroutine
    IEnumerator CoBossPattern()
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

        StartCoroutine(CoBossPattern());
    }

    // Move Parts
    IEnumerator MoveToNewLine(int lineNum, float waitTime, float yPadding)
    {
        moveToNewLine = false;
        // Fade Effect

        yield return new WaitForSeconds(waitTime);
        transform.position = miniSlimes.lines[lineNum].transform.position;

        // Fade Effect
        moveToNewLine = true;
    }

    IEnumerator MoveToMiddle(float movePower, float waitTime)
    {
        moveToMiddle = false;

        Vector3 end = new Vector3(0, transform.position.y, 0);

        transform.localScale = new Vector3(3, 3, 3);

        while (Vector3.Distance(transform.position, end) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, movePower * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }        
        transform.position = new Vector3(0, transform.position.y, 0);     

        moveToMiddle = true;
    }

    IEnumerator MoveFromMiddleToSide(float movePower, float halfRange, float waitTime)
    {
        moveFromMiddleToSide = false;

        Vector3 leftEnd = transform.position - new Vector3(halfRange, 0, 0);
        Vector3 rightEnd = transform.position + new Vector3(halfRange, 0, 0);

        transform.localScale = new Vector3(3, 3, 3);

        while (Vector3.Distance(transform.position, leftEnd) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftEnd, movePower * Time.deltaTime);      
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(waitTime);

        transform.localScale = new Vector3(-3, 3, 3);

        while (Vector3.Distance(transform.position, rightEnd) > 0.5f)
        {            
            transform.position = Vector3.MoveTowards(transform.position, rightEnd, movePower * Time.deltaTime);   
            yield return new WaitForSeconds(0.01f);      
        }

        moveFromMiddleToSide = true;
    }

    IEnumerator Jump(int count, float waitTime)
    {
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
        miniSlimes.SpawnSlime(2);
        yield return new WaitForSeconds(waitTime);
    }
}
