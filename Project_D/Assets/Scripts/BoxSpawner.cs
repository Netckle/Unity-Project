using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour {

    public GameObject box;
    public bool isSpawned = true;

    void Start()
    {

    }

    void Update()
    {
        if (isSpawned)
        {
            isSpawned = false;
            Instantiate(box, transform);
        }
    }

}
