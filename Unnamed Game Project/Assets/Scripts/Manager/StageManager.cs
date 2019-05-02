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
        GameManager.Instance().objectM.portal.SetActive(false);
        currentStageIndex = 0;

        Player.Instace().currentRoomNum = currentStageIndex;
        Player.Instace().transform.position = Vector3.zero;

        stageData = GameManager.Instance().jsonM.Load();

        stageMaxCount = stageData.Count; 
        generatedStages = new GameObject[stageMaxCount];

        for (int i = 0; i < stageMaxCount; i++)
        {
            switch(stageData[i].cardName)
            {
                case "사냥": stageIndex = (stageData[i].cardLevel - 1); break;
                case "채집": stageIndex = 3; break;
                case "마을": stageIndex = 4; break;
                case "시장": stageIndex = 5; break;
            }          

            generatedStages[i] = Instantiate(stagePrefabs[stageIndex], new Vector3(0, i * (-12), 0), Quaternion.identity);
            generatedStages[i].name = "Stage 0" + i;
        }

        for (int i = 0; i < stageMaxCount; i++)
        {
            generatedStages[i].GetComponent<Stage>().stageType = stageData[i].cardName;
            switch (stageData[i].cardName)
            {
                case "사냥":
                    generatedStages[i].GetComponent<Stage>().monsterLevel = stageData[i].cardLevel;
                    generatedStages[i].GetComponent<Stage>().monsterMaxCount = (stageData[i].cardLevel + 1);
                    break;
                case "채집":
                    
                    break;                
                case "마을":
                    break;
                case "시장":
                    break;
            }
        }
    

    }

    public void MoveToNextRoom()
    {              
        if (currentStageIndex == (stageMaxCount - 1)) // 마지막 스테이지 일 경우...
        {
            GameManager.Instance().sceneChangeM.Activate("Dungeon Scene Select", false);
            Player.Instace().transform.position = Vector3.zero;
        }
        else // 아직 스테이지가 남아 있을 경우...
        {
            currentStageIndex++;
            Player.Instace().currentRoomNum = currentStageIndex;

            Camera.main.GetComponent<MoveNext>().MoveToNextRoomCamera();
            Player.Instace().transform.position = generatedStages[currentStageIndex].transform.position;
        }                 

        for (int i = 0; i < currentStageIndex; i++)
        {
            generatedStages[i].GetComponent<Stage>().stageCleared = true; // 이전 스테이지들을 "이미 클리어함" 상태로 변경.
        }    
        GameManager.Instance().objectM.portal.SetActive(false); // 포탈 비활성화.
    }
}
