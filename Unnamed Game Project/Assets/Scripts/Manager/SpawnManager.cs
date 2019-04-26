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

    static SpawnManager instance = null;

    public static SpawnManager Instance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;

        portal = FindObjectOfType<Portal>().gameObject;
        portal.SetActive(false);
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
