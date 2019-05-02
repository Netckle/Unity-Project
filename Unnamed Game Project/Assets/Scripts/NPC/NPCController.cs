using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    //public int[] dialogueRange;          
    //public DIALOGUEBOX dialogueType = DIALOGUEBOX.NORMAL; 

    public int npc_index;
    private GameObject npc;

    void Start()
    {
        Vector3 npc_spawn_pos = this.transform.position - new Vector3(-3, 0, 0);
        npc = GameObject.Instantiate(GameManager.Instance().objectM.npcPrefabs[npc_index], npc_spawn_pos, Quaternion.identity);


        //대충 npc 설정하는 내용
    }
}
