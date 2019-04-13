using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextRoom : MonoBehaviour
{
    private Vector3 tempPos;

    public void MoveNext()
    {
        StartCoroutine("MoveNextCoroutine");
    }

    IEnumerator MoveNextCoroutine()
    {
        for (int i = 0; i < 12; i++)
        {
            tempPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + i, Camera.main.transform.position.z);

            Camera.main.transform.position = tempPos;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
