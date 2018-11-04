using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDelete : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            Destroy(collision.gameObject);
        }
    }
}
