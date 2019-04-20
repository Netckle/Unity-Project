using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCSV : MonoBehaviour // TXT 파일에서 데이터를 읽어와 변수에 저장합니다.
{
    static LoadCSV instance = null; 

    public static LoadCSV Instace()
    {
        return instance;
    } 

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        dialogueData = CSVReader.Read(dialogueFileName);
        questData = CSVReader.Read(questFileName);

        dialogueCheck = CheckFile(dialogueFileName, dialogueData);
        questCheck = CheckFile(questFileName, questData);
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private List<Dictionary<string, object>> dialogueData;
    private List<Dictionary<string, object>> questData;

    public string dialogueFileName;
    public string questFileName;    

    private bool dialogueCheck = false;
    private bool questCheck = false;

    bool CheckFile(string fileName, List<Dictionary<string, object>> data)
    {
        if (data == null) 
        {
            Debug.Log(fileName + "파일 로딩 실패.");
            return false;
        }
        else if (data != null) 
        {
            Debug.Log(fileName + "파일 로딩 성공.");
            return true;
        }
        else 
            return false;
    }

    public List<Dictionary<string, object>> GetData(string dataType)
    {
        switch (dataType)
        {
            case "Dialogue":
                if (dialogueCheck) return dialogueData;
                else break;
            case "Quest":
                if (questCheck) return questData;
                else break;
        }
        Debug.LogError("얻을 파일이 없습니다.");
        return null;
    }
}
