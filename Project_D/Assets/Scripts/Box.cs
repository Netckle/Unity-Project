using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BOXSTATE
{
    IDLE,
    FALLING,
    THROWING
}

public enum BOXDIR
{
    RIGHT,
    LEFT
}

public class Box : MonoBehaviour
{
    private int accelCount = 0;

    public float damage;
    public float moveSpeed;
    public float fallSpeed;
    public float accelSpeed;

    private float preMoveSpeed = 0;

    public BOXSTATE state = BOXSTATE.FALLING;
    public BOXDIR dir = BOXDIR.RIGHT;

    private Rigidbody2D rigid;
    private Collider2D collide;

    private Vector3 movement;

    public Sprite[] player_sprite;

    public SpriteRenderer box;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        collide = GetComponent<Collider2D>();

        preMoveSpeed = moveSpeed;
    }

    void Update()
    {
        switch(state)
        {
            case BOXSTATE.IDLE:
                {
                    moveSpeed = preMoveSpeed;
                    accelCount = 0;
                    collide.isTrigger = true;
                    rigid.isKinematic = true;
                }
            break;
            case BOXSTATE.FALLING:
                {
                    collide.isTrigger = true;
                    rigid.isKinematic = true;

                    rigid.velocity = new Vector2(0, -transform.localScale.y) * fallSpeed;
                }
                break;
            case BOXSTATE.THROWING:
                {
                    collide.isTrigger = true;
                    rigid.isKinematic = true;

                    if (dir == BOXDIR.RIGHT)
                        rigid.velocity = new Vector2(transform.localScale.x, 0) * moveSpeed;
                    else if (dir == BOXDIR.LEFT)
                        rigid.velocity = new Vector2(-transform.localScale.x, 0) * moveSpeed;
                    
                }
                break;
        }
    }

    public void ChangeMoveDir()
    {
        if (dir == BOXDIR.RIGHT)
            dir = BOXDIR.LEFT;

        else if (dir == BOXDIR.LEFT)
            dir = BOXDIR.RIGHT;
    }

    public void AccelBoxSpeed()
    {
        if (accelCount >= 3)
            return;

        moveSpeed += accelSpeed;
        accelCount += 1;
    }

    public float GetRigid()
    {
        float returnValue = rigid.velocity.x;

        return returnValue;
    }

    public void ChangeSprite(GameObject obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            box.sprite = player_sprite[0];
        }
    }
}
