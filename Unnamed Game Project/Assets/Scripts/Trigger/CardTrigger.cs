using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTrigger : MonoBehaviour
{
    public bool isOpen = true;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            GameManager.Instance().canvasM.ControllPanel(HUDTYPE.CARD, isOpen);

            switch(isOpen)
            {
                case true:
                    isOpen = false;
                    break;
                case false:
                    isOpen = true;
                    break;
            }
        }
    }
}
