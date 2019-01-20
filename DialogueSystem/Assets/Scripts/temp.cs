using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class temp : MonoBehaviour
{
    private List<Dictionary<string, object>> data = null;
    private Queue<Dictionary<string, object>> sentences = null;
    private int count = 0;

    public TextMeshProUGUI sentenceText = null;
    public GameObject buttonPrefab = null;    
    public GameObject choicePanel = null; 
    [Range(0, 100)]
    public int space = 0;
    public int startIndex = 0;    

    void Start()
    {
        data = CSVReader.Read("Dialogue"); // CSV 데이터 로딩
        sentences = new Queue<Dictionary<string, object>>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartDialogue();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            NextSentence();           
        }
    }

    public void StartDialogue()
    {
        sentences.Enqueue(data[startIndex]);

        NextSentence();
    }

    public void NextSentence()
    {
        if (sentences.Count == 0)
        {
            Debug.Log("대화 데이터 큐가 비었습니다. 데이터를 불러올 수 없습니다.");
            return;
        }

        Dictionary<string, object> sentence = sentences.Dequeue();

        sentenceText.text = sentence["Text"].ToString(); // 대화 텍스트를 출력합니다.

        // (예정) 이곳에 텍스트 출력 코루틴을 추가합니다.

        // ---------- [대화 종료 기능을 담당하는 부분입니다.]        
        if (sentence["NextIndex"].ToString() == "End")
        {
            EndDialogue();
            return;
        }
        else 
        {
            count = Convert.ToInt32(sentence["NextIndex"].ToString()); // 다음 대사의 Index값을 저장합니다.
        }               

        if (count != -1) // 다음 대사가 있다면...
        {
            sentences.Enqueue(data[count]); // 다음 대사 데이터를 큐에 넣습니다.
        }

        // ---------- [선택지 기능을 담당하는 부분입니다.]
        if (sentence["Event"].ToString() == "Question") // 대화 데이터의 유형이 질문(Question)유형이라면...
        {
            int CurrentIndex  = Convert.ToInt32(sentence["Index"].ToString()); // 현재 대화 데이터의 번호
            int QuestionRange = Convert.ToInt32(sentence["EventIndex"].ToString()); // 선택지의 갯수

            for (int i = 1; i <= QuestionRange; i++)
            {
                GameObject choiceButton = (GameObject)Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity); // 선택지 버튼을 동적으로 생성합니다.

                choiceButton.GetComponentInChildren<Text>().text = data[CurrentIndex + i]["Text"].ToString(); // 선택지에 적힐 내용을 불러옵니다.

                choiceButton.transform.position += new Vector3(choicePanel.transform.position.x, choicePanel.transform.position.y - (space * i), 0);
                choiceButton.transform.SetParent(choicePanel.transform);
                
                AddListener(choiceButton, CurrentIndex + QuestionRange + i); // 동적 생성한 버튼에 클릭시 실행되는 기능을 추가합니다. 
            }            
        }
    }

    // 해당 인덱스의 대화 데이터를 큐에 집어넣는 함수입니다.
    void AddSentence(int index)
    {
        sentences.Clear(); // 남아있는 대화 데이터를 비웁니다.

        sentences.Enqueue(data[index]);

        NextSentence();
    }

    // 선택지 버튼을 클릭 시 AddSentence가 실행되도록 만든 OnClick.AddListener함수입니다.
    void AddListener(GameObject button, int index)
    {
        button.GetComponent<Button>().onClick.AddListener(() => AddSentence(index));
    }    

    // 대화가 종료되었음을 알리는 함수입니다.
    void EndDialogue()
    {
        sentences.Clear();
        Debug.Log("대화 종료됨.");
    }
}
