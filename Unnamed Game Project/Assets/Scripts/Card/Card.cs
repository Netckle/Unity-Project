using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonData
{
    public string Category;
    public int EnemyCount;
    public int EventNum;
    public int QuestNum;

    public JsonData(string category, int enemyCount, int eventNum, int questNum)
    {
        Category = category;
        EnemyCount = enemyCount;
        EventNum = eventNum;
        QuestNum = questNum;
    }
}

public class Card : MonoBehaviour
{
    [HideInInspector]
    public JsonData data;

    public string category;
    public int enemyCount;
    public int evenetNum;
    public int questNum;

    void Start()
    {
        data = new JsonData(category, enemyCount, evenetNum, questNum);
    }
}
