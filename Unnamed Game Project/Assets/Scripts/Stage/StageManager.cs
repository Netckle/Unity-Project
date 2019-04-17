using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class StageManager : MonoBehaviour
{
    public GameObject[] stagePrefabs;       // 만들어둔 맵 오브젝트를 넣습니다.
    private int stageRandomIndex;           // 생성할 맵의 번호. 매번 달라집니다.

    public int stageMaxCount;               // 맵 갯수를 제한합니다.
    public GameObject[] generatedStages;    // 생성될 맵이 저장되는 곳입니다.

    private JsonManager jsonManager;        // Json 기능을 사용하기 위해 만들어둔 매니저를 불러옵니다.
    private List<JsonData> stageData;       // 불러온 Json 데이터를 저장할 곳입니다.

    public int currentStageIndex = 0;       // 플레이어가 있는 방의 번호입니다.
    private Player player;                  // 플레이어 데이터를 불러옵니다.

    private SpawnManager spawnManager;

    void Awake()
    {
        jsonManager = FindObjectOfType<JsonManager>().GetComponent<JsonManager>();
        player = FindObjectOfType<Player>().GetComponent<Player>();

        player.currentRoomNum = currentStageIndex;

        spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();

        GenerateStage();
    }

    void GenerateStage()
    {
        stageData = jsonManager.LoadMapDataJson();
        generatedStages = new GameObject[stageMaxCount];

        stageMaxCount = stageData.Count;

        for (int i = 0; i < stageMaxCount; i++)
        {
            stageRandomIndex = Random.Range(0, stagePrefabs.Length);

            generatedStages[i] = Instantiate(stagePrefabs[stageRandomIndex], new Vector3(0, i * (-12), 0), Quaternion.identity);
            generatedStages[i].name = "Stage 0" + i;

            switch(stageData[i].MapCategory)
            {
                case "몬스터":
                    generatedStages[i].GetComponent<Stage>().stageType = StageType.MONSTER;
                    generatedStages[i].GetComponent<Stage>().monsterMaxCount = stageData[i].EnemyCount;
                    break;
                case "이벤트":
                    generatedStages[i].GetComponent<Stage>().stageType = StageType.NPC;
                    generatedStages[i].GetComponent<Stage>().npcIndex = stageData[i].NPCIndex;
                    break;
            }
        }

        Camera.main.transform.position = new Vector3(0, 0, -10);
        player.transform.position = generatedStages[currentStageIndex].transform.position;
    }

    public void MoveNextRoom()
    {
        Debug.Log (currentStageIndex+"가 현재 번호" + (stageMaxCount -1) +"가 최대번호");

        

        if (currentStageIndex == stageMaxCount - 1)
        {
            SceneManager.LoadScene("던전 구성 단계");
        }

        else if (currentStageIndex < stageMaxCount -1)
        {
            currentStageIndex++;
            Camera.main.transform.position = new Vector3(generatedStages[currentStageIndex].transform.position.x, generatedStages[currentStageIndex].transform.position.y, -10);
            player.transform.position = generatedStages[currentStageIndex].transform.position;
        }    

        for (int i = 0; i < currentStageIndex; i++)
        {
            generatedStages[i].GetComponent<Stage>().alreadyClear = true;
        }    

        spawnManager.portal.SetActive(false);
    }
}
