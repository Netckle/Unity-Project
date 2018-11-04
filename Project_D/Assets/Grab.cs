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

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isGrabbed)
            {
                Physics2D.queriesStartInColliders = false;
                hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

                if (hit.collider == null || hit.collider.tag != "Box")
                    return;

                if (hit.collider != null && hit.collider.tag == "Box") ;
                {
                    hit.collider.gameObject.GetComponent<Box>().state = BOXSTATE.IDLE;
                    isGrabbed = true;
                }
            }
            else if (Physics2D.OverlapPoint(holdPoint.position, notGrabbed))
            {
                isGrabbed = false;

                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    hit.collider.gameObject.GetComponent<Box>().state = BOXSTATE.THROWING;
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwForce;
                }
            }
        }
        if (isGrabbed)
            hit.collider.gameObject.transform.position = holdPoint.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
    }
}
