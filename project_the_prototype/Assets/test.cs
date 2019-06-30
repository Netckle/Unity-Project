using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class test : MonoBehaviour
{
    public TextMeshProUGUI text;

    public GameObject player;

    void Update()
    {
        float x = Mathf.Round(player.transform.position.x * 10) * 0.1f;
        float y = Mathf.Round(player.transform.position.y * 10) * 0.1f;

        text.text = "플레이어 좌표 : X (" + x + ") Y (" + y + ")";
    }
}
