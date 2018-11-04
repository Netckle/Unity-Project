using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour {

    public GameObject box;
    public bool canSpawn = true;

    void Start()
    {

    }

    void Update()
    {
        if (canSpawn)
            StartCoroutine(SpawnBox());
    }

    IEnumerator SpawnBox()
    {
        canSpawn = false;

        GameObject BoxObj = Instantiate(box, transform.position, Quaternion.identity) as GameObject;

        yield return new WaitForSeconds(0.5F);

        canSpawn = true;
    }
}
