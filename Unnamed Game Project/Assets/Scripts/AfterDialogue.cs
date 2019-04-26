using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterDialogue : MonoBehaviour
{
    static AfterDialogue instance = null;

    public static AfterDialogue Instance()
    {
        return instance;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	void Start () 
	{
		if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void StartEvent(int key)
    {
        switch (key / 10)
        {
            case 1:
                GetQuest(key);
                break;
            case 2:
                GetItem(key);
                break;
            case 3:
                GetStat(key);
                break;
        }
    }

    void GetItem(int key)
    {
        Debug.Log(key + "번 아이템을 습득했습니다.");
    }

    void GetQuest(int key)
    {
        int temp = key % 10;

        if (!QuestManager.Instance().CheckQuestState(temp))
			QuestManager.Instance().StartQuest(temp);
    }

    void GetStat(int key)
    {
        Debug.Log("스텟이 업그레이드 되었습니다.");
    }
}
