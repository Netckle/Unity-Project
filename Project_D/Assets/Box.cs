using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BOXSTATE
{
    IDLE,
    FALLING,
    THROWING
}

public class Box : MonoBehaviour
{
    public int damage;

    public BOXSTATE state = BOXSTATE.FALLING;
    private Rigidbody2D rigid;
    private Collider2D collide;

    private Vector2 movement;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;

        collide = GetComponent<Collider2D>();
    }

    void Update()
    {
        switch(state)
        {
            case BOXSTATE.IDLE:
                {
                    collide.isTrigger = true;
                    rigid.isKinematic = true;
                }
            break;
            case BOXSTATE.FALLING:
                {
                    collide.isTrigger = true;
                    transform.Translate(Vector2.down * 0.1F);
                    rigid.isKinematic = true;
                }
                break;
            case BOXSTATE.THROWING:
                {
                    collide.isTrigger = false;
                    rigid.isKinematic = false;
                }
                break;
        }
    }
}
