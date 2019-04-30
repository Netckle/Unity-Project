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

    public void StartEvent(int key) // key는 두자리 숫자.
    {
        int category = (key / 10);
        int index = (key % 10);

        switch (category)
        {
            case 1:
                GetQuest(index);
                break;
            case 2:
                GetItem(index);
                break;
            case 3:
                GetStat(index);
                break;
        }
    }
    public GameObject[] items;
    void GetItem(int key)
    {
        Inventory.Instance().AddItem(items[key].GetComponent<Item>());
        Debug.Log(items[key].gameObject.name + " 아이템을 습득했습니다.");
    }

    void GetQuest(int key)
    {
        int temp = key % 10;

        if (!GameManager.Instance().questM.CheckQuestState(temp))
			GameManager.Instance().questM.StartQuest(temp);
    }

    void GetStat(int key)
    {
        Debug.Log("스텟이 업그레이드 되었습니다.");
    }
}
