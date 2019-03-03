using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    static public WeatherManager instance;

    private AudioManager the_audio;
    public ParticleSystem rain_particle;
    public string rain_sound;

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

    void Start()
    {
        the_audio = FindObjectOfType<AudioManager>();
    }

    public void RainStart() 
    {
        the_audio.Play(rain_sound);
        rain_particle.Play(); 
    }

    public void RainStop() 
    {
        the_audio.Stop(rain_sound);
        rain_particle.Stop(); 
    }

    public void RainDrop()
    {
        rain_particle.Emit(10);
    }
}
