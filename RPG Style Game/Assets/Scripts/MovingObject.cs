using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public LayerMask layerMask;

    public float speed;

    private Vector3 vector;

    public float runSpeed;
    private float applyRunSpeed;

    public int walkCount;
    private int currentWalkCount;

    // speed = 2.4, walkCount = 20
    // 2.4 * 20 = 48 pixel
    // while 
    // currentWalkCount += 1, 20   

    private bool canMove = true;
    private bool applyRunFlag = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private RaycastHit2D hit;
    private Vector2 startPos;
    private Vector2 endPos;

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
