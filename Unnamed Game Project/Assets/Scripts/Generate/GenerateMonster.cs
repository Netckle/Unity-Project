using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMonster : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    private int monsterRandomIndex;

    public int monsterMaxCount;
    private GameObject[] monsters;

    private Vector3[] monsterSpawnPos;
    private int monsterFadingSpace;



    void Start()
    {
        MakeMonster();        
    }

    void MakeMonster()
    {
        monsters = new GameObject[monsterMaxCount];
        monsterSpawnPos = new Vector3[monsterMaxCount];

        for (int i = 0; i < monsterMaxCount; i++)
        {
            monsterRandomIndex = Random.Range(0, 1); // 0 ~ 3
            monsterFadingSpace = Random.Range(-8, 9); // -8 ~ 8

            monsterSpawnPos[i] = new Vector3(transform.position.x + monsterFadingSpace, transform.position.y + 3, 0);
            monsters[i] = Instantiate(monsterPrefabs[monsterRandomIndex], monsterSpawnPos[i], Quaternion.identity);
        }
    }

    

    
}
