using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{   
    public static PlayerManager instance; 

    public string currentMapName; 

    public string[] walkSound = new string[4];

    private AudioManager theAudio;

    public float runSpeed;
    private float applyRunSpeed;

    public bool canMove = true;
    private bool applyRunFlag = false;

    public bool transferMap = true;

    public bool notMove = false;
    private bool attacking = false;
    public float attackDelay;
    private float currentAttackDelay;

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
        queue = new Queue<string>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();            
        theAudio = FindObjectOfType<AudioManager>();           
    }        

    IEnumerator MoveCoroutine()
    {
        while(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && !notMove && !attacking)
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
            
            bool checkCollisionFlag = base.CheckCollision();            
            if (checkCollisionFlag) break;   
                        
            animator.SetBool("Walking", true);            
            
            int temp = Random.Range(1, 5);
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

            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount,
            vector.y * 0.7f * speed * walkCount);   

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

                if (currentWalkCount == 12)
                    boxCollider.offset = Vector2.zero;
                yield return new WaitForSeconds(0.01f);                
            }

            currentWalkCount = 0;       
        }   

        animator.SetBool("Walking", false);
        canMove = true;  
    }

    void Update()
    {                
        if (canMove && !notMove && !attacking)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());        
            }
        }  

        if (!notMove && !attacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentAttackDelay = attackDelay;
                attacking = true;
                animator.SetBool("Attacking", true);
            }
        }    

        if (attacking)
        {
            currentAttackDelay -= Time.deltaTime;

            if (currentAttackDelay <= 0)
            {
                animator.SetBool("Attacking", false);
                attacking = false;
            }
        }  
    }
}
