using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TOUNCHED_OBJ
{
	NONE,
	PLAYER,
	ENEMY
}
public enum BOX_STATE
{
	IDLE,
	HOLDING,
	FLYING
}
public enum MOVE_DIR
{
	IDLE,
	RIGHT,
	LEFT
}

public class BOX : MonoBehaviour 
{
	// Public
	public float damage;
	public float moveSpeed;
	public float accelAmount;

	public TOUNCHED_OBJ touched_obj 	= TOUNCHED_OBJ.NONE;
	public MOVE_DIR box_dir 			= MOVE_DIR.IDLE;
	public BOX_STATE box_state			= BOX_STATE.IDLE;

	public Sprite[] box_sprites;
	public SpriteRenderer spriteRenderer;
	// Private
	public int accelCount = 0;
	private float moveSpeedBackUp = 0;

	private Vector3 movement;

	private Rigidbody2D rigidbody;
	private Collider2D collider;

	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();

		moveSpeedBackUp = moveSpeed;
	}

	void FixedUpdate()
	{
		OnMove();
	}
	// 박스의 움직임 관련 함수 (IDLE, FLYING, HOLDING)
	void OnMove()
	{
		switch(box_state)
		{
			case BOX_STATE.IDLE: // 박스가 스폰되서 떨어질 때
			{
				// 중력 물리값 적용, 콜라이더 충돌 활성화
				collider.isTrigger = false;
				rigidbody.isKinematic = false;				
			} break;
			case BOX_STATE.FLYING:
			{
				// 방향 확인 후 벡터 설정
				switch(box_dir)
				{
					case MOVE_DIR.RIGHT:
					{
						movement = Vector3.right * moveSpeed;
					} break;
					case MOVE_DIR.LEFT:
					{
						movement = Vector3.left * moveSpeed;
					} break;					
				}
				// 중력 물리값 비적용, 콜라이더 충돌 활성화
				collider.isTrigger = true;
				rigidbody.isKinematic = true;
				rigidbody.velocity = movement;
			} break;
			case BOX_STATE.HOLDING:
			{
				// 가속 횟수 초기화, 초기 속도로 롤백
				accelCount = 0;
				moveSpeed = moveSpeedBackUp;
				// 중력 물리값 비적용, 콜라이더 충돌 비활성화
				collider.isTrigger = true;
				rigidbody.isKinematic = true;
			} break;
		}
	}
	// 박스를 공격할 때 외부에서 불러낼 함수 (RIGHT, LEFT)
	public void OnHit(TOUNCHED_OBJ other_touched_obj, MOVE_DIR other_move_dir)
	{
		touched_obj = other_touched_obj; // 만진 오브젝트 정보 저장
		box_state = BOX_STATE.FLYING;	 // 박스 상태 [날라감]

		switch(touched_obj)
		{
			case TOUNCHED_OBJ.PLAYER:
				spriteRenderer.sprite = box_sprites[0];
				break;
			case TOUNCHED_OBJ.ENEMY:
				spriteRenderer.sprite = box_sprites[1];
				break;
		}

		if (box_dir == MOVE_DIR.IDLE)
			box_dir = other_move_dir;
		// 박스 방향 전환
		if (box_dir == MOVE_DIR.LEFT)
			box_dir = MOVE_DIR.RIGHT;
		else if (box_dir == MOVE_DIR.RIGHT)
			box_dir = MOVE_DIR.LEFT;

		if (accelCount < 3)
		{
			moveSpeed += accelAmount;
			accelCount++;
		}
	}
}
