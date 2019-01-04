using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {

	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
	public Image dialoguePanel;

	//public Animator animator;

	private Queue<Dictionary<string,object>> sentences;

	void Start () 
	{
		sentences = new Queue<Dictionary<string,object>>();
	}

	public void StartDialogue (List<Dictionary<string,object>> data, int startIndex, int endIndex)
	{	
		sentences.Clear();

		for (int i = startIndex; i < endIndex; i++)
		{
			sentences.Enqueue(data[i]);
		}	

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		Dictionary<string, object> sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (Dictionary<string, object> sentence)
	{
		dialoguePanel.GetComponent<UpdateDialoguePanel>().SetTarget(sentence["Name"].ToString());

		nameText.text = sentence["Name"].ToString();
		dialogueText.text = "";

		foreach (char letter in sentence["Text"].ToString().ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		Debug.Log("대화 끝남.");
	}
}
