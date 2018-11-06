using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool isGrabbed = false;
    RaycastHit2D hit;

    public float distance = 2.0F;
    public Transform holdPoint;
    public float throwForce;
    public LayerMask notGrabbed;

    void Update()
    {       



        Attack();

        if (isGrabbed)
            hit.collider.gameObject.transform.position = holdPoint.position;
    }

    void Attack()
    {
        Physics2D.queriesStartInColliders = false;
        hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hit.collider == null || hit.collider.tag != "Box")
                return;

            if (hit.collider != null && hit.collider.tag == "Box") ;
            {
                // 박스가 FALLING 이든, IDLE 이든 상관없이 날라가게 한다.
                hit.collider.gameObject.GetComponent<Box>().state = BOXSTATE.THROWING;
                hit.collider.gameObject.GetComponent<Box>().ChangeMoveDir();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
    }
}
