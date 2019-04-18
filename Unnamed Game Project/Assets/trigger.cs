using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    private bool flag = true;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            ChangeScene.Instace().CardSystemSwitch(flag);

            switch(flag)
            {
                case true:
                    flag = false;
                    break;
                case false:
                    flag = true;
                    break;
            }
        }
    }
}
