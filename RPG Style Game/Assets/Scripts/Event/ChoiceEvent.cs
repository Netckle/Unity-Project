using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceEvent : MonoBehaviour
{
    [SerializeField]
    public Choice choice;

    private OrderManager the_order;
    private ChoiceManager the_choice;
    private DialogueManager the_dialogue;

    public bool flag;
    public string[] texts;

    void Start()
    {
        the_order = FindObjectOfType<OrderManager>();
        the_choice = FindObjectOfType<ChoiceManager>();
        the_dialogue = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag)
        {
            StartCoroutine(ChoiceEventCoroutine());
        }
    }

    IEnumerator ChoiceEventCoroutine()
    {
        flag = true;

        the_order.PreLoadCharacter();
        the_order.NotMove();

        the_dialogue.ShowText(texts);
        the_choice.ShowChoice(choice);

        yield return new WaitUntil(() => !the_dialogue.is_talking);
        yield return new WaitUntil(() => !the_choice.is_choicing);
        
        the_order.canMove();
        Debug.Log(the_choice.GetResult());
    }
}
