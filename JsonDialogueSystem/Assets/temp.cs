using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using LitJson;
using TMPro;

public enum QUESTTYPE { NORMAL, CLEAR };
public enum TYPE { NORMAL, SMALL };

public class DialogueInfo
{
    public int CurrentKey;
    public string Name;
    public string Sentence;
    public string Category;
    public int EventNum;
    public int NextKey;

    public DialogueInfo(int current_key, string name, string sentence, string category, int event_num, int next_key)
    {
        CurrentKey = current_key;
        Name = name;
        Sentence = sentence;
        Category = category;
        EventNum = event_num;
        NextKey = next_key;        
    }
}

public class temp : MonoBehaviour
{
    // JSON 파일 저장시에 사용 (예정)
    public List<DialogueInfo> dialogueInfoList = new List<DialogueInfo>();

    // 대화 관련 변수들
    public GameObject dialogueP = null;
    public TextMeshProUGUI dialogueNameT = null;
    public TextMeshProUGUI dialogueSentenceT = null;

    // 선택지 관련 변수들
    public GameObject choiceP = null;
    public GameObject[] choiceB = new GameObject[3];

    // 퀘스트 관련 변수들
    public GameObject questP = null;
    public TextMeshProUGUI questNameT = null;
    public TextMeshProUGUI questSentenceT = null;

    private int nextIndex = 0;    

    private JsonData dialogueData = null; // 대화 데이터 파일
    private JsonData questData = null; // 퀘스트 데이터 파일

    private JsonData newDialogueData = null; // 변경 후 파일을 저장할 때 사용 (예정)

    private Queue<JsonData> sentences = new Queue<JsonData>(); // 대화 데이터 파일에서 뽑아서 집어넣을 큐

    // ----- 유니티 라이프 사이클

    void Start()
    {
        dialogueP.SetActive(false);
        choiceP.SetActive(false);
        questP.SetActive(false);

        dialogueData = LoadDialogueInfo("DialogueData.json"); // 데이터 JSON 파일 로딩
        questData = LoadDialogueInfo("Quest.json");
    }

    // ----- JSON 파일 로드 / 세이브 관련 함수들

    public void SaveDialogueInfo()
    {
        Debug.Log("대화정보 저장.");

        dialogueInfoList.Add(new DialogueInfo(0, "블루넷", "안녕 내 이름은 아 몰라 시발", "Idle", 0, 1));
        dialogueInfoList.Add(new DialogueInfo(1, "블루넷", "난 그냥 갈거야!", "Idle", 0, 2));

        JsonData infoJson = JsonMapper.ToJson(dialogueInfoList);

        byte[] byteForEncoding = Encoding.UTF8.GetBytes(infoJson.ToString());
        string encodingedString = Encoding.UTF8.GetString(byteForEncoding);

        File.WriteAllText(Application.dataPath + "/Resources/Data/DialogueData.json", encodingedString);
    }

    public JsonData LoadDialogueInfo(string filename)
    {
        Debug.Log("파일에서 추출 시작.");

        JsonData temp = new JsonData();

        if (File.Exists(Application.dataPath + "/Resources/Data/" + filename))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + "/Resources/Data/" + filename);

            temp = JsonMapper.ToObject(jsonStr);

