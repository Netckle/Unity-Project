using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

// 대화상자의 타입.
public enum TYPE { NORMAL, SMALL };

public class DialogueManager : MonoBehaviour 
{
	static DialogueManager instance = null;

    public static DialogueManager Instance()
    {
        return instance;
    }

	private Queue<Dictionary<string,object>> sentences = null;

	public TextMeshProUGUI smallDialogueSentence; 
	public TextMeshProUGUI bigDialogueName; 
	public TextMeshProUGUI bigDialogueSentence; 
	
	public Image smallDialoguePanel = null;
	public Image bigDialoguePanel = null;

	void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	void Start () 
	{
		if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

		sentences = new Queue<Dictionary<string,object>>();

		smallDialoguePanel.gameObject.SetActive(false);
		bigDialoguePanel.gameObject.SetActive(false);
	}

	private int key = 0;

	public void StartDialogue (GameObject talkedObj, List<Dictionary<string,object>> dialogueData, 
	int[] dialogueIndexRange, TYPE dialogueType, int _key)
	{	
		key = _key;

		Player.Instace().targetPos = new Vector3(talkedObj.transform.position.x - 4, talkedObj.transform.position.y, talkedObj.transform.position.z);
		Player.Instace().canMove = false;

		if (dialogueType == TYPE.NORMAL) 
		{
			bigDialoguePanel.gameObject.SetActive(true);
			smallDialoguePanel.gameObject.SetActive(false);
		}
		else if (dialogueType == TYPE.SMALL)
		{
			bigDialoguePanel.gameObject.SetActive(false);
			smallDialoguePanel.gameObject.SetActive(true);
		}

		sentences.Clear();

		for (int i = dialogueIndexRange[0]; i < (dialogueIndexRange[1] - 1); i++)
		{
			sentences.Enqueue(dialogueData[i]);
		}

		Player.Instace().StartCoroutine("MoveToPlayerForTalk");

		DisplayNextSentence(dialogueType);
	}

	public void DisplayNextSentence (TYPE dialogueType)
	{
		if (sentences.Count == 0) // 큐에 남은 대사가 없으면
		{
			EndDialogue();

			return;
		}

		Dictionary<string, object> sentence = sentences.Dequeue(); 

		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence, dialogueType));
	}



	IEnumerator TypeSentence (Dictionary<string, object> sentence, TYPE dialogueType)
	{
		switch (dialogueType)
		{
			case TYPE.NORMAL: 
			bigDialogueName.text = sentence["Name"].ToString();
			bigDialogueSentence.text = "";		
			break;

			case TYPE.SMALL: 
			smallDialoguePanel.GetComponent<UpdateDialoguePanel>().SetTarget(sentence["Name"].ToString());
			smallDialogueSentence.text = "";		
			break;
		}		

		foreach (char letter in sentence["Text"].ToString().ToCharArray())
		{
			if (dialogueType == TYPE.NORMAL) bigDialogueSentence.text += letter;			
			else if (dialogueType == TYPE.SMALL) smallDialogueSentence.text += letter;

			yield return null;
		}
	}

	void EndDialogue()
	{
		bigDialoguePanel.gameObject.SetActive(false);
		smallDialoguePanel.gameObject.SetActive(false);

		Player.Instace().canMove = true;
		Player.Instace().isTalking = false;

		AfterDialogue.Instance().StartEvent(key);		

		StageManager.Instance().generatedStages[StageManager.Instance().currentStageIndex].GetComponent<Stage>().SpawnBox();
			
		key = 0;
	}
}
