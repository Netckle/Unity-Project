using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonManager : MonoBehaviour
{
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
    //-----

    public List<JsonData> mapData = new List<JsonData>();

    public void AddData(JsonData data)
    {
        mapData.Add(data);
    }

    public void RemoveData(JsonData data, int index)
    {
        mapData.RemoveAt(index);
    }

    public void InsertData(JsonData data, int index)
    {
        mapData.Insert(index, data);
    }

    public void Save()
    {
        string data = JsonConvert.SerializeObject(mapData, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Resources/Data/MapData.json", data);
        UnityEditor.AssetDatabase.Refresh(); 
    }

    public List<JsonData> LoadMapDataJson()
    {
        List<JsonData> mapDataTemp = new List<JsonData>();

        if (File.Exists(Application.dataPath + "/Resources/Data/MapData.json"))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + "/Resources/Data/MapData.json");

            mapDataTemp = JsonConvert.DeserializeObject<List<JsonData>>(jsonStr);
        }

        return mapDataTemp;
    }
}
