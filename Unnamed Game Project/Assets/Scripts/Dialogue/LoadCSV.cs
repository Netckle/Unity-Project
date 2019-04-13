using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXTERNALDATA
{
    public List<Dictionary<string, object>> data;
    public string dataName;
    public bool canUse = false;
}

public class LoadCSV : MonoBehaviour // TXT 파일에서 데이터를 읽어와 변수에 저장합니다.
{
    private EXTERNALDATA dialogue = null;
    private EXTERNALDATA quest = null;

    public string dialogueFileName;
    public string questFileName;    
    
    void Awake()
    {
        dialogue.data = CSVReader.Read(dialogueFileName); // Resource 폴더에 있는 CSV 파일 이름.
        dialogue.dataName = dialogueFileName;
        Debug.Log(dialogue.dataName);

        quest.data = CSVReader.Read(questFileName);       
        quest.dataName = questFileName; 
        Debug.Log(quest.dataName);

        CheckFile(dialogue);
        CheckFile(quest);
    }

    void CheckFile(EXTERNALDATA data)
    {
        if (data.data != null) 
        {
            data.canUse = true;
            Debug.Log(data.dataName + "파일 불러오기 성공."); 
        }
        else if (data.data == null)
        {
            data.canUse = false;
            Debug.Log(data.dataName + "파일 불러오기 실패."); 
        }
    }
    
    public EXTERNALDATA GetData(string dataFileName) 
    {        
        EXTERNALDATA copiedData = new EXTERNALDATA();

        switch(dataFileName)
        {
            case "대화":
                copiedData = dialogue;
                break;
            case "퀘스트":
                copiedData = quest;
                break;
        }    

        if (copiedData.canUse) 
        {
            return copiedData;
        }
        else 
        {
            Debug.Log("파일을 얻어오지 못했습니다.");
            return null;    
        }
    }    
}
