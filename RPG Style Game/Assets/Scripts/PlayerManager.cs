using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{
    static public PlayerManager instance;
    public string currentMapName; // transferMap 스크립트에 있는 transferMapName 변수의 값을 저장.

    public string walkSound_1;
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;

    private AudioManager theAudio;

    public float runSpeed;
    private float applyRunSpeed;

    private bool canMove = true;
    private bool applyRunFlag = false;

    private RaycastHit2D hit;
    private Vector2 startPos;
    private Vector2 endPos;

    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider2D>();            
            theAudio = FindObjectOfType<AudioManager>();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }        
    }    

    IEnumerator MoveCoroutine()
    {
        while(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
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

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if (vector.x != 0)
                vector.y = 0;

            if (vector.y != 0)
                vector.z = 0;

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            
            // A지점, B지점
            // 레이저
            // hit = Null;
            // hit = 방해물

            //Vector2 start = transform.position; // A지점. 캐릭터의 현재 위치 값
            //Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); // B지점. 캐릭터가 이동하고자 하는 위치 값

            startPos = transform.position;
            endPos = startPos + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

            hit = Physics2D.Linecast(startPos, endPos, layerMask);
            
            boxCollider.enabled = true;

            if (hit.transform != null)           
                break;

            animator.SetBool("Walking", true);            
            
            int temp = Random.Range(1, 5);
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound_1);
                    break;
                case 2:
                    theAudio.Play(walkSound_2);
                    break;
                case 3:
                    theAudio.Play(walkSound_3);
                    break;
                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }     

            // theAudio.SetVolumn(walkSound_2, 0.5f); 2번 사운드 소리를 반으로 줄임      

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
                {
                    currentWalkCount++;
                }

                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);                
            }

            currentWalkCount = 0;       
        }   

        animator.SetBool("Walking", false);
        canMove = true;  
    }

    void Update()
    {        
        Debug.DrawLine(startPos, endPos);

        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());        
            }
        }        
    }
}
