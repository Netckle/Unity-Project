using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSlimesController : MonoBehaviour
{
    public GameObject[] miniSlimes = new GameObject[6];
    public Vector3[] firstPos = new Vector3[6];

    void Start()
    {
        for (int i = 0; i < miniSlimes.Length; ++i)
        {
            firstPos[i] = miniSlimes[i].transform.position;
            miniSlimes[i].SetActive(false);
        }
    } 

    public bool miniSlimeIs()
    {
        for (int i = 0; i < miniSlimes.Length; i++)
        {
            if (miniSlimes[i].activeSelf)
            {
                return false;
            }            
        }
        return true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SpawnSlime(2);
        }
    }

    public void SpawnSlime(int length)
    {
        for (int i = 0; i < miniSlimes.Length; ++i)
        {
            miniSlimes[i].SetActive(false);
        }

        int[] pos = getRandomInt(length, 0, miniSlimes.Length);


        foreach (int num in pos)
        {
            miniSlimes[num].transform.position = firstPos[num];
            miniSlimes[num].SetActive(true);
            Camera.main.GetComponent<MultipleTargetCamera>().targets.Add(miniSlimes[num].transform);
            miniSlimes[num].GetComponent<MiniSlimeMove>().particle.Play();
        }
    }

    public int[] getRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;

                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }

    // 살짝 사라지면서 이동!
    public PlayerMovement player;

    public GameObject[] lines = new GameObject[3];

    void test()
    {
        int temp = player.currentLine;

        Vector3 tempPos = lines[temp].transform.position + new Vector3(0, 3, 0);

        // 페이드 인? 아웃?

        transform.position = tempPos;

        //  페이드 인 아웃
    
    }
}
