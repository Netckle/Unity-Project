using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextRoom : MonoBehaviour
{
    private Vector3 nextPos;
    public float delayTime;

    public void MoveNext()
    {
        StartCoroutine("MoveNextCoroutine");
    }

    IEnumerator MoveNextCoroutine()
    {
        for (int i = 0; i < 12; i++)
        {
            nextPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + i, Camera.main.transform.position.z);

            Camera.main.transform.position = nextPos;

            yield return new WaitForSeconds(delayTime);
        }
    }
}
