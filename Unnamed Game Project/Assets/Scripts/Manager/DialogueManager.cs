using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public enum DIALOGUEBOX 
{
	NORMAL, SMALL
}

public enum EVENTTYPE 
{
	NONE, ITEM, QUEST, STATUS 
}

public class DialogueManager : MonoBehaviour 
{
	private Queue<Dictionary<string,object>> sentences = new Queue<Dictionary<string,object>>();

	public Image smallPanel = null;
	public TextMeshProUGUI smallContent; 

	public Image bigPanel = null;
	public TextMeshProUGUI bigName; 
	public TextMeshProUGUI bigContent; 

	public Sprite[] cutsceneImages;
	public GameObject cutscenePrefab;

	public GameObject[] itemPrefabs;

	private int eventKey = 0;	
	private GameObject appliedCutscene = null;

	void Start () 
	{
		smallPanel.gameObject.SetActive(false);
		bigPanel.gameObject.SetActive(false);
	}

	public void StartDialogue (GameObject talkingObject, List<Dictionary<string,object>> dialogueData, 
	int[] dialogueRange, DIALOGUEBOX boxType, int key, int cutsceneNum = -1)
	{	
		eventKey = key;

		Player.Instace().targetPos = new Vector3(talkingObject.transform.position.x - 4, talkingObject.transform.position.y, talkingObject.transform.position.z);
		Player.Instace().canMove = false;

		if (cutsceneNum != -1)
		{
			appliedCutscene = Instantiate(cutscenePrefab, GameManager.Instance().stageM.generatedStages[GameManager.Instance().stageM.currentStageIndex].transform.position, Quaternion.identity);
			appliedCutscene.AddComponent<SpriteRenderer>();
			appliedCutscene.GetComponent<SpriteRenderer>().sprite = cutsceneImages[cutsceneNum];
		}

		// Determines the type of dialog box.
		switch (boxType)
		{
			case DIALOGUEBOX.NORMAL:
				bigPanel.gameObject.SetActive(true);
				smallPanel.gameObject.SetActive(false);
				break;
			case DIALOGUEBOX.SMALL:
				bigPanel.gameObject.SetActive(false);
				smallPanel.gameObject.SetActive(true);
				break;
		}

		sentences.Clear();

		int startIndex = dialogueRange[0];
		int endIndex = dialogueRange[1] - 1;

		for (int i = startIndex; i < endIndex; i++)
		{
			sentences.Enqueue(dialogueData[i]);
		}

		Player.Instace().StartCoroutine("MoveToPlayerForTalk");
		DisplayNextSentence(boxType);
	}

	public void DisplayNextSentence (DIALOGUEBOX boxType)
	{
		if (sentences.Count == 0) 
		{
			EndDialogue();
			return;
		}

		Dictionary<string, object> sentence = sentences.Dequeue(); 

		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence, boxType));
	}

	IEnumerator TypeSentence (Dictionary<string, object> sentence, DIALOGUEBOX boxType)
	{
		// Set name text
		switch (boxType)
		{
			case DIALOGUEBOX.NORMAL: 
				bigName.text = sentence["Name"].ToString();
				bigContent.text = "";		
				break;

			case DIALOGUEBOX.SMALL: 
				smallPanel.GetComponent<UpdateDialoguePanel>().SetTarget(sentence["Name"].ToString());
				smallContent.text = "";		
				break;
		}		

		// Set content text
		foreach (char letter in sentence["Text"].ToString().ToCharArray())
		{
			if (boxType == DIALOGUEBOX.NORMAL) 
			{
				bigContent.text += letter;		
			}	
			else if (boxType == DIALOGUEBOX.SMALL) 
			{
				smallContent.text += letter;
			}
			yield return null;
		}
	}

	void EndDialogue()
	{
		Destroy(appliedCutscene);
		appliedCutscene = null;	

		bigPanel.gameObject.SetActive(false);
		smallPanel.gameObject.SetActive(false);

		Player.Instace().canMove = true;
		Player.Instace().isTalking = false;	

		ChooseEvent(eventKey);

		GameManager.Instance().stageM.generatedStages[GameManager.Instance().stageM.currentStageIndex].GetComponent<Stage>().SpawnBox();
		
		eventKey = 0;
	}

	// Functions that occur after the conversation is over.

	void ChooseEvent(int key) 
    {
        int category = (key / 10);
        int eventNum = (key % 10);

		EVENTTYPE type = EVENTTYPE.NONE;

        switch (category)
		{
			case 1: 
				type = EVENTTYPE.ITEM; 
				break;
			case 2: 
				type = EVENTTYPE.QUEST; 
				break;
			case 3:
				type = EVENTTYPE.STATUS;
				break;
		}
		StartEvent(type, eventNum);
    }

	void StartEvent(EVENTTYPE type, int key)
	{
		switch (type)
		{
			case EVENTTYPE.ITEM:
				Inventory.Instance().AddItem(itemPrefabs[key].GetComponent<Item>());
				break;
			case EVENTTYPE.QUEST:
        		if (!GameManager.Instance().questM.CheckState(key))
					GameManager.Instance().questM.FirstStart(key);
				break;
			case EVENTTYPE.STATUS:
				Debug.Log("스탯 관련 내용.");
				break;
		}
	}
}
