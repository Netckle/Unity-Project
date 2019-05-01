using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNext : MonoBehaviour
{
    private Vector3 previousPos;
    private Vector3 nextPos;
    public float delayTime;

    private WaitForSeconds wait;

    void Start()
    {
        wait = new WaitForSeconds(delayTime);
    }

    public void MoveToNextRoomCamera()
    {
        previousPos = this.transform.position;
        StartCoroutine("CoMoveToNextRoomCamera");
    }

    private IEnumerator CoMoveToNextRoomCamera()
    {
        for (int i = 0; i < 13; i++)
        {
            nextPos = new Vector3(previousPos.x, previousPos.y - i, previousPos.z);
            this.transform.position = nextPos;
            yield return wait;
        }
    }
}
