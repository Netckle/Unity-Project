using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Box" && Input.GetKeyDown(KeyCode.R))
        {
            Box collidedBox = collision.gameObject.GetComponent<Box>();

            collidedBox.state = BOXSTATE.THROWING;


            collidedBox.ChangeMoveDir();
            collidedBox.ChangeSprite(this.gameObject);
        }
    }
}


