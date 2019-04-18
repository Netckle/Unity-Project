using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //StageManager stageManager;

    void Start()
    {
        //stageManager = FindObjectOfType<StageManager>().GetComponent<StageManager>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            //stageManager.MoveNextRoom();
            StageManager.Instance().MoveNextRoom();
        }
    }
}
