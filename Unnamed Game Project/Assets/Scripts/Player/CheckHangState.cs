using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHangState : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerScript;

    void Start()
    {
        playerScript = player.GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            playerScript.Hanging();
        }
    }
}
