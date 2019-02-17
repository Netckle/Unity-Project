using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour
{
    public string targetMapName;
    private PlayerManager thePlayer;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            thePlayer.currentMapName = targetMapName;
            SceneManager.LoadScene(targetMapName);
        }
    }
}
