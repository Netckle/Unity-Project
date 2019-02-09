using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    BGMManager BGM;

    public int playMusicTrack;

    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BGM.Play(playMusicTrack);
        this.gameObject.SetActive(false);
    }
}
