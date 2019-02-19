using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST01 : MonoBehaviour
{
    [SerializeField]
    public Choice choice;

    private OrderManager theOrder;
    private ChoiceManager theChoice;
    private DialogueManager theDM;

    public bool flag;
    public string[] texts;

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theDM = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag)
        {
            StartCoroutine(ACoroutine());
        }
    }

    IEnumerator ACoroutine()
    {
        flag = true;
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theDM.ShowText(texts);
        theChoice.ShowChoice(choice);
        yield return new WaitUntil(() => !theDM.talking);
        yield return new WaitUntil(() => !theChoice.choiceIng);
        
        theOrder.canMove();
        Debug.Log(theChoice.GetResult());
    }
}
