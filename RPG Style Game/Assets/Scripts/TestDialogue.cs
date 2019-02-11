using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    [SerializeField]
    public Dialogue _dialogue;

    private DialogueManager theDM;

    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            theDM.ShowDialogue(_dialogue);
        }
    }    
}
