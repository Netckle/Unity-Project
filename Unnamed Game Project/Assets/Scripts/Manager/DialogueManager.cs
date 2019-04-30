using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

// 대화상자의 타입.
public enum TYPE { NORMAL, SMALL };

public class DialogueManager : MonoBehaviour 
{
	private Queue<Dictionary<string,object>> sentences = null;

	public TextMeshProUGUI smallDialogueSentence; 
	public TextMeshProUGUI bigDialogueName; 
	public TextMeshProUGUI bigDialogueSentence; 
	
	public Image smallDialoguePanel = null;
	public Image bigDialoguePanel = null;

	void Start () 
	{
		sentences = new Queue<Dictionary<string,object>>();

		smallDialoguePanel.gameObject.SetActive(false);
		bigDialoguePanel.gameObject.SetActive(false);
	}

	private int 		key 		= 0;
	public Sprite[] 	imgSamples;
	public GameObject imagePrefab;
	private GameObject 	appliedImg 	= null;

	public void StartDialogue (GameObject talkedObj, List<Dictionary<string,object>> dialogueData, 
	int[] dialogueIndexRange, TYPE dialogueType, int _key, int image = -1)
	{	
		key = _key;

		Player.Instace().targetPos = new Vector3(talkedObj.transform.position.x - 4, talkedObj.transform.position.y, talkedObj.transform.position.z);
		Player.Instace().canMove = false;

		if (image != -1)
		{
			appliedImg = Instantiate(imagePrefab, new Vector3(0, 0, -1), Quaternion.identity);
			appliedImg.AddComponent<SpriteRenderer>();
			appliedImg.GetComponent<SpriteRenderer>().sprite = imgSamples[image];
		}

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

		//AfterDialogue.Instance().StartEvent(key);	

		Destroy(appliedImg);
		appliedImg = null;	

		GameManager.Instance().stageM.generatedStages[GameManager.Instance().stageM.currentStageIndex].GetComponent<Stage>().SpawnBox();
		AfterDialogue.Instance().StartEvent(key);
		key = 0;
	}
}
