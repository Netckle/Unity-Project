using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    public GameObject[] npcPrefabs;
    public GameObject[] objectPrefabs;

    public int[] spawnFadingRange = new int[2];

    [HideInInspector]
    public GameObject portal;

    void Awake()
    {

    }

    public void UpdatePortalState()
    {
        portal = FindObjectOfType<Portal>().gameObject;
        if (portal != null)
            Debug.Log("포탈 캐치함");
        portal.SetActive(false);
    }
}
