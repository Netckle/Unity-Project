using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] stagePrefabs;  
    public GameObject[] generatedStages;  

    public int stageMaxCount;
    public int currentStageIndex = 0;  

    private int stageIndex;       
    private List<CardData> stageData;             
    
    public void GenerateStage()
    {
        currentStageIndex = 0;

        Player.Instace().currentRoomNum = currentStageIndex;
        Player.Instace().transform.position = Vector3.zero;

        stageData = GameManager.Instance().jsonM.Load();

        stageMaxCount = stageData.Count; 
        generatedStages = new GameObject[stageMaxCount];

        for (int i = 0; i < stageMaxCount; i++)
        {
            if (stageData[i].cardName == "음식채집")
            {
                stageIndex = 3;
            }

            else if (stageData[i].cardLevel <= 5) 
            {
                stageIndex = 0;
            } 

            else if (stageData[i].cardLevel > 5 || stageData[i].cardLevel <= 10) 
            {
                stageIndex = 1;
            }

            

            generatedStages[i] = Instantiate(stagePrefabs[stageIndex], new Vector3(0, i * (-12), 0), Quaternion.identity);
            generatedStages[i].name = "Stage 0" + i;
        }

        for (int i = 0; i < stageMaxCount; i++)
        {
            generatedStages[i].GetComponent<Stage>().stageType = stageData[i].cardName;
            switch (stageData[i].cardName)
            {
                case "던전탐험":
                    generatedStages[i].GetComponent<Stage>().monsterLevel = stageData[i].cardLevel;
                    generatedStages[i].GetComponent<Stage>().monsterMaxCount = (stageData[i].cardLevel + 1);
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

    public void MoveToNextRoom()
    {       
        

        if (currentStageIndex == (stageMaxCount - 1))
        {
            GameManager.Instance().sceneChangeM.Activate("Dungeon Scene Select", true);
            Player.Instace().transform.position = Vector3.zero;
        }
        else 
        {
            currentStageIndex++;
            Player.Instace().currentRoomNum = currentStageIndex;

            Camera.main.GetComponent<MoveNext>().MoveToNextRoomCamera();
            Player.Instace().transform.position = generatedStages[currentStageIndex].transform.position;
        }                 

        for (int i = 0; i < currentStageIndex; i++)
        {
            generatedStages[i].GetComponent<Stage>().stageCleared = true; // 이전 스테이지 전부 "이미 클리어함" 상태로 변경.
        }    
        GameManager.Instance().spawnM.portal.SetActive(false); // 포탈 비활성화.
    }
}
