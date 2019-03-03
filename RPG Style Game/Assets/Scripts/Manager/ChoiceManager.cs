using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;

    private AudioManager the_audio;

    private string question; // choice.question 복사.
    private List<string> answer_list; // choice.answer 배열 복사.

    public GameObject go; // 평소에 비활성화.

    public Text question_text;
    public Text[] answer_text;
    public GameObject[] answer_panel;

    public Animator anim;

    public string key_sound; 
    public string enter_sound;

    public bool is_choicing; // 람다식 대기를 위한 조건.
    private bool key_input; // 선택창이 없으면, 키입력 불가.

    private int count; // 추가 선택지의 갯수. (기본은 1개)
    private int result; // 선택한 선택창.

    private WaitForSeconds  wait_time = new WaitForSeconds(0.01f);

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    void Start () 
    {
        the_audio = FindObjectOfType<AudioManager>();
        answer_list = new List<string>();

        for(int i = 0; i < answer_text.Length; i++)
        {
            answer_text[i].text = "";
            answer_panel[i].SetActive(false);
        }
        
        question_text.text = "";
    }

    void Update()
    {
        if (key_input)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                the_audio.Play(key_sound);
                if (result > 0)
                    result--;
                else
                    result = count;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                the_audio.Play(key_sound);
                if (result < count)
                    result++;
                else
                    result = 0;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                the_audio.Play(enter_sound);
                key_input = false;
                ExitChoice();
            }
        }
    }  

    public void ShowChoice(Choice _choice)
    {
        is_choicing = true;

        go.SetActive(true);
        result = 0;
        question = _choice.question;

        for(int i = 0; i< _choice.answers.Length; i++)
        {
            answer_list.Add(_choice.answers[i]);
            answer_panel[i].SetActive(true);
            count = i;
        }

        anim.SetBool("Appear", true);
        Selection();
        StartCoroutine(ChoiceCoroutine());
    }

    public void ExitChoice()
    {
        question_text.text = "";

        for (int i = 0; i <= count; i++)
        {
            answer_text[i].text = "";
            answer_panel[i].SetActive(false);
        }

        answer_list.Clear();
        anim.SetBool("Appear", false);

        is_choicing = false;
        go.SetActive(false);
    }

    public int GetResult()
    {
        return result;
    }

    public void Selection()
    {
        Color color = answer_panel[0].GetComponent<Image>().color;
        color.a = 0.75f;
        for(int i = 0; i <= count; i++)
        {
            answer_panel[i].GetComponent<Image>().color = color;
        }
        color.a = 1f;
        answer_panel[result].GetComponent<Image>().color = color;
    }

    IEnumerator ChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(TypingQuestion());

        StartCoroutine(TypingAnswer_0());

        if(count >= 1)
        {
            StartCoroutine(TypingAnswer_1());
        }
        if (count >= 2)
        {
            StartCoroutine(TypingAnswer_2());
        }
        if (count >= 3)
        {
            StartCoroutine(TypingAnswer_3());
        }

        yield return new WaitForSeconds(0.5f);
        key_input = true;
    }

    // Typing Coroutine

    IEnumerator TypingQuestion()
    {
        for(int i = 0; i < question.Length; i++)
        {
            question_text.text += question[i];
            yield return wait_time;
        }
    }

    IEnumerator TypingAnswer_0()
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < answer_list[0].Length; i++)
        {
            answer_text[0].text += answer_list[0][i];
            yield return wait_time;
        }
    }

    IEnumerator TypingAnswer_1()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < answer_list[1].Length; i++)
        {
            answer_text[1].text += answer_list[1][i];
            yield return wait_time;
        }
    }

    IEnumerator TypingAnswer_2()
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < answer_list[2].Length; i++)
        {
            answer_text[2].text += answer_list[2][i];
            yield return wait_time;
        }
    }

    IEnumerator TypingAnswer_3()
    {
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < answer_list[3].Length; i++)
        {
            answer_text[3].text += answer_list[3][i];
            yield return wait_time;
        }
    }
}
