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

    public List<Item> itemList = new List<Item>();

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
    
    void Start()
    {
        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 체워주는 마법의 물약", Item.ItemType.Use));
    }
}
