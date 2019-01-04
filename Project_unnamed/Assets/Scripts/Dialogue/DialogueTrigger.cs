using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour 
{
	public int startDialogueIndex;
	public int endDialogueIndex;

	public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(GameObject.Find("LoadCSV").GetComponent<LoadCSV>().GetDialogueData(), startDialogueIndex, endDialogueIndex);
	}
}
