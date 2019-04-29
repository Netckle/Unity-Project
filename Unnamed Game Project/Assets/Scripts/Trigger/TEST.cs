using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            Debug.Log("들어왔다.");
            GameManager.Instance().changeSceneM.OpenCardPanel(true);
        }
    }
}
