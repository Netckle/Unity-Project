using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int key;
    private StageManager stageManager;

    private Stage data;

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>().GetComponent<StageManager>();        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            stageManager.generatedStages[stageManager.currentStageIndex].GetComponent<Stage>().DestroyMonster(key);
        }
    }
}
