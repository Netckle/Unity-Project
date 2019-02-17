using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager    instance;

    public AudioClip[]          clips;
    private AudioSource         source;

    private WaitForSeconds      waitTime = new WaitForSeconds(0.01F);

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
        source = GetComponent<AudioSource>();
    }

    public void Play(int _index)
    {
        source.volume   = 1.0F; // NORMAL
        source.clip     = clips[_index];
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }

    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i = 1.0F; i >= 0F; i -= 0.01F)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0F; i <= 1.0F; i += 0.01F)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
