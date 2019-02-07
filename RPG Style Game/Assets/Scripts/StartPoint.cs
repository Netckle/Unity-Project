using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint;

    private MovingObject thePlayer;
    private CameraManager theCamera;

    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<MovingObject>();

        if (startPoint == thePlayer.currentMapName)
        {
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.transform.position.y, -10);
            thePlayer.transform.position = this.transform.position;
        }
    } 
}
