﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType { NONE, MONSTER, NPC };

public enum StageState { CLEARED, CLEAR, NOTCLEAR };

public class Stage : MonoBehaviour
{
    public StageType stageType = StageType.NONE;
    public StageState stageState = StageState.NOTCLEAR;

    private int monsterRandomIndex;

    public int monsterMaxCount;
    public Dictionary<int, GameObject> generatedMonsters = new Dictionary<int, GameObject>();

    private Vector3[] monsterSpawnPos;
    private int monsterFadingSpace;
    
    SpawnManager spawnManager;

    public int npcIndex;
    private GameObject generatedNpc;

    public bool alreadyClear = false;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();

        switch(stageType)
        {
            case StageType.MONSTER:
                monsterSpawnPos = new Vector3[monsterMaxCount];
                GenerateMonster();
                break;
            case StageType.NPC:
                GenerateNPC();
                break;
            case StageType.NONE:
                Debug.LogError("스테이지 타입에서 에러가 생겼습니다.");
                break;
        }
    }

    private bool portalSpawned = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(generatedMonsters[1].name);
        }

        if (alreadyClear)
        {
            stageState = StageState.CLEARED;
        }

        else if (!alreadyClear && generatedMonsters.Count == 0)
        {
            stageState = StageState.CLEAR;
        }

        if (stageState == StageState.CLEAR)
        {
            if (!portalSpawned)
                SpawnBox();
        }
    }

    void SpawnBox()
    {
        spawnManager.portal.transform.position = this.gameObject.transform.position;
        spawnManager.portal.SetActive(true);
        portalSpawned = true;
    }

    void GenerateMonster()
    {
        Debug.Log(monsterMaxCount);

        for (int i = 0; i < monsterMaxCount; i++)
        {
            monsterRandomIndex = Random.Range(0, spawnManager.monsterPrefabs.Length);
            monsterFadingSpace = Random.Range(spawnManager.spawnFadingRange[0], spawnManager.spawnFadingRange[1]);            

            monsterSpawnPos[i] = new Vector3(transform.position.x + monsterFadingSpace, transform.position.y + 3, 0);

            Debug.Log("이제" + spawnManager.monsterPrefabs[monsterRandomIndex].name + "를 추가합니다.");   
            Debug.Log("그리고 " + monsterSpawnPos[i] + " 가 현재 벡터입니다.");        

            GameObject temp = Instantiate(spawnManager.monsterPrefabs[monsterRandomIndex], monsterSpawnPos[i], Quaternion.identity) as GameObject;
            temp.GetComponent<Monster>().key = i;

            generatedMonsters.Add(i, temp);
        }
    }

    void GenerateNPC()
    {
        generatedNpc = Instantiate(spawnManager.npcPrefabs[npcIndex], transform.position, Quaternion.identity);
    }

    public void DestroyMonster(int key)
    {
        if (generatedMonsters.ContainsKey(key))
        {
            Destroy(generatedMonsters[key]);
            generatedMonsters.Remove(key);
        }
    }
}