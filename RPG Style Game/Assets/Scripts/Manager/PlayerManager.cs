using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{   
    public static PlayerManager instance; 

    public string current_map_name; 
    public bool transferMap = true;    

    private AudioManager the_audio;
    public string[] walk_sound = new string[4];    

    public float run_speed;
    private float apply_run_speed;

    public bool can_move = true;
    public bool do_not_move = false;
    private bool apply_run_flag = false;    
    
    private bool attacking = false;
    public float attack_delay;
    private float current_attack_delay;

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
        box_collider = GetComponent<BoxCollider2D>();            
        the_audio = FindObjectOfType<AudioManager>();          
    }        

    IEnumerator MoveCoroutine()
    {
        while (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && !do_not_move && !attacking)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                apply_run_speed = run_speed;
                apply_run_flag = true;
            }
            else
            {
                apply_run_speed = 0;
                apply_run_flag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if (vector.x != 0)
                vector.y = 0;

            if (vector.y != 0)
                vector.z = 0;

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);       
            
            bool checkCollisionFlag = base.CheckCollision();            
            if (checkCollisionFlag) 
                break;   
                        
            animator.SetBool("Walking", true);            
            
            int temp = Random.Range(1, 5);
            switch (temp)
            {
                case 1:
                    the_audio.Play(walk_sound[0]); 
                    break;
                case 2:
                    the_audio.Play(walk_sound[1]); 
                    break;
                case 3:
                    the_audio.Play(walk_sound[2]); 
                    break;
                case 4:
                    the_audio.Play(walk_sound[3]); 
                    break;
            }      

            box_collider.offset = new Vector2(vector.x * 0.7f * speed * walk_count, vector.y * 0.7f * speed * walk_count);   

            while(current_walk_count < walk_count)
            {
                if(vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + apply_run_speed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + apply_run_speed), 0);
                }

                if (apply_run_flag) 
                {
                    current_walk_count++;
                }

                current_walk_count++;

                if (current_walk_count == 12)
                    box_collider.offset = Vector2.zero;

                yield return new WaitForSeconds(0.01f);                
            }
            current_walk_count = 0;       
        }   

        animator.SetBool("Walking", false);
        can_move = true;  
    }

    void Update()
    {      
        // 이동.          
        if (can_move && !do_not_move && !attacking)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                can_move = false;
                StartCoroutine(MoveCoroutine());        
            }
        }  

        // 공격.
        if (!do_not_move && !attacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                current_attack_delay = attack_delay;
                attacking = true;
                animator.SetBool("Attacking", true);
            }
        }    

        // 공격 딜레이만큼의 시간이 지나면 다시 공격 가능.
        if (attacking)
        {
            current_attack_delay -= Time.deltaTime;

            if (current_attack_delay <= 0)
            {
                animator.SetBool("Attacking", false);
                attacking = false;
            }
        }  
    }
}
