using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageState { CLEARED, CLEAR, NOTCLEAR };

public class Stage : MonoBehaviour
{
    public string       stageType;      
    public StageState   stageState = StageState.NOTCLEAR;

    [HideInInspector]
    public int          monsterLevel = 0;

    public int          monsterMaxCount = 0;

    public Dictionary<int, GameObject> generatedMonsters = new Dictionary<int, GameObject>();

    private Vector3[]   monsterSpawnPos;
    private int         monsterFadingSpace;

    public int          npcIndex        = 0;
    private GameObject  generatedNpc    = null;

    public int          objectIndex     = 0;
    private GameObject  generateObject  = null;

    public bool         alreadyClear    = false;
    private bool        portalSpawned   = false;

    void Start()
    {
        switch(stageType)
        {
            case "던전탐험":
                monsterSpawnPos = new Vector3[monsterMaxCount];
                GenerateMonster();
                break;
            case "음식채집":
                
                break;
            default:
                Debug.LogError("스테이지 타입 에러.");
                break;
        }
    }

    void Update()
    {
        if (alreadyClear) // 다음 맵으로 이동할 때 true가 되는 변수.
        {
            stageState = StageState.CLEARED; // 스테이지는 이미 클러어됨 상태로 변경.
        }

        // [던전탐험의 경우]
        else if (!alreadyClear && stageType == "던전탐험" && generatedMonsters.Count == 0) // 해당 맵의 몬스터를 모두 제거했을때.
        {
            stageState = StageState.CLEAR;
        }

        if (stageState == StageState.CLEAR) // 클리어한 직 후.
        {
            if (!portalSpawned)
                SpawnBox(); // 포탈 소환.
        }
    }

    public void SpawnBox()
    {
        SpawnManager.Instance().portal.transform.position = this.gameObject.transform.position;
        SpawnManager.Instance().portal.SetActive(true);
        portalSpawned = true;
    }

    // Generate Function
    void GenerateMonster()
    {
        for (int i = 0; i < monsterMaxCount; i++)
        {
            int monsterRandomIndex = Random.Range(monsterLevel - 1, monsterLevel + 1); // monsterLevel - 1 부터 monsterLevel 까지
            monsterFadingSpace = Random.Range(SpawnManager.Instance().spawnFadingRange[0], SpawnManager.Instance().spawnFadingRange[1]);            

            monsterSpawnPos[i] = new Vector3(transform.position.x + monsterFadingSpace, transform.position.y + 3, 0);      

            GameObject temp = Instantiate(SpawnManager.Instance().monsterPrefabs[monsterRandomIndex], monsterSpawnPos[i], Quaternion.identity) as GameObject;
            temp.GetComponent<Monster>().key = i;

            generatedMonsters.Add(i, temp);
        }
    }

    void GenerateNPC()
    {
        generatedNpc = Instantiate
        (
            SpawnManager.Instance().npcPrefabs[npcIndex], 
            new Vector3(transform.position.x + 3, transform.position.y, transform.position.z), 
            Quaternion.identity
        );
    }

    void GenerateObject()
    {
        generateObject = Instantiate
        (
            SpawnManager.Instance().objectPrefabs[objectIndex],
            new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.identity
        );
    }

    public void DestroyMonster(int key)
    {
        if (generatedMonsters.ContainsKey(key)) // 키로 오브젝트가 있는지 탐색.
        {
            Debug.Log(key + "해당 오브젝트는 있습니다. 이제 제거합니다.");
            Destroy(generatedMonsters[key]); // 오브젝트 삭제.
            generatedMonsters.Remove(key);   // 딕셔너리 삭제.
        }
    }
}
