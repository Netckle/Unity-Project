using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public string characterName;

    public float            speed;
    public int              walkCount;
    protected int           currentWalkCount;

    private bool            notCoroutine = false;

    protected Vector3       vector;

    public Queue<string>    queue;

    public BoxCollider2D    boxCollider;
    public LayerMask        layerMask;
    public Animator         animator;

    public bool             canMove = true;

    public bool CheckCollision()
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, layerMask);
        boxCollider.enabled = true;

        // DEBUG
        Debug.DrawLine(startPos, endPos, Color.red);

        if (hit.transform != null) return true;
        else return false;
    }

    public void Move(string _dir, int _frequency = 5)
    {
        if (!canMove) return;

        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    }
    
    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while(queue.Count != 0)
        {
            // _frequency에 따라 움직이는 빈도 결정
            switch(_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4.0F); break;
                case 2:
                    yield return new WaitForSeconds(3.0F); break;
                case 3:
                    yield return new WaitForSeconds(2.0F); break;
                case 4:
                    yield return new WaitForSeconds(1.0F); break;
                case 5:
                    break;
            }

            string direction = queue.Dequeue();
            vector.Set(0, 0, vector.z);

            // direction에 따라 벡터 결정
            switch(direction)
            {
                case "UP":
                    vector.y = 1f; break;
                case "DOWN":
                    vector.y = -1f; break;
                case "RIGHT":
                    vector.x = 1f; break;
                case "LEFT":
                    vector.x = -1f; break;
            }

            // 벡터에 따라 그려질 스프라이트 결정 
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            // 충돌 처리
            while(true)
            {
                bool checkCollisonFlag = CheckCollision();

                if (checkCollisonFlag)
                {
                    animator.SetBool("Walking", false);
                    yield return new WaitForSeconds(1.0F);
                }
                else break;
            }

            animator.SetBool("Walking", true);

            // 콜라이더를 앞으로 내밀어 다른 MovingObject와의 충돌을 방지함. 
            boxCollider.offset = new Vector2(vector.x * 0.7F * speed * walkCount, vector.y * 0.7F * speed * walkCount);

            // 실제 이동
            while(currentWalkCount < walkCount)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0);

                currentWalkCount++;

                if (currentWalkCount == 12)                
                    boxCollider.offset = Vector2.zero;
                
                yield return new WaitForSeconds(0.01F);
            }
            currentWalkCount = 0;

            if (_frequency != 5)
                animator.SetBool("Walking", false);
        }
        animator.SetBool("Walking", false);
        notCoroutine = false;
    }
}
