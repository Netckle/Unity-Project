using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public Animator animator;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }        
    }
}
