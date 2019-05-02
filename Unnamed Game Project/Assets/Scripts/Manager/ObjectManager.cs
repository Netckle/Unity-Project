using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    public GameObject[] npcPrefabs;
    public GameObject[] objectPrefabs;

    public int[] spawnFadingRange = new int[2];

    [HideInInspector]
    public GameObject portal;

    public bool SearchPortal()
    {
        portal = FindObjectOfType<Portal>().gameObject;

        if (portal = null)
        {
            Debug.LogError("포탈을 찾을 수 없음.");
            return false;
        } 
        else
        {
            Debug.Log("포탈 검색 완료.");
            return true;
        }
    }
}
