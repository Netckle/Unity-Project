using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    static public WeatherManager instance;

#region Singleton
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
#endregion Singleton

    private AudioManager theAudio;
    public ParticleSystem rain;
    public string rain_sound;

    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    void Update()
    {

    }

    public void Rain() 
    {
        theAudio.Play(rain_sound);
        rain.Play(); 
    }

    public void RainStop() 
    {
        theAudio.Stop(rain_sound);
        rain.Stop(); 
    }

    public void RainDrop()
    {
        rain.Emit(10);
    }
}
