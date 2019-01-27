using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
	// Public
	public float moveSpeed;
	public float jumpVelocity;
	public float fallMultiplier = 2.5F;
	public float lowJumpMultiplier = 2.0F;

	public float checkRadius;
	public Transform feetPos;
	public LayerMask whatIsGround;

	public MOVE_DIR playerDir = MOVE_DIR.IDLE;

    public Animator animator;

    // Private
    private float moveInput;

	private bool isGrounded = false;
	private bool isJumping 	= false;

	private Rigidbody2D rigidbody;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{

	}

	void FixedUpdate()
	{
		OnMove();
		OnJump();
	}
	// 플레이어 이동 함수
	void OnMove()
	{
		moveInput = Input.GetAxisRaw("Horizontal");

		rigidbody.velocity = new Vector2(moveInput * moveSpeed, rigidbody.velocity.y);

		// 방향 전환
		if (moveInput > 0)
		{
            animator.SetBool("isMoving", true);

            playerDir = MOVE_DIR.RIGHT;
			transform.localScale = new Vector3(1, 1, 1);
		}
		else if (moveInput < 0)
		{
            animator.SetBool("isMoving", true);

            playerDir = MOVE_DIR.LEFT;
			transform.localScale = new Vector3(-1, 1, 1);
		}

        else if (moveInput == 0)
        {
            animator.SetBool("isMoving", false);
        }
	}
	// 플레이어 점프 함수
	void OnJump()
	{
		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (!isGrounded)
        {
            animator.SetBool("isJumping", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }

		if (isGrounded == true && Input.GetButtonDown("Jump"))
		{
        	rigidbody.velocity = Vector2.up * jumpVelocity;
		}

		if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
	}
}
