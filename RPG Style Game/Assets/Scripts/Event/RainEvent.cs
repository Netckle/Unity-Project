using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEvent : MonoBehaviour
{
    private WeatherManager the_weather;
    public bool is_ranning;

    void Start()
    {
        the_weather = FindObjectOfType<WeatherManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (is_ranning)
        {
            the_weather.RainDrop(); 
        }
        else
        {
            the_weather.RainStop();
        }
    }
}
