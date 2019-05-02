using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    private bool can_spawn = false;

    void Update()
    {
        if (can_spawn && Input.GetButtonDown("Interact"))
        {
            GameManager.Instance().objectM.portal.SetActive(true);
            GameManager.Instance().objectM.portal.transform.position = GameManager.Instance().stageM.generatedStages[Player.Instace().currentRoomNum].transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            can_spawn = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            can_spawn = false;
    }
}
