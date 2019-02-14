using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    // 01. 씬 이동
    // 02. 세이브와 로드
    // 03. 아이템
    static public DataBaseManager instance;
    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches; 

#region Singleton
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
#endregion Singleton
    
}
