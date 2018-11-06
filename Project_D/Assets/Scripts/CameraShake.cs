using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    Vector3 originPos;

    public float _amount, _duration;

    void Start()
    {
        originPos = transform.localPosition;
    }

    public IEnumerator Shake()
    {
        float timer = 0;
        while (timer <= _duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originPos;

    }
}
