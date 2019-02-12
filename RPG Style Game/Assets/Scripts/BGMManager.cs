using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 주로 배경음을 관리하는 클래스
public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    public AudioClip[] clips; // 배경음악 파일
    
    private AudioSource source;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

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

    public void Play(int _playMusicTrack)
    {
        source.volume   = 1f;
        source.clip     = clips[_playMusicTrack];
        source.Play();
    }

    public void Stop() { source.Stop(); }    

    public void Pause() { source.Pause(); }

    public void UnPause() { source.UnPause(); }

    public void SetVolume(float _volume) { source.volume = _volume; }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i = 1.0f; i >= 0f; i -= 0.01f)
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
        for (float i = 0f; i <= 1.0f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
