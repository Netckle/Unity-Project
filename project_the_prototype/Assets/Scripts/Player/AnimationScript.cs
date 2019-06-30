using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement move;
    private Collision coll;

    [HideInInspector]
    public SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<Collision>();
        move = GetComponentInParent<PlayerMovement>();
        sr   = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        anim.SetBool("onGround"   , coll.onGround);
        anim.SetBool("canMove"    , move.canMove);
    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);      
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void Flip(int side)
    {
        bool state = (side == 1) ? false : true;

        //sr.flipX = state;

        switch (state)
        {
            case false:
                move.transform.localScale = new Vector3(1, 1, 1);
                break;
            case true:
                move.transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }
}
