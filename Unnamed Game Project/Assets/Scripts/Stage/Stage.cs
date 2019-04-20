using System.Collections;
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
    
    //SpawnManager spawnManager;

    public int npcIndex;
    private GameObject generatedNpc;

    public bool alreadyClear = false;

    void Start()
    {
        //spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();

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

    public void SpawnBox()
    {
        SpawnManager.Instance().portal.transform.position = this.gameObject.transform.position;
        SpawnManager.Instance().portal.SetActive(true);
        portalSpawned = true;
    }

    void GenerateMonster()
    {
        for (int i = 0; i < monsterMaxCount; i++)
        {
            monsterRandomIndex = Random.Range(0, SpawnManager.Instance().monsterPrefabs.Length);
            monsterFadingSpace = Random.Range(SpawnManager.Instance().spawnFadingRange[0], SpawnManager.Instance().spawnFadingRange[1]);            

            monsterSpawnPos[i] = new Vector3(transform.position.x + monsterFadingSpace, transform.position.y + 3, 0);      

            GameObject temp = Instantiate(SpawnManager.Instance().monsterPrefabs[monsterRandomIndex], monsterSpawnPos[i], Quaternion.identity) as GameObject;
            temp.GetComponent<Monster>().key = i;

            generatedMonsters.Add(i, temp);
        }
    }

    void GenerateNPC()
    {
        generatedNpc = Instantiate(SpawnManager.Instance().npcPrefabs[npcIndex], new Vector3(transform.position.x + 3, transform.position.y, transform.position.z), Quaternion.identity);
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
