using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;

    private DialogueManager the_dialogue;

    public bool can_talk = false;

    void Start()
    {
        the_dialogue = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            can_talk = true;
        }
    } 

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            can_talk = false;
        }
    }

    void Update()
    {
        if (can_talk && Input.GetKeyDown(KeyCode.Z))
        {
            can_talk = false;
            the_dialogue.ShowDialogue(dialogue);
        }
    }
}
