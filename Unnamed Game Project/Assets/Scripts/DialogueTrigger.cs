using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public int[] dialogueIndexRange;
    public TYPE dialogueType;

    private QuestManager questManager;
    public int questIndex;

    public bool isEnd = false;

    void Start()
    {
        dialogueManager = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        questManager = GameObject.Find("Quest Manager").GetComponent<QuestManager>();
    }

    void Update()
    {
        if (isEnd && SceneManager.GetActiveScene().name == "던전 플레이 단계")
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
            dialogueManager.StartDialogue(this.gameObject, GameObject.Find("Load CSV").GetComponent<LoadCSV>().GetData("대화"), dialogueIndexRange, dialogueType);

                       
        }        
    }
}
