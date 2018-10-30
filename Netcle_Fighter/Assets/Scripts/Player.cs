using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION
{
    NONE = 0,

    UP,
    DOWN,
    LEFT,
    RIGHT,

    UP_LEFT,
    UP_RIGHT,
    DOWN_LEFT,
    DOWN_RIGHT
}

public class Player : MonoBehaviour
{
    public BattleChip normalAttackChip;

    public float hp = 100;

    private Vector3 movement;
    private Vector3 rayDir;

    private DIRECTION curBlockType = DIRECTION.NONE;

    private bool dir_up     = false;
    private bool dir_down   = false;
    private bool dir_left   = false;
    private bool dir_right  = false;

    void Start()
    {
        rayDir = new Vector3(0, -1, 0);
        normalAttackChip.Instantiate();        
    }

    void Update()
    {
        Move();
        NormalAttack();
        RaycastHit hit;

        int mask = 1 << 9;
        mask = ~mask;

        if (Physics.Raycast(transform.position, rayDir, out hit, 1.0F, mask))
        {
            if (hit.transform.tag.Equals("Floor"))
            {
                Block block = hit.transform.GetComponent<Block>();

                if (block != null)
                    curBlockType = block.blockType;
            }
        }

        CheckCurBlockType();
    }

    void CheckCurBlockType()
    {
        switch(curBlockType)
        {
            case DIRECTION.UP:
                {
                    dir_up = true; dir_down = false; dir_left = false; dir_right = false;
                }
                break;
            case DIRECTION.DOWN:
                {
                    dir_up = false; dir_down = true; dir_left = false; dir_right = false;
                }
                break;
            case DIRECTION.LEFT:
                {
                    dir_up = false; dir_down = false; dir_left = true; dir_right = false;
                }
                break;
            case DIRECTION.RIGHT:
                {
                    dir_up = false; dir_down = false; dir_left = false; dir_right = true;
                }
                break;

            case DIRECTION.UP_LEFT:
                {
                    dir_up = true; dir_down = false; dir_left = true; dir_right = false;
                }
                break;
            case DIRECTION.UP_RIGHT:
                {
                    dir_up = true; dir_down = false; dir_left = false; dir_right = true;
                }
                break;
            case DIRECTION.DOWN_LEFT:
                {
                    dir_up = false; dir_down = true; dir_left = true; dir_right = false;
                }
                break;
            case DIRECTION.DOWN_RIGHT:
                {
                    dir_up = false; dir_down = true; dir_left = false; dir_right = true;
                }
                break;
        }
    }

    // 이동 함수
    void Move()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !dir_left)
            movement = new Vector3(0, 0, 1);
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !dir_right)
            movement = new Vector3(0, 0, -1);
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !dir_up)
            movement = new Vector3(1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !dir_down)
            movement = new Vector3(-1, 0, 0);

        transform.position += movement;

        movement = Vector3.zero;
    }

    // 일반 공격 함수
    void NormalAttack()
    {
        if (Input.GetButtonDown("Jump"))
        {
            normalAttackChip.AttachToPlayerPos(this.gameObject);

            StartCoroutine(normalAttackChip.executeAct());
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + rayDir);
    }
}
