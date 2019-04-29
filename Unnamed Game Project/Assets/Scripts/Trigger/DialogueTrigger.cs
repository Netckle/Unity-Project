using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    private int[]   appliedDialogueRange;

    public int[]    dialogueRangeBefore;
    public int[]    dialogueRangeAfter;

    public TYPE     dialogueType;

    public int      key;
    private bool    isChanged = false;
    public int imageNum;

    private bool canTalk = false;

    void Start()
    {
        appliedDialogueRange = dialogueRangeBefore;
    }

    void Update()
    {
        if (!isChanged && GameManager.Instance().questM.CheckQuestState(key % 10))
        {
            GameManager.Instance().stageM.generatedStages[GameManager.Instance().stageM.currentStageIndex].GetComponent<Stage>().SpawnBox();

            appliedDialogueRange = dialogueRangeAfter;
            isChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance().questM.ClearQuest(key % 10);
        }

        if (canTalk && Input.GetButtonDown("Interact"))
        {
            Player.Instace().isTalking = true;

            GameManager.Instance().dialgoueM.
            StartDialogue(this.gameObject, GameObject.Find("Load CSV").GetComponent<LoadCSV>().GetData("Dialogue"), appliedDialogueRange, dialogueType, key, imageNum);  

            canTalk = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canTalk = true;
        }       
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canTalk = false;
        }
    }
}
