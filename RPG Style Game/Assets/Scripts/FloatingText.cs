using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float move_speed;
    public float destroy_time;

    public Text text; // text prefabs.

    private Vector3 vector;

    void Update()
    {
        vector.Set(text.transform.position.x, text.transform.position.y + (move_speed * Time.deltaTime), text.transform.position.z);
        text.transform.position = vector;

        destroy_time -= Time.deltaTime;

        if (destroy_time <= 0)
            Destroy(this.gameObject);
    }
}
