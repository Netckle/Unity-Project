using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST01 : MonoBehaviour
{
    [SerializeField]
    public Choice choice;

    private OrderManager theOrder;
    private ChoiceManager theChoice;

    public bool flag;

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
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
        theChoice.ShowChoice(choice);

        yield return new WaitUntil(() => !theChoice.choiceIng);
        
        theOrder.Move();
        Debug.Log(theChoice.GetResult());
    }
}
