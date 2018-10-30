using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 배틀칩의 속성 열거형
public enum STATE
{
    NORMAL,
    FIRE,
    ICE
}

public class BattleChip : MonoBehaviour
{

    public STATE chipState = STATE.NORMAL;

    public float damage;
    public float coolTime;

    public int maxIndex; // 3

    [TextArea(3, 10)]
    public string explanation;

    public Vector3[] chipPoints;
    private Vector3[] chipPoints_backup;
    public GameObject colliderSpace;
    private GameObject[] chipColliders;

    public bool onSkill = false;

    void Start()
    {
        
    }

    // 배틀칩의 변수들을 초기화하는 함수
    public void Instantiate()
    {
        chipColliders = new GameObject[maxIndex];
        chipPoints_backup = new Vector3[maxIndex];

        switch (chipState)
        {
            case STATE.NORMAL:
                {

                }
                break;
            case STATE.FIRE:
                {

                }
                break;
            case STATE.ICE:
                {

                }
                break;
        }
    }

    // 배틀칩에 저장된 경로의 시작점을 플레이어의 위치를 기준으로 재설정하는 함수
    public void AttachToPlayerPos(GameObject player)
    {
        for (int i = 0; i < maxIndex; i++) 
        {
            chipPoints_backup[i].Set(
                player.transform.position.x + chipPoints[i].x,
                player.transform.position.y + chipPoints[i].y,
                player.transform.position.z + chipPoints[i].z
                );
        }
    }

    // 배틀칩의 기록된 범위와 경로에 콜라이더 생성 및 제거하는 함수
    public IEnumerator executeAct()
    {
        if (onSkill)
            yield break;

        onSkill = true;

        for (int i = 0; i < maxIndex; i++)
        {
            chipColliders[i] = Instantiate(colliderSpace, chipPoints_backup[i], Quaternion.identity) as GameObject;

            yield return new WaitForSeconds(coolTime);
        }

        yield return new WaitForSeconds(0.1F);

        for (int i = 0; i < maxIndex; i++)
        {
            Destroy(chipColliders[i]);

            yield return new WaitForSeconds(coolTime);
        }

        onSkill = false;
    }
}
