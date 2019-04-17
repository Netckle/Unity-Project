using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextRoom : MonoBehaviour
{
    private Vector3 previousPos;
    private Vector3 nextPos;
    public float delayTime;

    public void MoveNext()
    {
        previousPos = Camera.main.transform.position;
        StartCoroutine("MoveNextCoroutine");
    }

    IEnumerator MoveNextCoroutine()
    {
        for (int i = 0; i < 13; i++)
        {
            nextPos = new Vector3(previousPos.x, previousPos.y - i, previousPos.z);

            Camera.main.transform.position = nextPos;

            yield return new WaitForSeconds(delayTime);
        }
    }
}
