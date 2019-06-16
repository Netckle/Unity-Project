using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTest : Cutscene
{
    public PlayerMovement player;
    public NPCMovement npc;
    public BossSlimeMovement bossSlime;

    public int start;
    public int end;
    
    void Start()
    {
        StartCoroutine(Cutscene01());
    }
    IEnumerator Cutscene01()
    {        
        bossSlime.gameObject.SetActive(false);

        player.canMove = false;

        player.gameObject.transform.position = npc.gameObject.transform.position + new Vector3(-5, 1, 0);
       
        DialogueManager.instance.StartDialogue(this, JsonManager.instance.Load<Dialogue>(), start, end);

        yield return new WaitUntil(() => dialogueIsEnd);

        bossSlime.gameObject.SetActive(true);
        
        Camera.main.GetComponent<MultipleTargetCamera>().targets[1] = bossSlime.gameObject.transform;
        player.canMove = true;
    }
    
}
