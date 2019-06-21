using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene01 : Cutscene
{
    public PlayerMovement player;
    public NPCMovement npc;
    public BossSlimeMovement bossSlime;

    public bool bossIsDead = false;

    public bool cutscne01IsEnd, cutscne02IsEnd;
    
    void Start()
    {
        if (!cutscne01IsEnd)
            StartCoroutine(Cutscene0101(0, 6));
    }

    void Update()
    {
        if (player.catchedSlimes >= 1 && !cutscne02IsEnd)
        {
            cutscne02IsEnd = true;
            StartCoroutine(Cutscene0102(7, 12));
        }

        if (bossIsDead)
        {
            bossIsDead = false;
            StartCoroutine(EndCutscene(4.0f, 11, 16));
        }
    }

    IEnumerator EndCutscene(float xPaddingSize, int start, int end)
    {
        // 페이드 효과

        player.transform.position = npc.gameObject.transform.position + new Vector3(xPaddingSize, 0, 0);

        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;

        DialogueManager.instance.StartDialogue(this, JsonManager.instance.Load<Dialogue>(), start, end);

        yield return new WaitUntil(() => dialogueIsEnd);

        // 페이드 효과...
    }

    IEnumerator Cutscene0101(int start, int end)
    {        
        bossSlime.gameObject.SetActive(false);

        player.canMove = false;
        player.gameObject.transform.position = npc.gameObject.transform.position + new Vector3(-5, 1, 0);
       
        DialogueManager.instance.StartDialogue(this, JsonManager.instance.Load<Dialogue>(), start, end);

        yield return new WaitUntil(() => dialogueIsEnd);

        bossSlime.gameObject.SetActive(true);
        
        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;
        player.canMove = true;

        cutscne01IsEnd = true;
    }
    
    IEnumerator Cutscene0102(int start, int end)
    {
        dialogueIsEnd = false;
        Debug.Log("실행되었단다.");
        
        bossSlime.pause = true;

        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = npc.gameObject.transform;
        player.canMove = false;

        DialogueManager.instance.StartDialogue(this, JsonManager.instance.Load<Dialogue>(), start, end);

        yield return new WaitUntil(() => dialogueIsEnd);

        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;
        player.canMove = true;

        bossSlime.pause = false;
    }
}
