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

    [Space]
    [Header("움직임 플래그")]    
    public bool moveToNewLine = false;

    public bool moveToMiddle = false;
    public bool moveToSide = false;

    public bool blockDmgIsEnd = false;

    public bool pause = false;

    [Space]
    [Header("플래그")]
    public bool spawnIsEnd = false;
    public bool jumpIsEnd = false;

    public bool isJumping = false;

    [Space]
    [Header("페이즈 플래그")]
    public bool phase02IsOn = false;

    private Rigidbody2D rigid;
    private Animator anim;

    private bool spawnedSlimeIsExist = false;
    public bool canDamaged = false;

    /*
    보스 페이즈
    01. 플레이어가 있는 곳으로 순간이동 - 방방뛰면서 좌우로 이동 - 중간으로 이동 - 미니 슬라임 소환 (일반 움직임) 
    + 미니 슬라임이 활성화 된 동안은 공격을 받지 않음. (아마도 콜라이더 isTrigger가 좋을듯) 모두 제거하면 한동안 경직되어 공격할 수 있는 시간 주어짐. 물론 이동 중에도 때릴 수 있음.

    02. 컨셉 : 버그 걸린 보스
    오른쪽 끝에 순간이동 후 한 방향으로 돌진 - 벽에 닿으면 남은 다른 라인으로 이동 해서 반복 - 모든 라인을 한 번씩 돌면 중간으로 이동 - 미니 슬라임 소환 (방방 뛰어다님)
    + 피는 절반에 발동, 속도는 up 됨.
    */

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        spawnedSlimeIsExist = miniSlimes.CheckSlimeIsActive();

        switch(spawnedSlimeIsExist)
        {
            case true:
                canDamaged = false;
                break;
            case false:
                canDamaged = true;
                break;
        }
        
        if (phase02IsOn)
        {
            anim.SetBool("pase02", true);
        }
    }

    void FixedUpdate()
    {
        Jump();
    }

    public void BossPattern()
    {
        StartCoroutine(CoBossPatternPhase01());
    }

    // 1 페이즈 메인 코루틴.
    private IEnumerator CoBossPatternPhase01()
    {
        // 01. 플레이어가 있는 라인으로 이동.
        StartCoroutine(CoMoveToNewLine(player.currentLine, 1.0f));
        yield return new WaitUntil(() => moveToNewLine);        

        // 02. 양 옆 이동.
        StartCoroutine(CoMoveToSide(movePower, halfRange, 1.0f));
        yield return new WaitUntil(() => moveToSide);

        // 03. 중간으로 이동.
        StartCoroutine(CoMoveToMiddle(movePower, 1.0f));    
        yield return new WaitUntil(() => moveToMiddle);

        // 04. 점프함.
        StartCoroutine(CoJump(2, 2.0f));
        yield return new WaitUntil(() => jumpIsEnd);

        // 05. 미니 슬라임 스폰.
        StartCoroutine(CoSpawn(2.0f));
        yield return new WaitUntil(() => spawnIsEnd);

        // 무한 반복.
        StartCoroutine(CoBossPatternPhase01());
    }

    // 플레이어가 있는 라인으로 이동합니다.
    IEnumerator CoMoveToNewLine(int lineNum, float waitTime)
    {
        moveToNewLine = false;

        yield return new WaitForSeconds(waitTime);
        transform.position = miniSlimes.lines[lineNum].transform.position;

        moveToNewLine = true;
    }

    // 맵의 중간으로 이동합니다.
    IEnumerator CoMoveToMiddle(float movePower, float waitTime)
    {
        moveToMiddle = false;

        Vector3 endPos = new Vector3(0, transform.position.y, 0);

        transform.localScale = new Vector3(3, 3, 3);

        while (Vector3.Distance(transform.position, endPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, movePower * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }        
        transform.position = new Vector3(0, transform.position.y, 0);     

        moveToMiddle = true;
    }

    IEnumerator CoMoveToSide(float movePower, float halfRange, float waitTime)
    {
        moveToSide = false;

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

        moveToSide = true;
    }

    IEnumerator CoJump(int count, float waitTime)
    {
        jumpIsEnd = false;

        for (int i = 0; i < count; ++i)
        {
            isJumping = true;
            yield return new WaitForSeconds(waitTime);
        }

        isJumping = false;
        jumpIsEnd = true;
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

    IEnumerator CoSpawn(float waitTime)
    {
        spawnIsEnd = false;

        miniSlimes.SpawnMiniSlimes(2);
        yield return new WaitForSeconds(waitTime);
    }
}
