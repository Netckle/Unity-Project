using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private ChangeScene change;

    void Start()
    {
        change = GameObject.Find("Change Scene Manager").GetComponent<ChangeScene>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("일단 충돌함");

        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            Debug.Log("들어왔다.");
            change.CardSystemSwitch(true);
        }
    }
}
