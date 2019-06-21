using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    Queue<Dialogue> sentences = new Queue<Dialogue>();

    [Header("UI")]
    public Image panel;
    public TextMeshProUGUI content;

    void Start()
    {
        //panel.gameObject.SetActive(false);
    }

    private Cutscene cutscneTemp;

    public void StartDialogue(Cutscene cutscne, Dialogue[] data, int start, int end)
    {
        Debug.Log("StartDialogue");
        FindObjectOfType<PlayerMovement>().isTalking = true;

        cutscneTemp = cutscne;
        panel.gameObject.SetActive(true);

        sentences.Clear();

        for (int i = start; i < end; ++i)
        {
            sentences.Enqueue(data[i]);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(Dialogue sentence)
    {
        content.text = "";

        foreach (char letter in sentence.content)
        {
            content.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    void EndDialogue()
    {
        panel.gameObject.SetActive(false);
        cutscneTemp.dialogueIsEnd = true;
        FindObjectOfType<PlayerMovement>().isTalking = false;
    }
}
