using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCSV : MonoBehaviour // CSV 파일을 불러와서 data에 저장하는 함수.
{
    private List<Dictionary<string, object>> data;
    private bool canUse = false;
    
    void Start()
    {
        data = CSVReader.Read("CH01 Dialogue");
        
        if (data != null) 
        {
            canUse = true;
            Debug.Log("스크립트(LoadCSV) : CSV 파일 불러오기 성공."); // 성공 메세지.
        }
        else if (data == null)
        {
            canUse = false;
            Debug.Log("스크립트(LoadCSV) : CSV 파일 불러오기 실패."); // 에러 메세지.
        }
    }
    
    public List<Dictionary<string,object>> GetDialogueData() // 저장한 data를 반환하는 함수.
    {
        if (canUse) return data;
        else return null;
    }
}
