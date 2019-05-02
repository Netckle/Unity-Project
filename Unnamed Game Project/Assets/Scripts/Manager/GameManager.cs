using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    static GameManager instance = null;

    public DialogueManager      dialgoueM;
    public QuestManager         questM;
    public JsonManager          jsonM;
    public ObjectManager        objectM;
    public StageManager         stageM;
    public SceneChangeManager   sceneChangeM;
    public CanvasManager        canvasM;

    public LoadCSV loadCSV;

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

        dialgoueM       = FindManager<DialogueManager>      (0);
        questM          = FindManager<QuestManager>         (1);
        jsonM           = FindManager<JsonManager>          (2);
        objectM          = FindManager<ObjectManager>        (3);
        stageM          = FindManager<StageManager>         (4);
        sceneChangeM    = FindManager<SceneChangeManager>   (5);
        canvasM         = FindManager<CanvasManager>        (6);

        loadCSV = FindManager<LoadCSV>(7);
    }

    T FindManager<T>(int index)
    {
        T tempData = transform.GetChild(index).GetComponent<T>();

        if (tempData == null)
            Debug.LogError(transform.GetChild(index).gameObject.name + " 데이터 불러오기 실패.");
        
        return tempData;
    }    
}
