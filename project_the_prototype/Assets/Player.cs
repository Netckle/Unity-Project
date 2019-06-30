using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private Collision coll;
    private Animator anim;
    private AnimationScript animScript;

    public float moveSpeed = 10.0f;
    public float jumpPower = 50.0f;
    
    private bool groundTouch;

    public int side = 1;
    public int currentLine = 0;

    public ParticleSystem jumpParticle;

    public bool canGoDown;
    public PlatformEffector2D effector;

    public bool isTalking;
    public bool canMove;

    public void ChangePos(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        animScript = GetComponentInChildren<AnimationScript>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);

        Walk(dir);        
        animScript.SetHorizontalMovement(x, y, rb.velocity.y);

        UpdateWithButtonInput();

        if (!canMove) return;
        
        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }        

        if (x > 0)
        {
            side = 1;
            animScript.Flip(side);
        }

        if (x < 0)
        {
            side = -1;
            animScript.Flip(side);
        }
    }

    void GroundTouch()
    {
        side = animScript.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    } 

    void UpdateWithButtonInput()
    {
        // 대화중이고 A 버튼을 눌렀을 경우 다음 대사로 넘어감.
        if (isTalking && Input.GetButtonDown("A"))
        {
            DialogueManager.instance.DisplayNextSentence();
        }

        // 내려갈 수 있는 상태가 아니고 B버튼을 누르면 이펙터 돌리는 코루틴 실행.
        if (!canGoDown && Input.GetButtonDown("B"))
        {
            StartCoroutine(CoRotatePlatformEffector(0.5f));
        }
    }

    void Walk(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
    }

    void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpPower;

        jumpParticle.Play();
    }

    IEnumerator CoRotatePlatformEffector(float waitTime)
    {
        canGoDown = true;

        effector.rotationalOffset = 180;
        yield return new WaitForSeconds(waitTime);
        effector.rotationalOffset = 0;

        canGoDown = false;
    }
}
