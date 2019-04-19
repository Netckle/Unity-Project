using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
    static CanvasSingleton instance = null; 

    public static CanvasSingleton Instace()
    {
        return instance;
    } 

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
