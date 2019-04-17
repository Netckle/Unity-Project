using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public int[] dialogueIndexRange;
    public TYPE dialogueType;

    private QuestManager questManager;
    public int questIndex;

    void Start()
    {
        dialogueManager = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        questManager = GameObject.Find("Quest Manager").GetComponent<QuestManager>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            other.gameObject.GetComponent<Player>().isTalking = true;
            dialogueManager.StartDialogue(GameObject.Find("Load CSV").GetComponent<LoadCSV>().GetData("대화"), dialogueIndexRange, dialogueType);


                questManager.StartQuest(questIndex);
            
        }

        
    }
}
