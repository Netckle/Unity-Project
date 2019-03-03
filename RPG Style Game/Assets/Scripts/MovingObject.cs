using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어와 NPC의 부모 클래스
public class MovingObject : MonoBehaviour
{
    public string character_name;

    public float speed;
    public int walk_count;
    protected int current_walk_count;

    private bool not_coroutine = false;

    protected Vector3 vector;

    public Queue<string> queue;
    
    public BoxCollider2D box_collider;
    public LayerMask layer_mask;
    public Animator animator;

    public bool CheckCollision()
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + new Vector2(vector.x * speed * walk_count, vector.y * speed * walk_count);

        box_collider.enabled = false; // Ray를 쏘는 자기자신이 맞을 수 있기 때문에 꺼야함.
        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, layer_mask);   
        box_collider.enabled = true;        

        Debug.DrawLine(startPos, endPos, Color.red);

        if (hit.transform != null) return true;      
        else return false;
    }
    
    public void Move(string _dir, int _frequency = 5)
    {            
        queue.Enqueue(_dir);
        if (!not_coroutine)
        {
            not_coroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    }

    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {    
        while(queue.Count != 0)
        {
            switch(_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    yield return new WaitForSeconds(1f);
                    break;
                case 5:
                    break;
            }

            string direction = queue.Dequeue();          
            vector.Set(0, 0, vector.z);

            switch(direction)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIGHT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = -1f;
                    break;
            }

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            while(true)
            {
                bool checkCollisionFlag = CheckCollision();
                
                if (checkCollisionFlag) 
                {
                    animator.SetBool("Walking", false);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break;
                }
            }           

            animator.SetBool("Walking", true);

            box_collider.offset = new Vector2(vector.x * 0.7f * speed * walk_count,
            vector.y * 0.7f * speed * walk_count);

            while(current_walk_count < walk_count)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0);

                current_walk_count++;

                if (current_walk_count == walk_count * 0.5f + 2)
                    box_collider.offset = Vector2.zero;

                yield return new WaitForSeconds(0.01f);                
            } 

            current_walk_count = 0;

            if (_frequency != 5)
                animator.SetBool("Walking", false); 
        }         
        animator.SetBool("Walking", false);  
        not_coroutine = false;
    }
}
