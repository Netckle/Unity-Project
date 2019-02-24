using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOrder : MonoBehaviour
{
    public Dialogue dialogue_01;
    public Dialogue dialogue_02;

    private OrderManager the_order;
    private PlayerManager the_player;
    private FadeManager the_fade;
    private DialogueManager the_dialogue;

    public bool flag;

    void Start()
    {
        the_order = FindObjectOfType<OrderManager>();
        the_dialogue = FindObjectOfType<DialogueManager>();
        the_player = FindObjectOfType<PlayerManager>();
        the_fade = FindObjectOfType<FadeManager>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!flag && Input.GetKey(KeyCode.Z) && the_player.animator.GetFloat("DirY")==1f)
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        the_order.PreLoadCharacter();
        the_order.NotMove();

        the_dialogue.ShowDialogue(dialogue_01);

        yield return new WaitUntil(()=>!the_dialogue.talking);

        the_order.canMove();

        the_order.Move("player", "RIGHT");
        the_order.Move("player", "RIGHT");
        the_order.Move("player", "UP");

        yield return new WaitUntil(()=>the_player.queue.Count == 0);

        the_fade.Flash();
        
        the_order.NotMove();
        
        the_dialogue.ShowDialogue(dialogue_02);

        yield return new WaitUntil(()=>!the_dialogue.talking);

        the_order.canMove();
    }
}
