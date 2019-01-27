using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
   [Range(0, 5)]
   public float TimeScale = 0.0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {        
            Time.timeScale = TimeScale;
            
        }

        else if (!Input.GetKey(KeyCode.K))
        {
            Time.timeScale = 1.0f;
        }
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
