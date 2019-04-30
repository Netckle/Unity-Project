using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    static GameManager instance = null;

    // Manager
    [HideInInspector]
    public DialogueManager  dialgoueM;
    [HideInInspector]
    public QuestManager     questM;
    [HideInInspector]
    public JsonTest         jsonM;
    [HideInInspector]
    public SpawnManager     spawnM;
    [HideInInspector]
    public StageManager     stageM;
    [HideInInspector]
    public ChangeScene      changeSceneM;
    [HideInInspector]
    public IntroSceneManager sceneM;

    public static GameManager Instance()
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

        dialgoueM       = FindManager<DialogueManager>  (0);
        questM          = FindManager<QuestManager>     (1);
        jsonM           = FindManager<JsonTest>         (2);
        spawnM          = FindManager<SpawnManager>     (3);
        stageM          = FindManager<StageManager>     (4);
        changeSceneM    = FindManager<ChangeScene>      (5);
        sceneM          = FindManager<IntroSceneManager>(6);

        UpdateManager();
    }

    T FindManager<T>(int index)
    {
        T tempData = transform.GetChild(index).GetComponent<T>();

        if (tempData == null)
            Debug.LogError(transform.GetChild(index).gameObject.name + " 데이터 불러오기 실패.");
        
        return tempData;
    }    

    public void UpdateManager()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Dungeon Scene Play":
                spawnM.gameObject.SetActive(true);
                spawnM.UpdatePortalState();
                stageM.GenerateStage();
                break;
            case "Dungeon Scene Select":
                spawnM.gameObject.SetActive(false);
                break;
        }
    }
}
