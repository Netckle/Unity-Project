using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputField : MonoBehaviour
{
    private PlayerManager the_player;
    public Text text;

    void Start()
    {
        the_player = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            the_player.character_name = text.text;
            Destroy(this.gameObject);
        }
    }
}
