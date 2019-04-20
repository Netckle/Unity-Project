using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    private int[] appliedDialogueRange;

    public int[] dialogueRangeBefore;
    public int[] dialogueRangeAfter;

    public TYPE dialogueType;

    public int questIndex;

    private bool isChanged = false;

    void Start()
    {
        appliedDialogueRange = dialogueRangeBefore;
    }

    void Update()
    {
        if (!isChanged && QuestManager.Instance().CheckQuestState(questIndex))
        {
            StageManager.Instance().generatedStages[StageManager.Instance().currentStageIndex].GetComponent<Stage>().SpawnBox();

            appliedDialogueRange = dialogueRangeAfter;
            Debug.Log("바꼈다구");
            isChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            QuestManager.Instance().ClearQuest(questIndex);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            Player.Instace().isTalking = true;

            DialogueManager.Instance().
            StartDialogue(this.gameObject, GameObject.Find("Load CSV").GetComponent<LoadCSV>().GetData("Dialogue"), appliedDialogueRange, dialogueType, questIndex);                       
        }        
    }
}
