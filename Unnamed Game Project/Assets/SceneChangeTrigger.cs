using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    private ChangeScene sceneChange;

    void Start()
    {
        sceneChange = GameObject.Find("Scene Change Manager").GetComponent<ChangeScene>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("상호작용"))
        {
            sceneChange.CardSystemSwitch(true);
        }
    }
}
