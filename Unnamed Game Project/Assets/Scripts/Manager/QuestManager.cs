using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questContent;
    public Image questPanel = null;

    private List<Dictionary<string, object>> questData;
    private bool isEnd = false;

    private LoadCSV csvLoader;

    void Start()
    {
        csvLoader = FindObjectOfType<LoadCSV>().GetComponent<LoadCSV>();
        questData = csvLoader.GetData(DATATYPE.QUEST);

        questPanel.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (isEnd) 
        {
            questPanel.gameObject.SetActive(false);
        }
    }

    public void FirstStart(int index)
    {
        Dictionary<string, object> loadedData = questData[index];

        if (loadedData["isCleared"].ToString() == "no")
        {
            questContent.text = questData[index]["content"].ToString();
            StartCoroutine("ShowQuestPanel");
        }
        else if (loadedData["isCleared"].ToString() == "yes")
        {
            Debug.Log("해당 퀘스트는 이미 클리어 되었습니다.");
        }
    }

    public void Clear(int questIndex)
    {
        if (questData[questIndex]["isCleared"].ToString() == "no")
        {
            questData[questIndex]["isCleared"] = "yes";

            // Debug
            Debug.Log(questIndex + "번 퀘스트 클리어했습니다.");

            StartCoroutine("ShowQuestPanel");
        }
    }

    public bool CheckState(int questIndex)
    {
        bool isCleared = false;

        switch (questData[questIndex]["isCleared"].ToString())
        {
            case "yes":
                isCleared = true;
                break;
            case "no":
                isCleared = false;
                break;
        }
        return isCleared;
    }

    private IEnumerator ShowPanel()
    {
        isEnd = false;

        questPanel.gameObject.SetActive(true);

        for (float i = 0f; i < 1f; i += 0.05f)
        {
            questContent.color = new Color(questContent.color.r, questContent.color.g, questContent.color.b, i);
            questPanel.color = new Color(questPanel.color.r, questPanel.color.g, questPanel.color.b, i); 
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2.0f);

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            questContent.color = new Color(questContent.color.r, questContent.color.g, questContent.color.b, i);
            questPanel.color = new Color(questPanel.color.r, questPanel.color.g, questPanel.color.b, i); 
            yield return new WaitForSeconds(0.05f);
        }        

        isEnd = true;
    }
}
