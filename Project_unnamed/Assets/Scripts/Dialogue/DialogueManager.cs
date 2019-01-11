using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Text Mesh Pro를 사용하기 위해 필요하다.

public enum TYPE
{
	NORMAL, SMALL
}

public class DialogueManager : MonoBehaviour 
{
	private Queue<Dictionary<string,object>> sentences;
	private bool isEnd = false;

	/*
		<필요요소>
		큰 대화창   : 이름, 문장  
		작은 대화창 : 문장
	 */ 
	public TextMeshProUGUI smallDiaSentence; 
	public TextMeshProUGUI bigDiaName; 
	public TextMeshProUGUI bigDiaSentence; 
	public Image smallDiaPanel = null;
	public Image bigDiaPanel = null;

	void Start () 
	{
		sentences = new Queue<Dictionary<string,object>>();

		smallDiaPanel.gameObject.SetActive(false);
		bigDiaPanel.gameObject.SetActive(false);
	}

	public void StartDialogue (List<Dictionary<string,object>> data, int[] normalIndexRange, TYPE dialogueType)
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

		for (int i = normalIndexRange[0]; i < normalIndexRange[1]; i++)
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
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence, dialogueType));
	}

	IEnumerator TypeSentence (Dictionary<string, object> sentence, TYPE dialogueType)
	{
		switch (dialogueType)
		{
			case TYPE.NORMAL: 
			// bigDiaPanel.GetComponent<UpdateDialoguePanel>().SetTarget(sentence["Name"].ToString());
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
