using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour
{
    public string transfer_map_name;
    private PlayerManager the_player;

    void Start()
    {
        the_player = FindObjectOfType<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            the_player.current_map_name = transfer_map_name;
            SceneManager.LoadScene(transfer_map_name);
        }
    }
}