using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript2 : MonoBehaviour
{
    BGMManager BGM;

    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(abc());
    }

    IEnumerator abc()
    {
        BGM.FadeOutMusic();

        yield return new WaitForSeconds(3f);

        BGM.FadeInMusic();
    }
}
