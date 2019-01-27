using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private MOVE_DIR enemyDir = MOVE_DIR.LEFT;

    private bool isAttacked = true;
    private bool isGrabbed = true;

    public CameraShake cameraShake;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!isAttacked && !isGrabbed)
            return;

        if (isAttacked && collider.gameObject.tag == "Box")
        {
            Debug.Log("Box와 충돌함");

            BOX boxData = collider.gameObject.GetComponent<BOX>();

            if (boxData.box_dir != enemyDir)
            {
                cameraShake.StartCoroutine("Shake");
                boxData.OnHit(TOUNCHED_OBJ.ENEMY, enemyDir);
            }
        }
    }
}
