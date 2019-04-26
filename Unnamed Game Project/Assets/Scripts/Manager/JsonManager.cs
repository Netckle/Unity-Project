using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

using Newtonsoft.Json;

public class JsonManager : MonoBehaviour
{
    string path = Application.streamingAssetsPath + "/Data";
    [HideInInspector]
    public List<CardData> cardData = new List<CardData>();

    // Singleton
    static JsonManager instance = null; 

    public static JsonManager Instace()
    {
        return instance;
    } 

    void Awake()
    {
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

    public void Save()
    {
        string data = ObjectToJson(cardData);

        CreateJsonFile(path, "cardData", data);
        Debug.Log(path + "/cardData.json");
    }

    public List<CardData> Load()
    {
        List<CardData> mapDataTemp = new List<CardData>();

        if (File.Exists(path + "/cardData.json"))
        {
            Debug.Log("파일 존재함.");
            string jsonStr = File.ReadAllText(path + "/cardData.json");

            mapDataTemp = JsonToObject<List<CardData>>(jsonStr);
        }

        return mapDataTemp;
    }

    string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    T JsonToObject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream filestream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        filestream.Write(data, 0, data.Length);
        filestream.Close();
    }
}
