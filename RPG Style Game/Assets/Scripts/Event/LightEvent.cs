using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEvent : MonoBehaviour
{
    public GameObject go;

    private bool flag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag)
        {
            flag = true;
            go.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        flag = false;
        go.SetActive(false);
    }
}
