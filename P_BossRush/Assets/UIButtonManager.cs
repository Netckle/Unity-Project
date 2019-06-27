using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonManager : MonoBehaviour
{
    GameObject player;
    PlayerMovement playerScript;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    public void LeftDown()
    {
        playerScript.inputLeft = true;
    }

    public void LeftUp()
    {
        playerScript.inputLeft = false;
    }

    public void RightDown()
    {
        playerScript.inputRight = true;
    }

    public void RightUp()
    {
        playerScript.inputRight = false;
    }

    public void DashDown()
    {
        playerScript.inputDash = true;
    }

    public void DashUp()
    {
        playerScript.inputDash = false;
    }

    public void JumpDown()
    {
        playerScript.inputJump = true;
    }

    public void JumpUp()
    {
        playerScript.inputJump = false;
    }
}
