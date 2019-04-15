using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHangState : MonoBehaviour
{
    public GameObject player;
    private Player playerScript;

    void Start()
    {
        playerScript = player.GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            playerScript.Hanging();
        }
    }
}
