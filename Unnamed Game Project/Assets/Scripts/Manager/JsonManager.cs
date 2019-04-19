using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonManager : MonoBehaviour
{
    string path = Application.dataPath + "/StreamingAssets/Data/MapData.json";

    static JsonManager instance = null; 

    public static JsonManager Instace()
    {
        return instance;
    } 

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
        File.WriteAllText(path, data);
        //UnityEditor.AssetDatabase.Refresh(); 
    }

    public List<JsonData> LoadMapDataJson()
    {
        List<JsonData> mapDataTemp = new List<JsonData>();

        if (File.Exists(path))
        {
            string jsonStr = File.ReadAllText(path);

            mapDataTemp = JsonConvert.DeserializeObject<List<JsonData>>(jsonStr);
        }

        return mapDataTemp;
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
}
