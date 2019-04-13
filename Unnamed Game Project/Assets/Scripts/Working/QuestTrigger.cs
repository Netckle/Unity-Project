using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public QuestManager questManger;
    public int questIndex;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("상호작용"))
        {
            questManger.StartQuest(questIndex);
        }
    }
}
