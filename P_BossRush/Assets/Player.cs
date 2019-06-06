using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower = 1.0f;
    public float jumpPower = 1.0f;

    Rigidbody2D rigid;
    SpriteRenderer render;
    Animator anim;

    public Collision coll;

    Vector3 movement;
    bool isJumping = false;

    public ParticleSystem jumpParticle;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
        render = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DialogueManager.instance.DisplayNextSentence();
        }

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("isMoving", false);
        }
        else 
        {
            anim.SetBool("isMoving", true);
        }


        if (Input.GetButtonDown("Jump"))
            isJumping = true;

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if(!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("눌렀다");
            jumpParticle.time = 0f;
            jumpParticle.Play(true);
        }

        if (Input.GetKeyDown(KeyCode.J))
            rotateEffector(0.5f);
    }

    public PlatformEffector2D effector;

    IEnumerator rotateEffector(float time)
    {
        effector.rotationalOffset = 180;

        yield return new WaitForSeconds(time);

        effector.rotationalOffset = 0;
    }

    void GroundTouch()
    {

        //side = anim.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }
    
    public bool groundTouch;

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;

            render.flipX = true;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;

            render.flipX = false;
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
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
}
