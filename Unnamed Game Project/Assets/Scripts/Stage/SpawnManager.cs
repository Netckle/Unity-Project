﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    public GameObject[] npcPrefabs;

    public int[] spawnFadingRange = new int[2];

    [HideInInspector]
    public GameObject portal;

    static SpawnManager instance = null;

    public static SpawnManager Instance()
    {
        return instance;
    }

    //-----[Prefab Setting Start]

    void Awake()
    {
        instance = this;

        portal = FindObjectOfType<Portal>().gameObject;
        portal.SetActive(false);
    }

    //-----[Prefab Setting End]
}
