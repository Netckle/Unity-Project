using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class StageManager : MonoBehaviour
{
    public GameObject[]     stagePrefabs;               // 만들어둔 맵 오브젝트를 넣습니다.
    private int             stageRandomIndex;           // 생성할 맵의 번호. 매번 달라집니다.

    public int              stageMaxCount;              // 맵 갯수를 제한합니다.
    public GameObject[]     generatedStages;            // 생성될 맵이 저장되는 곳입니다.

    private List<CardData>  stageData;                  // 불러온 Json 데이터를 저장할 곳입니다.

    public int              currentStageIndex = 0;      // 플레이어가 있는 방의 번호입니다.

    void Awake()
    {
        
    }

    public void GenerateStage()
    {
        currentStageIndex = 0;
        Player.Instace().currentRoomNum = currentStageIndex;
        Player.Instace().transform.position = Vector3.zero;

        stageData       = GameManager.Instance().jsonM.LoadData();
        stageMaxCount   = stageData.Count; 
        generatedStages = new GameObject[stageMaxCount];

        // 맵 생성 반복문
        for (int i = 0; i < stageMaxCount; i++)
        {
            if (stageData[i].cardLevel <= 5) // 카드 레벨 : 1~5 까지.
            {
                stageRandomIndex = 0;
            } 
            else if (stageData[i].cardLevel > 5 || stageData[i].cardLevel <= 10) // 카드 레벨 : 5~10 까지.
            {
                stageRandomIndex = 1;
            }

            if (stageData[i].cardName == "음식채집")
            {
                stageRandomIndex = 3;
            }
            // 맵 생성
            generatedStages[i] = Instantiate(stagePrefabs[stageRandomIndex], new Vector3(0, i * (-12), 0), Quaternion.identity);
            generatedStages[i].name = "Stage 0" + i;
        }

        // 맵 타입별 처리 반복문
        for (int i = 0; i < stageMaxCount; i++)
        {
            generatedStages[i].GetComponent<Stage>().stageType = stageData[i].cardName;
            switch (stageData[i].cardName)
            {
                case "던전탐험":
                    generatedStages[i].GetComponent<Stage>().monsterLevel       = stageData[i].cardLevel;
                    generatedStages[i].GetComponent<Stage>().monsterMaxCount    = (stageData[i].cardLevel + 1);
                    break;
                case "음식채집":
                    // 화면 가운데 오브젝트 만들기.
                    break;
                
                    // 나중에...
                case "시장":
                    break;
                case "술집":
                    break;
            }
        }
    }

    public void MoveNextRoom()
    {               
        if (currentStageIndex == stageMaxCount - 1) 
        {
            Player.Instace().transform.position = Vector3.zero;
            //SceneManager.LoadScene("Dungeon Scene Select");
            GameManager.Instance().changeSceneM.SceneChange("Dungeon Scene Select");
        }
        else 
        {
            currentStageIndex++;
            Player.Instace().currentRoomNum = currentStageIndex;

            Camera.main.GetComponent<MoveToNextRoom>().MoveNext();
            Player.Instace().transform.position = generatedStages[currentStageIndex].transform.position;
        }                 

        for (int i = 0; i < currentStageIndex; i++)
        {
            generatedStages[i].GetComponent<Stage>().alreadyClear = true; // 이전 스테이지 전부 "이미 클리어함" 상태로 변경.
        }    
        GameManager.Instance().spawnM.portal.SetActive(false); // 포탈 비활성화.
    }
}
