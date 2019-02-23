using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    // 01. 씬 이동
    // 02. 세이브와 로드
    // 03. 아이템
    static public DataBaseManager instance;

    private PlayerStat thePlayerStat;

    public GameObject prefabs_Floating_Text;
    public GameObject parent;

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches; 

    public List<Item> itemList = new List<Item>();

    private void FloatText(int number, string color)
    {
        Vector3 vector = thePlayerStat.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = number.ToString();
        if (color == "GREEN")
            clone.GetComponent<FloatingText>().text.color = Color.green;
        else if (color == "BLUE")
            clone.GetComponent<FloatingText>().text.color = Color.blue;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);

    }

    public void UseItem(int _itemID)
    {
        switch(_itemID)
        {
            case 10001:
                if (thePlayerStat.hp >= thePlayerStat.currentHp + 50)
                    thePlayerStat.currentHp += 50;
                else
                    thePlayerStat.currentHp += thePlayerStat.hp;

                FloatText(50, "GREEN");
                break;
            case 10002:
                if (thePlayerStat.mp >= thePlayerStat.currentMp + 50)
                    thePlayerStat.currentMp += 15;
                else
                    thePlayerStat.currentMp += thePlayerStat.mp;

                FloatText(50, "BLUE");
                break;
        }
    }

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
        thePlayerStat = FindObjectOfType<PlayerStat>();

        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 체워주는 마법의 물약", Item.ItemType.Use));
    }
}
