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
    private int count = 0;

    void Start()
    {
        dialogueAndQuestM = GameObject.FindGameObjectWithTag("DialogueAndQuestM").GetComponent<temp>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) dialogueAndQuestM.DisplayNextSentence();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == collideObjectN)
        {
            
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == collideObjectN && Input.GetKeyDown(KeyCode.O))
        {            
            if (count == 0)
            {
                //dialogueAndQuestM.UnlockQuest(questIndex); Debug.Log(questIndex + "번 퀘스트 언락됨.");
            }
            else if (count == 1)
            {
                dialogueAndQuestM.CompleteQuest(questIndex); Debug.Log(questIndex + "번 퀘스트 클리어함.");
            }

            if (dialogueAndQuestM.GetQuestComplete(questIndex))
            {
                TriggerDialogue(clearStartIndex);
            }
            else
            {
                TriggerDialogue(normalStartIndex);
            }       
            
            ++count;            
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
