using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSlimesController : MonoBehaviour
{
    public PlayerMovement player;

    public GameObject[] miniSlimes = new GameObject[6];
    public GameObject[] lines = new GameObject[3];

    private Vector3[] spawnedPos = new Vector3[6];  

    void Start()
    {
        SaveSlimesPos();
    } 

    private void SaveSlimesPos()
    {
        for (int i = 0; i < miniSlimes.Length; ++i)
        {
            // 미니 슬라임은 Destroy 하지 않고 SetActive 로 제어해야하기 때문에 위치 저장 필요.
            spawnedPos[i] = miniSlimes[i].transform.position;
            miniSlimes[i].SetActive(false);
        }
    }

    public bool CheckSlimeIsActive()
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

    public void SpawnMiniSlimes(int count)
    {
        // 혹시라도 남아있는 슬라임은 끈다.
        for (int i = 0; i < miniSlimes.Length; ++i)
        {
            miniSlimes[i].SetActive(false);
        }

        int[] pos = getRandomInt(count, 0, miniSlimes.Length);

        foreach (int num in pos)
        {
            miniSlimes[num].transform.position = spawnedPos[num];
            miniSlimes[num].SetActive(true);

            Camera.main.GetComponent<MultipleTargetCamera>().targets.Add(miniSlimes[num].transform);
            
            miniSlimes[num].GetComponent<MiniSlimeMove>().particle.Play();
        }
    }

    // 중복되지 않는 정수를 length 갯수만큼 생성합니다.
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
}
