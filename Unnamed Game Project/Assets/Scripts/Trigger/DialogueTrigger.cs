using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    //private DialogueManager dialogueManager;
    public int[] dialogueIndexRange;
    public TYPE dialogueType;

    //private QuestManager questManager;
    public int questIndex;

    public bool isEnd = false;

    void Update()
    {
        if (isEnd && SceneManager.GetActiveScene().name == "Dungeon Scene Play")
        {
            StageManager.Instance().generatedStages[StageManager.Instance().currentStageIndex].GetComponent<Stage>().SpawnBox();
            isEnd = false;
        }        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            other.gameObject.GetComponent<Player>().isTalking = true;

            DialogueManager.Instance().
            StartDialogue(this.gameObject, GameObject.Find("Load CSV").GetComponent<LoadCSV>().GetData("대화"), dialogueIndexRange, dialogueType);                       
        }        
    }
}
