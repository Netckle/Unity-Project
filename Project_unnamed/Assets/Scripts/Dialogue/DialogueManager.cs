using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

// 대화상자의 타입 결정
public enum TYPE { NORMAL, SMALL };

public class DialogueManager : MonoBehaviour 
{
	// Private 변수
	private Queue<Dictionary<string,object>> sentences = null;
	private bool isEnd = false;

	// Public 변수
	public TextMeshProUGUI smallDiaSentence; 
	public TextMeshProUGUI bigDiaName; 
	public TextMeshProUGUI bigDiaSentence; 
	public Image smallDiaPanel 	= null;
	public Image bigDiaPanel 	= null;

	void Start () 
	{
		sentences = new Queue<Dictionary<string,object>>();

		smallDiaPanel.gameObject.SetActive(false);
		bigDiaPanel.gameObject.SetActive(false);
	}

	public void StartDialogue (List<Dictionary<string,object>> data, 
	int[] normalIndexRange, TYPE dialogueType)
	{	
		isEnd = false;

		if (dialogueType == TYPE.NORMAL) 
		{
			bigDiaPanel.gameObject.SetActive(true);
			smallDiaPanel.gameObject.SetActive(false);
		}
		else if (dialogueType == TYPE.SMALL)
		{
			bigDiaPanel.gameObject.SetActive(false);
			smallDiaPanel.gameObject.SetActive(true);
		}

		sentences.Clear();

		for (int i = 0; i < normalIndexRange.Length; i++)
		{
			sentences.Enqueue(data[i]);
		}

		DisplayNextSentence(dialogueType);
	}

	public void DisplayNextSentence (TYPE dialogueType)
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		Dictionary<string, object> sentence = sentences.Dequeue();

		if ((int)sentence["NextIndex"] != -1)
		{
			
		}

		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence, dialogueType));
	}

	IEnumerator TypeSentence (Dictionary<string, object> sentence, TYPE dialogueType)
	{
		switch (dialogueType)
		{
			case TYPE.NORMAL: 
			bigDiaName.text = sentence["Name"].ToString();
			bigDiaSentence.text = "";		
			break;

			case TYPE.SMALL: 
			smallDiaPanel.GetComponent<UpdateDialoguePanel>().SetTarget(sentence["Name"].ToString());
			smallDiaSentence.text = "";		
			break;
		}

		

		foreach (char letter in sentence["Text"].ToString().ToCharArray())
		{
			if (dialogueType == TYPE.NORMAL) bigDiaSentence.text += letter;			
			else if (dialogueType == TYPE.SMALL) smallDiaSentence.text += letter;

			yield return null;
		}
	}

	void EndDialogue()
	{
		bigDiaPanel.gameObject.SetActive(false);
		smallDiaPanel.gameObject.SetActive(false);

		isEnd = true;

		Debug.Log("대화가 끝났습니다.");
	}

	public bool GetDialogueEnd()
	{
		return isEnd;
	}
}
