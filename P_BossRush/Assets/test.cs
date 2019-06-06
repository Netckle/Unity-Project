using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            DialogueManager.instance.StartDialogue(JsonManager.instance.Load<Dialogue>(), 0, 3);
        }
    }
}
