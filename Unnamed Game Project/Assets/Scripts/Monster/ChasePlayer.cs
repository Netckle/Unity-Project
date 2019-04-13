using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{   
    private MonsterMovement monster;

    void Start()
    {
        monster = GetComponentInParent<MonsterMovement>();
    } 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (monster.creatureType == 0)
            return;

        if (other.gameObject.tag == "Player")
        {
            monster.traceTarget = other.gameObject;

            monster.StopAllCoroutines();
        } 
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (monster.creatureType == 0)
            return;

        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                monster.movementFlag = 2;   
            }
            if (other.gameObject.transform.position.x < transform.position.x)
            {
                monster.movementFlag = 1; 
            }

            monster.isTracing = true;
            monster.animator.SetBool("isMoving", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (monster.creatureType == 0)
            return;

        if (other.gameObject.tag == "Player")
        {
            monster.isTracing = false;

            //monster.StopAllCoroutines();
            monster.StartCoroutine("ChangeMovement");
        }
    }
}
