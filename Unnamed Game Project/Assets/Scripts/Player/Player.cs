using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    static Player instance = null; 

    public static Player Instace()
    {
        return instance;
    } 

    public int              currentRoomNum = 0;       

    public SpriteRenderer   render;
    public Animator         animator;
    private Rigidbody2D     rigid;
    private Collider2D      collide;

    private Vector3         movement;
    public bool             canMove = true;
    private bool            isJumping = false;
    private bool            isHanging = false;

    public float            movePower = 1.0f;
    public float            jumpPower = 1.0f;

    private bool            isUnBeatTime = false;

    // Jump
    public Transform        groundCheck;
    public LayerMask        whatIsGround;
    public float            checkRadius;
    private bool            isGrounded = false;
    
    public int              extraJumpsValue;
    private int             extraJumps;

    public Collider2D otherCollider;

    // Attack
    public float            cooldown = 0.5f;   // Combo Attack Cooldown
    public float            maxTime = 0.8f;    // Accepted Combo Limit Time
    public int              maxCombo;          // Combo Attack Max Count
    private int             combo = 0;         // Current Combo Count
    private float           lastTime;          // Last Attack Time

    public int              maxHealth = 6;
    public int              health = 6;
    
    // Dialogue
    public Vector3          targetPos;
    public bool             isTalking = false;
    public float            horPaddingSpace = 0.0f;

    private TYPE            dialogueType = TYPE.NORMAL;   

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        StartCoroutine(MeleeAttack());

        rigid = gameObject.GetComponent<Rigidbody2D>();
        collide = gameObject.GetComponent<Collider2D>();

        extraJumps = extraJumpsValue;        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ItemIO.SaveDate();
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            otherCollider.enabled = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            otherCollider.enabled = false;
        }
        // 대화중이고, 상호작용 키를 누르면 다음 대화로 넘어갑니다.
        if ((isTalking) && (Input.GetButtonDown("Interact")))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogueType);
        }

        if (!canMove) // 움직일 수 없는 상태라면...
        {
            animator.SetBool("isMoving", false);
        }
        else if (canMove) // 움직일 수 있는 상태라면...
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                animator.SetBool("isMoving", false);
            }
            else if (Input.GetAxisRaw("Horizontal") != 0)
            {
                animator.SetBool("isMoving", true);
            }
        }      

        // 일반 점프 & 추가 점프
        if (!isHanging && isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            animator.SetBool("isJumping", true);    
            animator.SetTrigger("doJumping");        
        }

        else if (!isHanging && !isGrounded && Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            isJumping = true;            
            animator.SetBool("isJumping", true);  
            animator.SetTrigger("doJumping");  

            extraJumps--;
        }

        // 매달려 있을 때 점프
        if (isHanging && Input.GetButtonDown("Jump"))
        {
            canMove = true;
            collide.enabled = true;

            rigid.velocity = Vector2.zero;
            rigid.gravityScale = 1;

            isJumping = true;

            animator.SetBool("isHanging", false);
            animator.SetBool("isJumping", true);  
            animator.SetTrigger("doJumping");
        }

        // 추가 점프 횟수 초기화
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (rigid.velocity.y < 0) // 플레이어 속도가 - : 낙하하고 있을 때.
        {
            animator.SetBool("isFalling", true);
        }
        else 
        {
            animator.SetBool("isFalling", false);
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (canMove) 
            Move();

        Jump();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Attacked by Creature
        if ((other.gameObject.tag == "Monster" || other.gameObject.tag == "MonsterAttack") && !isUnBeatTime)
        {
            canMove = false;
            Vector2 attackedVelocity = Vector2.zero;

            if (other.gameObject.transform.position.x > transform.position.x)
            {
                attackedVelocity = new Vector2(-2f, 2f);
            }
            else
            {
                attackedVelocity = new Vector2(2f, 2f);
            }

            rigid.AddForce(attackedVelocity, ForceMode2D.Impulse);

            // Health Down
            health--;

            // UnBeatTime On
            if (health > 0)
            {
                isUnBeatTime = true;
                StartCoroutine("UnBeatTime");
            }
        }
    }

    // [행동 함수]

    public void Hanging()
    {
        extraJumps = extraJumpsValue; // Charge Jump Count
    
        rigid.Sleep();
        collide.enabled = false;

        animator.SetBool("isHanging", true);

        rigid.velocity = Vector3.zero;
        rigid.gravityScale = 0;

        canMove = false; // Prevent Movement
        isHanging = true;
    }
    
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!isJumping) return;

        // Prevent Velocity Amplification
        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
    }

    // [코루틴 함수]

    IEnumerator UnBeatTime() // 깜박거리는 효과.
    {
        int countTime = 0;

        while (countTime < 10)
        {
            if (countTime >= 5)
            {
                canMove = true;
            }

            // Alpha Effect
            if (countTime % 2 == 0)
            {
                render.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                render.color = new Color32(255, 255, 255, 180);
            }

            // Wait Update Frame
            yield return new WaitForSeconds(0.2f);

            countTime++;
        }

        // Alpha Effect End
        render.color = new Color32(255, 255, 255, 255);

        // UnBeatTime Off
        isUnBeatTime = false;
        //canMove = true;
        yield return null;
    }

    public IEnumerator MoveToPlayerForTalk() // 대화시 플레이어를 적당한 거리로 떨어뜨려 놓음.
    {
        while((transform.position.x >= targetPos.x))
        {
            transform.position += Vector3.left * movePower * Time.deltaTime;          
            yield return null;
        }
    }

    IEnumerator MeleeAttack()
    {
        // Constantly loops so you only have to call it once
        while(true)
        {
            // Checks if attacking and then starts of the combo
            if (Input.GetKeyDown(KeyCode.Q))
            {
                canMove = false;
                combo++;

                animator.SetBool("isAttacking", true);

                animator.SetInteger("attackCount", combo);
                lastTime = Time.time;

                //Combo loop that ends the combo if you reach the maxTime between attacks, or reach the end of the combo
                while((Time.time - lastTime) < maxTime && combo < maxCombo)
                {
                    // Attacks if your cooldown has reset
                    if (Input.GetKeyDown(KeyCode.Q) && (Time.time - lastTime) > cooldown)
                    {
                        combo++;

                        animator.SetInteger("attackCount", combo);
                        lastTime = Time.time;
                    }
                    yield return null;
                }                
                // Resets combo and waits the remaining amount of cooldown time before you can attack again to restart the combo
                canMove = true;

                combo = 0;
                animator.SetBool("isAttacking", false);
                animator.SetInteger("attackCount", combo);
                
                yield return new WaitForSeconds(cooldown - (Time.time - lastTime));
            }
            yield return null;
        }
    } 
}
