using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp2 : MonoBehaviour
{
    public int normalStartIndex = 0;
    public int clearStartIndex = 0;
    public int questIndex = 0;
    public string collideObjectN = "";

    private temp dialogueAndQuestM = null;
    private bool IsComplete = false;
    private int count = 0;

    void Start()
    {
        dialogueAndQuestM = GameObject.FindGameObjectWithTag("DialogueAndQuestM").GetComponent<temp>();
    }

    void Update()
    {
        if (dialogueAndQuestM.GetQuestComplete(questIndex))
            IsComplete = true;
        else
            IsComplete = false;

        if (Input.GetKeyDown(KeyCode.P)) dialogueAndQuestM.DisplayNextSentence();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == collideObjectN)
        {
            count++;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == collideObjectN)
        {            
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (count > 1)
                {
                    dialogueAndQuestM.CompleteQuest(questIndex);
                }

                if (IsComplete)
                {
                    TriggerDialogue(clearStartIndex);
                }
                else if (!IsComplete)
                {
                    TriggerDialogue(normalStartIndex);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == collideObjectN)
        {
        }            
    }

    public void TriggerDialogue(int index)
    {
        dialogueAndQuestM.StartDialogue(index);
    }
}
