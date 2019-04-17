using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonData
{
    public string   MapCategory; // 맵 카테고리
    public int      EnemyCount;  // 적 마릿수
    public int      NPCIndex;    // 이벤트 번호

    public JsonData(string mapCategory, int enemyCount, int npcIndex)
    {
        MapCategory = mapCategory;
        EnemyCount  = enemyCount;
        NPCIndex    = npcIndex;
    }
}

public class Card : MonoBehaviour
{
    [HideInInspector]
    public JsonData data;

    public string   mapCategory;
    public int      enemyCount;
    public int      npcIndex;

    void Start()
    {
        // 단순 저장.
        data = new JsonData(mapCategory, enemyCount, npcIndex);
    }
}
