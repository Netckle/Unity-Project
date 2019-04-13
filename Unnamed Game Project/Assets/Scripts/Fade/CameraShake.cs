using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originPos;
 
    void Start () 
    {
        originPos = transform.position;
    }

    public void StartShake(float amount, float duration)
    {
        StartCoroutine(Shake(amount, duration));
    }

    IEnumerator Shake(float amount, float duration)
    {
        float timer = 0;

        while(timer <= duration)
        {
            transform.position = (Vector3)Random.insideUnitCircle * amount + originPos;
    
            timer += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originPos;    
    }    
}