            Debug.Log("JsonData 생성 완료.");            
        }
        else Debug.Log("파일이 존재하지 않습니다.");

        return temp;
    }
  
    public void ModifyDialogueInfo()
    {
        string json = ("[");

        for (int i = 0; i < dialogueData.Count; i++)
        {
            newDialogueData = JsonMapper.ToJson(dialogueData[i]);
            if (i == dialogueData.Count - 1)
            {
                json = (json + newDialogueData.ToString());
            }
            else
            {
                json = (json + newDialogueData.ToString() + ",");
            }
        }
        json = (json + "]");

        File.WriteAllText(Application.dataPath + "/Resources/Data/DialogueData.json", json);
    }

    // ----- 대화창 관련 함수들

    /// <summary>
    /// 대화를 시작하는 함수입니다.
    /// </summary>
    public void StartDialogue(int startIndex)
    {
        dialogueP.SetActive(true);

        sentences.Clear();
        sentences.Enqueue(dialogueData[startIndex]);

        DisplayNextSentence();
    }

    /// <summary>
    /// 다음 대화로 넘어가도록 하는 함수입니다.
    /// </summary>
    public void DisplayNextSentence()
    {
        //HideQuestPanel();
        ClearChoicePanel(); // 이전에 있는 선택지 대화창을 비활성화 합니다.

        dialogueNameT.text = "";
        dialogueSentenceT.text = "";

        if (sentences.Count == 0)
        {
            Debug.Log("대화 데이터 큐가 비었습니다. 데이터를 불러올 수 없습니다.");
            EndDialogue();
            return;
        }

        JsonData sentence = sentences.Dequeue(); // 큐에서 데이터를 하나 뺍니다.

        // 한글자씩 출력하는 코루틴이 있을 경우, 여기에 넣으세요!

        dialogueNameT.text = sentence["Name"].ToString();
        dialogueSentenceT.text = sentence["Sentence"].ToString();

        CheckDialogueState(sentence); // 대화 상태를 확인하고 업데이트합니다.
    }     

    /// <summary>
    /// 대화가 끝났을 때 호출되는 함수입니다.
    /// </summary>
    void EndDialogue()
    {
        sentences.Clear();
        dialogueP.SetActive(false);
        Debug.Log("대화가 종료되었습니다.");
    }

    /// <summary>
    /// 대화 상태를 확인하고 추가 작업을 하는 함수입니다.
    /// </summary>
    /// <param name="sentence">상태를 확인할 대화 데이터입니다.</param>
    void CheckDialogueState(JsonData sentence)
    {
        // 만약 분류가 "종료"나 "퀘스트"일 경우...
        if (sentence["Category"].ToString() == "종료" || sentence["Category"].ToString() == "퀘스트")
        {
            if (sentence["Category"].ToString() == "퀘스트")
            {
                Debug.Log("들어가욧");
                UnlockQuest(Convert.ToInt32(sentence["EventNum"].ToString()));
            }

            //EndDialogue();
            return;
        }
        else
        {
            nextIndex = Convert.ToInt32(sentence["NextKey"].ToString()); // 다음으로 출력할 대사 번호를 불러옵니다.
        }

        if (nextIndex != -1)
        {
            sentences.Enqueue(dialogueData[nextIndex]); // 다음으로 출력할 대사 데이터를 큐에 넣습니다.
        }

        // 만약 분류가 "질문"일 경우...
        if (sentence["Category"].ToString() == "질문")
        {
            int currentIndex = Convert.ToInt32(sentence["CurrentKey"].ToString()); // 현재 대화의 번호
            int questionRange = Convert.ToInt32(sentence["EventNum"].ToString()); // 선택지의 갯수

            choiceP.SetActive(true);

            for (int i = 0; i < questionRange; i++)
            {
                choiceB[i].SetActive(true);
                choiceB[i].GetComponentInChildren<Text>().text = dialogueData[currentIndex + (i + 1)]["Sentence"].ToString();

                AddListener(choiceB[i], Convert.ToInt32(dialogueData[currentIndex + (i + 1)]["NextKey"].ToString())); // OnClick 이벤트 추가
            }
        }
    }

    void AddDialogue(int index)
    {
        sentences.Clear();
        sentences.Enqueue(dialogueData[index]);

        DisplayNextSentence();
    }

    void AddListener(GameObject button, int index)
    {
        button.GetComponent<Button>().onClick.AddListener(() => AddDialogue(index));
    }

    /// <summary>
    /// 선택지와 내용을 지우고 UI를 비활성화 시키는 함수입니다.
    /// </summary>
    void ClearChoicePanel()
    {
        for (int i = 0; i < 3; i++)
        {
            choiceB[i].GetComponentInChildren<Text>().text = "";
        }
        choiceP.SetActive(false); // 선택지 버튼들의 부모인 패널을 비활성화 합니다.
    }

    // ----- 퀘스트 관련 함수들

    /// <summary>
    /// 퀘스트를 표시해야 할 때 호출되는 함수입니다.
    /// </summary>
    /// <param name="index">출력되어야 하는 데이터의 위치값.</param>
    public void DisplayQuestPanel(int index, QUESTTYPE questType)
    {
        questP.SetActive(true);

        if (questType == QUESTTYPE.CLEAR)
        {
            questP.GetComponent<Image>().color = Color.green;
        }

        questNameT.text = questData[index]["Name"].ToString();
        questSentenceT.text = questData[index]["Text"].ToString();
    }

    /// <summary>
    /// 퀘스트를 숨겨야 할 때 호출되는 함수입니다.
    /// </summary>
    public void HideQuestPanel()
    {      
        questNameT.text = "";
        questSentenceT.text = "";

        questP.SetActive(false);
    }

    public void UnlockQuest(int index)
    {
        questData[index]["Unlock"] = "True";

        DisplayQuestPanel(index, QUESTTYPE.NORMAL);
    }

    public void CompleteQuest(int index)
    {
        if (questData[index]["Unlock"].ToString() == "False")
        {
            Debug.Log("퀘스트가 열리지 않았습니다. 먼저 해당 퀘스트를 언락해주세요.");
            return;
        }
        questData[index]["IsComplete"] = "Completed";

        DisplayQuestPanel(index, QUESTTYPE.CLEAR);
    }

    public void CompleteQuest(int index, int dialogueStartIndex)
    {
        if (questData[index]["Unlock"].ToString() == "False")
        {
            Debug.Log("퀘스트가 열리지 않았습니다. 먼저 해당 퀘스트를 언락해주세요.");
            return;
        }
        questData[index]["IsComplete"] = "Completed";

        DisplayQuestPanel(index, QUESTTYPE.CLEAR);

        StartDialogue(dialogueStartIndex);
    }

    public bool GetQuestComplete(int index)
    {
        if (questData[index]["Unlock"].ToString() == "False")
        {
            //Debug.Log("퀘스트가 열리지 않았습니다. 먼저 해당 퀘스트를 언락해주세요.");
            return false;
        }
        else
        {
            if (questData[index]["IsComplete"].ToString() == "Completed")
                return true;
            else if (questData[index]["IsComplete"].ToString() == "NotCompleted")
                return false;
            else
            {
                Debug.Log("무언가 잘못됬습니다.");
                return false;
            }                
        }     
    }
}
