using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    static QuestManager instance = null;

    public static QuestManager Instance()
    {
        return instance;
    }

    public TextMeshProUGUI questSentence;
    public Image questPanel = null;

    List<Dictionary<string, object>> questData;
    private bool isEnd = false;

    private LoadCSV csvLoader;

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
        csvLoader = FindObjectOfType<LoadCSV>().GetComponent<LoadCSV>();
        questData = csvLoader.GetData("Quest");

        questPanel.gameObject.SetActive(false);
    }

    public void StartQuest(int questIndex)
    {
        Dictionary<string, object> loadedData = questData[questIndex];

        if (loadedData["isCleared"].ToString() == "no")
        {
            questSentence.text = questData[questIndex]["content"].ToString();
            StartCoroutine("ShowQuestPanel");
        }
        else if (loadedData["isCleared"].ToString() != "no")
        {
            Debug.Log("해당 퀘스트는 이미 클리어 되었습니다.");
        }
    }

    public void ClearQuest(int questIndex)
    {
        if (questData[questIndex]["isCleared"].ToString() == "no")
        {
            questData[questIndex]["isCleared"] = "yes";

            // Debug
            Debug.Log(questIndex + "번 퀘스트 클리어했습니다.");

            StartCoroutine("ShowQuestPanel");
        }
    }

    public bool CheckQuestState(int questIndex)
    {
        bool temp = false;
        switch (questData[questIndex]["isCleared"].ToString())
        {
            case "yes":
                temp = true;
                break;
            case "no":
                temp = false;
                break;
        }
        return temp;
    }

    public void Update()
    {
        if (isEnd) // 퀘스트 표시가 끝나면 오브젝트 비활성화.
        {
            questPanel.gameObject.SetActive(false);
        }
    }

    IEnumerator ShowQuestPanel()
    {
        isEnd = false;

        questPanel.gameObject.SetActive(true);

        for (float i = 0f; i < 1f; i += 0.05f)
        {
            questSentence.color = new Color(questSentence.color.r, questSentence.color.g, questSentence.color.b, i);
            questPanel.color = new Color(questPanel.color.r, questPanel.color.g, questPanel.color.b, i); 
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2.0f);

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            questSentence.color = new Color(questSentence.color.r, questSentence.color.g, questSentence.color.b, i);
            questPanel.color = new Color(questPanel.color.r, questPanel.color.g, questPanel.color.b, i); 
            yield return new WaitForSeconds(0.05f);
        }        

        isEnd = true;
    }
}
