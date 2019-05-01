using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DATATYPE
{
    DIALOGUE, QUEST
}

public class LoadCSV : MonoBehaviour
{
    private List<Dictionary<string, object>> dialogueData;
    private List<Dictionary<string, object>> questData;

    public string dialogueFileName;
    public string questFileName;    

    private bool dialogueCheck = false;
    private bool questCheck = false;

    void Awake()
    {
        dialogueData = CSVReader.Read(dialogueFileName);
        questData = CSVReader.Read(questFileName);

        dialogueCheck = CheckFile(dialogueFileName, dialogueData);
        questCheck = CheckFile(questFileName, questData);
    }

    bool CheckFile(string fileName, List<Dictionary<string, object>> data)
    {
        if (data == null) 
        {
            Debug.Log(fileName + " 파일 로딩에 실패했습니다.");
            return false;
        }
        else if (data != null) 
        {
            Debug.Log(fileName + " 파일 로딩에 성공했습니다.");
            return true;
        }
        else 
            return false;
    }

    public List<Dictionary<string, object>> GetData(DATATYPE type)
    {
        switch (type)
        {
            case DATATYPE.DIALOGUE:
                if (dialogueCheck)
                { 
                    return dialogueData;
                }
                else 
                {
                    Debug.LogError("대화 파일을 불러올 수 없습니다.");
                    break;
                }
            case DATATYPE.QUEST:
                if (questCheck)
                {
                     return questData;
                }
                else 
                {
                    Debug.LogError("퀘스트 파일을 불러올 수 없습니다.");
                    break;
                }
        }        
        return null;
    }
}
