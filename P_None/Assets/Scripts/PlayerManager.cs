using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{
    public static PlayerManager instance;

    public string               currentMapName;

    public string[]             walkSound = new string[4];
    // private AudioManager theAudio;

    public float                runSpeed;
    private float               applyRunSpeed;
    private bool                applyRunFlag = false;

#region Singleton
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
#endregion Singleton

    void Start()
    {
        queue       = new Queue<string>();
        animator    = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        // theAudio = GetComponent<AudioManager>();
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
    }

    // 플레이어 전용 이동 코루틴
    IEnumerator MoveCoroutine()
    {
        while(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // LEFTSHIFT 누르면 대쉬함
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            // 키보드 입력에 따라 벡터 결정
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            // 벡터가 대각선이 되는 것을 방지
            if (vector.x != 0) vector.y = 0;
            if (vector.y != 0) vector.x = 0;

            // 벡터에 따라 그려질 스프라이트 결정
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            // 충돌 처리
            bool checkCollision = base.CheckCollision();
            if (checkCollision) break;

            animator.SetBool("Walking", true);

            // 발걸음 소리 발생
            int temp = Random.Range(1, 5); // 1 ~ 4
            /*
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound[0]); break;
                case 2:
                    theAudio.Play(walkSound[1]); break;
                case 3:
                    theAudio.Play(walkSound[2]); break;
                case 4:
                    theAudio.Play(walkSound[3]); break;
            }
            */

            // 콜라이더를 앞으로 내밀어 다른 MovingObject와의 충돌을 방지함. 
            boxCollider.offset = new Vector2(vector.x * 0.7F * speed * walkCount, vector.y * 0.7F * speed * walkCount);

            // 실제 이동
            while(currentWalkCount < walkCount)
            {
                if(vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                if (applyRunFlag) 
                    currentWalkCount++;

                currentWalkCount++;

                if (currentWalkCount == 12)
                {
                    boxCollider.offset = Vector2.zero;
                }                   
                
                yield return new WaitForSeconds(0.01F);
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }
}
