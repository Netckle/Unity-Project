using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MovingObject
{
    public float attack_delay;
    public string attack_sound;

    public float intermediate_MWT; // Move Wait Time 대기시간.
    private float current_intermediate_MWT;
    
    private Vector2 player_pos; // 플레이어의 좌표값.
    
    private int random_int;
    private string direction;

    void Start()
    {
        queue = new Queue<string>();
        current_intermediate_MWT = intermediate_MWT;
    }

    void Update()
    {
        current_intermediate_MWT -= Time.deltaTime;

        if (current_intermediate_MWT <= 0)
        {
            current_intermediate_MWT = intermediate_MWT;

            if (NearPlayer())
            {
                Flip();
                return;
            }

            RandomDirection();

            if (base.CheckCollision())
            {
                queue.Clear();
                return;
            }
            base.Move(direction);
        }
    }   

    private void Flip()
    {
        Vector3 flip = transform.localScale;

        if (player_pos.x > this.transform.position.x)
            flip.x = -1.0f;
        else
            flip.x = 1.0f;

        this.transform.localScale = flip;

        animator.SetTrigger("Attack");
        StartCoroutine(WaitAndAttackCoroutine());
    }

    IEnumerator WaitAndAttackCoroutine()
    {
        yield return new WaitForSeconds(attack_delay);

        AudioManager.instance.Play(attack_sound);

        if (NearPlayer())
            PlayerStat.instance.DamagedByEnemy(GetComponent<EnemyStat>().attack);
    }

    private bool NearPlayer()
    {
        player_pos = PlayerManager.instance.transform.position;

        if (Mathf.Abs(Mathf.Abs(player_pos.x) - Mathf.Abs(this.transform.position.x)) <= speed * walk_count * 1.01f)
        {
            if (Mathf.Abs(Mathf.Abs(player_pos.y) - Mathf.Abs(this.transform.position.y)) <= speed * walk_count * 0.5f)
            {
                return true;
            }
        }

        if (Mathf.Abs(Mathf.Abs(player_pos.y) - Mathf.Abs(this.transform.position.y)) <= speed * walk_count * 1.01f)
        {
            if (Mathf.Abs(Mathf.Abs(player_pos.x) - Mathf.Abs(this.transform.position.x)) <= speed * walk_count * 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    private void RandomDirection()
    {
        vector.Set(0, 0, vector.z);

        random_int = Random.Range(0, 4);
        switch(random_int)
        {
            case 0:
                vector.y = 1.0f;
                direction = "UP";
                break;
            case 1:
                vector.y = -1.0f;
                direction = "DOWN";
                break;
            case 2:
                vector.x = 1.0f;
                direction = "RIGHT";
                break;
            case 3:
                vector.x = -1.0f;
                direction = "LEFT";
                break;
        }
    }
}
