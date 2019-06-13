using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerMovement : MonoBehaviour
{   
    private Collision coll;
    private AnimationScript anim;
    [HideInInspector]
    public Rigidbody2D rb;

    [Space]
    [Header("Move Value")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    private bool groundTouch;
    private bool hasDashed;

    [Space]
    [Header("Flag")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;    

    [Space]
    public int side = 1;
    public int currentLine = 0;

    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    [Space]
    [Header("Platformer Effector")]
    public bool canGoDown = false;
    public PlatformEffector2D effector;

    void Start()
    {
        coll = GetComponent<Collision>();
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();

        StartCoroutine(MeleeAttack());
    }

    void Update()
    {
        float x     = Input.GetAxis("Horizontal");
        float y     = Input.GetAxis("Vertical");
        float xRaw  = Input.GetAxisRaw("Horizontal");
        float yRaw  = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, rb.velocity.y);

        // 벽을 잡는 상황.
        if (coll.onWall && Input.GetButton("Fire3") && canMove)
        {
            if (side != coll.wallSide)
                anim.Flip(side * -1);

            wallGrab  = true;
            wallSlide = false;
        }

        // 벽짚기 버튼을 떼거나, 벽이 아니거나, 움직일 수 없는 상황.
        if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove)
        {
            wallGrab  = false;
            wallSlide = false;
        }

        // 바닥에 있고 대시하는 중이 아닌 상황.
        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        // 벽을 짚고 대시는 안한 상황.
        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (x > 0.2f || x < -0.2f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }

            float speedModifier = y > 0 ? 0.5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        // 벽에 있고 바닥에 없는 상황.
        if (coll.onWall && !coll.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround)
        {
            wallSlide = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("jump");

            if (coll.onGround)
            {
                Jump(Vector2.up, false);
            }
            if (coll.onWall && !coll.onGround)
            {
                WallJump();
            }
        }

        // 대시하지 않은 상태에서 대시버튼을 눌렀을 경우.
        if (Input.GetButtonDown("Fire1") && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                Dash(xRaw, yRaw);
            }
        }

        // 바닥에 충돌했는데 groundTouch가 안바꼈을 경우.
        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        // 바닥에 충돌하지 않고 groundTouch가 안바꼈을 경우.
        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;

        if (x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
        }

        if (Input.GetKeyDown(KeyCode.J) && !canGoDown)
        {
            StartCoroutine(rotateEffector(0.5f));
        }
    }

    void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    void Dash(float x, float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    void Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        particle.Play();
    }

    void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || (side == -1 && !coll.onRightWall))
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(0.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    void WallSlide()
    {
        if (coll.wallSide != side)
        {
            anim.Flip(side * -1);
        }

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }    

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }

    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    IEnumerator rotateEffector(float time)
    {
        canGoDown = true;
        effector.rotationalOffset = 180;

        yield return new WaitForSeconds(time);

        effector.rotationalOffset = 0;
        canGoDown = false;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator DashWait()
    {
        FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        dashParticle.Play();
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.onGround)
            hasDashed = false;
    }

    // Attack
    public float            cooldown = 0.5f;   // Combo Attack Cooldown
    public float            maxTime = 0.8f;    // Accepted Combo Limit Time
    public int              maxCombo;          // Combo Attack Max Count
    private int             combo = 0;         // Current Combo Count
    private float           lastTime;          // Last Attack Time

    public int              maxHealth = 6;
    public int              health = 6;

    public Animator animator;

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
