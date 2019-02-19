using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRain : MonoBehaviour
{
    private WeatherManager theWeather;
    public bool rain;

    void Start()
    {
        theWeather = FindObjectOfType<WeatherManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (rain)
            theWeather.RainDrop();
        else
            theWeather.RainStop();
    }
}
