using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;
using System.Text;

public struct CardDataContainer
{
    public List<CardData> cardData;

    public CardDataContainer(List<CardData> _cardData)
    {
        cardData = _cardData;
    }
}

public class JsonTest : MonoBehaviour
{
    string path = Application.streamingAssetsPath + "/Data/stageData.json";

    List<CardData> cardData = new List<CardData>();

    static JsonTest instance = null; 

    public static JsonTest Instace()
    {
        return instance;
    } 

    void Awake()
    {
        Debug.Log(path);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);   
    }

    public void AddData(CardData data)
    {
        cardData.Add(data);
    }

    public void RemoveData(CardData data, int index)
    {
        cardData.RemoveAt(index);
    }

    public void InsertData(CardData data, int index)
    {
        cardData.Insert(index, data);
    }

    public void SaveData()
    {
        string temp = SerializeToJson();
        CreateJsonFile(temp);
    }

    public List<CardData> LoadData()
    {
        string temp = SerializeToJson();
        DeserializeFromJson(temp);

        return cardData;
    }

    string SerializeToJson()
    {       
        CardDataContainer container = new CardDataContainer(cardData);

        string json = JsonUtility.ToJson(container);

        Debug.Log(json);

        return json;
    }

    void DeserializeFromJson(string json)
    {
        CardDataContainer container = JsonUtility.FromJson<CardDataContainer>(json);

        Debug.Log(container.cardData.Count);
        for (int i = 0; i < container.cardData.Count; i++)
        {
            cardData[i] = container.cardData[i];
            Debug.Log(container.cardData[i].cardLevel + " " + container.cardData[i].cardName + " " + container.cardData[i].cardType);
        }
    }

    void CreateJsonFile(string jsonData)
    {
        FileStream filestream = new FileStream(path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        filestream.Write(data, 0, data.Length);
        filestream.Close();
    }
}
