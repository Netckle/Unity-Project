﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{   
    private bool canGo = false;

    void Start()
    {
        GameManager.Instance().objectM.portal = this.gameObject;
        GameManager.Instance().objectM.portal.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && canGo)
        {
            GameManager.Instance().stageM.MoveToNextRoom();
            canGo = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {        
            canGo = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canGo = false;
        }
    }
}
