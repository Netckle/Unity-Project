using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int key;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // 나중에 MonsterDie()로 바꿀 예정.
            // 몬스터를 관리하는 Stage 스크립트에서 key 값으로 해당 몬스터를 찾아 제거합니다.
            StageManager.Instance().
            generatedStages[StageManager.Instance().currentStageIndex].GetComponent<Stage>().DestroyMonster(key);
        }
    }
}
