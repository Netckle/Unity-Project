using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private BoxCollider2D bound;

    private CameraManager the_camera; 

    void Start()
    {
        bound = GetComponent<BoxCollider2D>();
        the_camera = FindObjectOfType<CameraManager>();
        the_camera.SetBound(bound);
    }
}
