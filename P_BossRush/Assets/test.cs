using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public int start;
    public int end;

    public GameObject slime;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //DialogueManager.instance.StartDialogue(JsonManager.instance.Load<Dialogue>(), start, end);
        }
    }
}
