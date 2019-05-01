using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{    
    public int[] dialogueRangeFirst;
    public int[] dialogueRangeAfter;

    public bool haveQuest;

    public DIALOGUEBOX type;

    public int eventKey;
    public int cutsceneNum;    
    
    private int eventIndex;
    private int[] appliedDialogueRange;
    private bool firstDialogueFinished = false;
    private bool canTalk = false;

    void Start()
    {
        eventIndex = (eventKey % 10);
        appliedDialogueRange = dialogueRangeFirst;
    }

    void Update()
    {
        // [임시] 퀘스트를 강제로 클리어 시킨다.
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance().questM.Clear(eventKey % 10);
        }

        // 퀘스트를 클리어
        if (!firstDialogueFinished && GameManager.Instance().questM.CheckState(eventKey % 10))
        {
            GameManager.Instance().stageM.generatedStages[GameManager.Instance().stageM.currentStageIndex].GetComponent<Stage>().SpawnBox();

            appliedDialogueRange = dialogueRangeAfter;
            firstDialogueFinished = true;
        }

        
        // 대화가 가능하고 상호작용 버튼을 눌렀을 경우.
        if (canTalk && Input.GetButtonDown("Interact"))
        {
            canTalk = false;
            Player.Instace().isTalking = true;

            GameManager.Instance().dialgoueM.
            StartDialogue(this.gameObject, GameManager.Instance().loadCSV.GetData(DATATYPE.DIALOGUE), appliedDialogueRange, DIALOGUEBOX.NORMAL, eventKey, cutsceneNum);              

            if (!haveQuest)
            {
                firstDialogueFinished = true;
            }
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
