using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove
{
    [Tooltip("체크하면 NPC가 움직입니다.")]
    public bool     isMoving;

    public string[] direction;

    [Range(1, 5)] [Tooltip("1 : 천천히, 2 : 조금 천천히, 3 : 보통, 4 : 조금 빠르게, 5 : 연속")]
    public int      frequency;
}

public class NPCManager : MovingObject
{
    public NPCMove  npc;

    void Start()
    {
        queue = new Queue<string>();
        SetMove();
    }

    public void SetMove()
    {
        if (npc.isMoving)
        {
            StartCoroutine(MoveCoroutine());
        }
    }

    public void SetNotMove()
    {
        StopAllCoroutines();
    }

    // MovingObject에 Move함수를 실행하기 위한 코루틴
    IEnumerator MoveCoroutine()
    {
        if (npc.direction.Length != 0)
        {
            for (int i = 0; i < npc.direction.Length; i++)
            {
                yield return new WaitUntil(() => queue.Count < 2);

                base.Move(npc.direction[i], npc.frequency);

                if (i == npc.direction.Length - 1)
                {
                    i = -1; // 원상복귀
                }
            }
        }
    }
}
