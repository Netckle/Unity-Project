using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;
using System.Text;

public class JsonManager : MonoBehaviour
{
    string path = Application.streamingAssetsPath + "/Data/";

    List<CardData> cardData = new List<CardData>();

    // Controll data before saved.

    public void Add(CardData data)
    {
        cardData.Add(data);
    }

    public void Remove(CardData data, int index)
    {
        cardData.RemoveAt(index);
    }

    public void Insert(CardData data, int index)
    {
        cardData.Insert(index, data);
    }

    // Save & Load

    public void Save(string fileName)
    {
        string data = SerializeToJson();
        CreateJsonFile(data, fileName);
    }

    public List<CardData> Load()
    {
        string data = "";

        if (File.Exists(path + "cardData.json"))
        {
            data = File.ReadAllText(path + "cardData.json");
            Debug.Log(data);
        }
        //string data = SerializeToJson();
        DeserializeFromJson(data);

        return cardData;
    }

    // There is no need to modify it.

    string SerializeToJson()
    {       
        CardDataContainer container = new CardDataContainer(cardData);
        string json = JsonUtility.ToJson(container, prettyPrint:true);

        return json;
    }

    void DeserializeFromJson(string json)
    {
        CardDataContainer container = JsonUtility.FromJson<CardDataContainer>(json);

        for (int i = 0; i < container.cardData.Count; i++)
        {
            cardData[i] = container.cardData[i];
        }
    }

    void CreateJsonFile(string jsonData, string fileName)
    {
        FileStream filestream = new FileStream(path + fileName, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);

        filestream.Write(data, 0, data.Length);
        filestream.Close();
    }
}
