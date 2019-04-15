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

    private LoadCSV csvLoader;

    List<Dictionary<string, object>> quest;
    private bool isEnd = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        csvLoader = GameObject.Find("Load CSV").GetComponent<LoadCSV>();

        quest = csvLoader.GetData("퀘스트");

        questPanel.gameObject.SetActive(false);
    }

    public void StartQuest(int questIndex)
    {
        Dictionary<string, object> loadedData = quest[questIndex];

        if (loadedData["클리어 여부"].ToString() == "아님")
        {
            questSentence.text = quest[questIndex]["내용"].ToString();
            StartCoroutine("ShowQuestPanel");
        }
        else if (loadedData["클리어 여부"].ToString() != "아님")
        {
            Debug.Log("해당 번호의 퀘스트를 진행할 수 없습니다.");
        }
    }

    public void Update()
    {
        if (isEnd)
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
