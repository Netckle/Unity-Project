using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TYPE { NORMAL, SMALL }; // Dialogue Size

public class DialogueManager : MonoBehaviour
{
    private List<Dictionary<string, object>> data = null;
    private List<Dictionary<string, object>> sentences = null;

    private int count = 0;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sentenceText;

    void Start()
    {
        data = CSVReader.Read("Dialogue");

        sentences = new List<Dictionary<string, object>>();
    }

    public void StartDialogue(int startIndex, int endIndex)
    {        
        sentences.Clear();

        for (int i = startIndex; i < endIndex; i++)
        {   
            Debug.Log(data[i]["Name"] + " " + data[i]["Sentence"]);
            sentences.Add(data[i]);
        }
        DisplayNextSentence();
    }

    void OnClick(int startIndex, int endIndex)
    {
        UpdateDialogue(startIndex, endIndex);
    }

    public void UpdateDialogue(int startIndex, int endIndex)
    {
        sentences = new List<Dictionary<string, object>>();
        
        for (int i = startIndex; i < endIndex; i++)
        {         
            sentences.Add(data[i]);
        }
        count = 0;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (count == sentences.Count)
        {
            EndDialogue(); 
            return;
        }
        
        Dictionary<string, object> sentence = sentences[count];       

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        count++;
    }

    void EndDialogue()
    {
        Debug.Log("대화가 종료되었습니다.");
        count = 0;
    }

    IEnumerator TypeSentence(Dictionary<string, object> sentence)
    {
        nameText.text = "";
        sentenceText.text = "";

        nameText.text = sentence["Name"].ToString();

        foreach (char letter in sentence["Sentence"].ToString().ToCharArray())
        {
            sentenceText.text += letter;
            yield return null;
        }
    }

}
