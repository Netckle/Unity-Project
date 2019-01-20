using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
    public DialogueManager manager;

    public int startIndex = 0;
    public int endIndex = 0;

    public int optionStartIndex = 0;
    public int optionEndIndex = 0;

    private bool isTalking = false;

    public Button choiceButton;
    
    void Start()
    {
        choiceButton.onClick.AddListener(() => manager.UpdateDialogue(optionStartIndex, optionEndIndex));
    }    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isTalking = true;
            manager.StartDialogue(startIndex, endIndex);
        }

        if (isTalking && Input.GetKeyDown(KeyCode.S))
        {
            manager.DisplayNextSentence();
        }

        if (isTalking && Input.GetKeyDown(KeyCode.D))
        {
            manager.UpdateDialogue(optionStartIndex, optionEndIndex);
        }

        
    }
}
