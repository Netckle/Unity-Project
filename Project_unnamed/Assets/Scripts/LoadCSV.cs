using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCSV : MonoBehaviour
{
    private List<Dictionary<string, object>> data;
    private bool canUse = false;
    
    void Start()
    {
        data = CSVReader.Read("CH01 Dialogue");
        
        if (data != null) 
        {
            canUse = true;
            Debug.Log("CSV 파일 불러오기 성공.");
        }
        else if (data == null)
        {
            canUse = false;
            Debug.Log("CSV 파일 불러오기 실패.");
        }
    }

    public List<Dictionary<string,object>> GetDialogueData()
    {
        if (canUse) return data;
        else return null;
    }
}
