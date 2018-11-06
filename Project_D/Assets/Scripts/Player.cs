using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 스크립트
public class Player : MonoBehaviour
{
    /*
     * [필요한 행동]
     *  - 좌우로 움직이기 : Move()        | 특이사항 : 
     *  - 점프하기        : Jump()        | 특이사항 :
     *  - 박스 잡기       : GrabBox()     | 특이사항 : 일정시간 동안 들고 있어야 하므로 타이머 기능 필요.
     *  - 박스 공격하기   : AttackBox()   | 특이사항 :
     */

    // ----- [움직임 관련 변수]
    private float moveInput;
    public float moveSpeed;

    public bool isRight = true;

    // ----- [점프 관련 변수]
    public float jumpVelocity;
    public float fallMultiplier = 2.5F;
    public float lowJumpMultiplier = 2.0F;

    private Rigidbody2D rigid;

    // ----- [바닥 확인 관련 변수]
    private bool isGrounded = false;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    // ----- [잡기 관련 변수]
    public bool isGrabbed = false;
    RaycastHit2D hit_grab;

    public float distance = 2.0F;
    public Transform holdPoint;
    public float throwForce;
    public LayerMask notGrabbed;
    // ----- [공격 관련 변수]
    public bool isAttacked = false;
    RaycastHit2D hit_attack;

    CameraShake shake;

    public Animator anim;

    // ----- [함수 정의]
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        shake = Camera.main.GetComponent<CameraShake>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GrabBox();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AttackBox();
        }
        if (isGrabbed)
            hit_grab.collider.gameObject.transform.position = holdPoint.position;

        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
        else if (!isGrounded)
        {
            anim.SetBool("isJumping", true);
        }
    }

    void FixedUpdate()
    {
        // 둘 다 리지드바드와 관련되어 있으므로 FixedUpdate()에 사용한다.
        Move();
        Jump();
    }

    // ----- [행동 관련 함수]
    void Move()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        

        rigid.velocity = new Vector2(moveInput * moveSpeed, rigid.velocity.y);

        if (moveInput > 0)
        {
            anim.SetBool("isMoving", true);
            isRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            anim.SetBool("isMoving", true);
            isRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput == 0)
        {
            anim.SetBool("isMoving", false);
        }
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
        }


        if (rigid.velocity.y < 0)
        {
            rigid.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigid.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigid.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


    }

    void GrabBox()
    {
        if (!isGrabbed)
        {
            Physics2D.queriesStartInColliders = false;
            hit_grab = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

            if (hit_grab.collider == null || hit_grab.collider.tag != "Box")
                return;

            if (hit_grab.collider != null && hit_grab.collider.tag == "Box")
            {
                hit_grab.collider.gameObject.GetComponent<Box>().state = BOXSTATE.IDLE;
                isGrabbed = true;
            }            
        }
        else if (Physics2D.OverlapPoint(holdPoint.position, notGrabbed))
        {
            isGrabbed = false;

            if (hit_grab.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                hit_grab.collider.gameObject.GetComponent<Box>().state = BOXSTATE.THROWING;
                hit_grab.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwForce;
            }
        }
    }

    void AttackBox()
    {
        if (!isAttacked)
        {
            Physics2D.queriesStartInColliders = false;
            hit_attack = Physics2D.Raycast(transform.position + new Vector3(0, 0.35F, 0), Vector2.right * transform.localScale.x, distance);

            if (hit_attack.collider == null || hit_attack.collider.tag != "Box")
                return;

            if (hit_attack.collider != null && hit_attack.collider.tag == "Box")
            {
                shake.StartCoroutine("Shake");

                hit_attack.collider.gameObject.GetComponent<Box>().state = BOXSTATE.THROWING;

                if (hit_attack.collider.gameObject.GetComponent<Box>().GetRigid() > 0)
                {
                    hit_attack.collider.gameObject.GetComponent<Box>().dir = BOXDIR.LEFT;
                }
                else if (hit_attack.collider.gameObject.GetComponent<Box>().GetRigid() < 0)
                {
                    hit_attack.collider.gameObject.GetComponent<Box>().dir = BOXDIR.RIGHT;
                }

                hit_attack.collider.gameObject.GetComponent<Box>().AccelBoxSpeed();
                hit_attack.collider.gameObject.GetComponent<Box>().ChangeSprite(this.gameObject);
            }
        }
        isAttacked = false;
    }

    // ----- [디버그용 함수]
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, 0.35F, 0), (transform.position + new Vector3(0, 0.35F, 0)) + Vector3.right * transform.localScale.x * distance);
    }
}
