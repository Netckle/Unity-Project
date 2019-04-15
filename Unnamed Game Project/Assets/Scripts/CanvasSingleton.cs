using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
    // Singleton
    static CanvasSingleton instance = null; 

    public static CanvasSingleton Instace()
    {
        return instance;
    } 

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    //-----
}
