﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string       name;   // 사운드 이름
    public AudioClip    clip;   // 사운드 파일
    private AudioSource source; // 사운드 플레이어

    public float        volume;
    public bool         loop;

    public void SetSource(AudioSource _source)
    {
        source          = _source;
        source.clip     = clip;
        source.loop     = loop;
        source.volume   = volume;
    }

    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }

    public void SetLoop(bool _loop)
    {
        source.loop = _loop;
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;

    [SerializeField]
    public Sound[] sounds;

#region Singleton
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
#endregion Singleton

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name, bool _loop)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop(_loop);
                return;
            }
        }
    }

    public void SetVolume(string _name, float _volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetVolume(_volume);
            }
        }
    }
}
