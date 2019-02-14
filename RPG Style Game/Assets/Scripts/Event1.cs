using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    public Dialogue dialogue_01;
    public Dialogue dialogue_02;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer; // animator.GetFloat DirY == 1f
    private FadeManager theFade;
    private bool flag = false;

    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!flag && Input.GetKey(KeyCode.Z) && thePlayer.animator.GetFloat("DirY")==1f)
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        Debug.Log("실행한다. 코루틴! 씨발아...");
        theOrder.PreLoadCharacter();

        theOrder.NotMove();

        theDM.ShowDialogue(dialogue_01);

        yield return new WaitUntil(()=>!theDM.talking);

        theOrder.Move();

        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "UP");

        yield return new WaitUntil(()=>thePlayer.queue.Count == 0);

        theFade.Flash();
        theDM.ShowDialogue(dialogue_02);

        yield return new WaitUntil(()=>!theDM.talking);

        theOrder.Move();
    }
}
