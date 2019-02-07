using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour
{
    public string TransferMapName;
    private MovingObject thePlayer;

    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            thePlayer.currentMapName = TransferMapName;
            SceneManager.LoadScene(TransferMapName);
        }
    }
}