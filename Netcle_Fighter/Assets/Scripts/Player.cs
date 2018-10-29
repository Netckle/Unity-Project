using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BattleChip normalAttackChip;
    //public BattleChip[] skillAttackChip;

    public float hp = 100;

    private Vector3 movement;

    void Start()
    {
        normalAttackChip.Instantiate();        
    }

    void Update()
    {
        Move();
        NormalAttack();
    }

    // 이동 함수
    void Move()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            movement = new Vector3(0, 0, 1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            movement = new Vector3(0, 0, -1);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            movement = new Vector3(1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
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
}
