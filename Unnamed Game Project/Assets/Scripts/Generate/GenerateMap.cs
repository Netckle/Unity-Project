using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GenerateMap : MonoBehaviour // StageManager
{
    public GameObject[] mapPrefabs;
    private int mapRadomIndex;

    public int mapMaxCount;
    private GameObject[] maps;
    
    private JsonManager jsonManager;
    private List<JsonData> mapDataFromJson;

    public int currentMapNum = 0;
    private GameObject player;

    //private GameObject door;
    //public bool isOpen = false;

    void Start()
    {
        jsonManager = GameObject.Find("Json Manager").GetComponent<JsonManager>();

        //door = GameObject.Find("Door");
        //door.SetActive(false);

        player = GameObject.FindWithTag("Player");
        player.GetComponent<Player>().currentRoomNum = currentMapNum;

        MakeMap();        
    }

    void MakeMap()
    {
        mapDataFromJson = jsonManager.LoadMapDataJson();
        maps = new GameObject[mapMaxCount];

        mapMaxCount = mapDataFromJson.Count;

        for (int i = 0; i < mapMaxCount; i++)
        {
            mapRadomIndex = Random.Range(0, mapPrefabs.Length);

            maps[i] = Instantiate(mapPrefabs[mapRadomIndex], new Vector3(0, i * -12, 0), Quaternion.identity);
            maps[i].name = "Map 0" + i;
            
            maps[i].GetComponent<GenerateMonster>().monsterMaxCount = mapDataFromJson[i].EnemyCount;
        }

        Camera.main.transform.position = new Vector3(maps[0].transform.position.x, maps[0].transform.position.y, -10);
        player.transform.position = new Vector3(maps[0].transform.position.x, maps[0].transform.position.y - 3, 0);
    }

    public void MoveNextRoom()
    {
        if (currentMapNum == mapMaxCount - 1) 
            return;

        currentMapNum += 1;
        player.GetComponent<Player>().currentRoomNum = currentMapNum;

        Vector3 to = new Vector3(maps[currentMapNum].transform.position.x, maps[currentMapNum].transform.position.y + 3, maps[currentMapNum].transform.position.z);
        player.transform.position = to;

        Camera.main.GetComponent<MoveToNextRoom>().MoveNext();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            MoveNextRoom();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //SpawnDoor();
        }
    }

    /*
    void SpawnDoor()
    {
        if (isOpen)
        {
            door.transform.position = new Vector2(-0.5f, -(currentMapNum * 12) + 3f);
            door.SetActive(true);
        }
        else if (!isOpen)
        {            
            door.SetActive(false);
        }
    }
    */
}
