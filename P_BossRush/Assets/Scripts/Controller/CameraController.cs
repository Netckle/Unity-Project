using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform OBJ01;
    public Transform OBJ02;

    private const float DISTANCE_MARGIN = 1.0f;

    private Vector3 middlePoint;
    private float distanceFromMiddlePoint;
    private float distanceBetweenOBJs;
    private float cameraDistance;
    private float aspectRatio;
    private float fov;
    private float tanFov;

    void Start()
    {
        aspectRatio = Screen.width / Screen.height;
        tanFov = Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f);
    }

    void Update()
    {
        Vector3 newCameraPos = Camera.main.transform.position;
        newCameraPos.x = middlePoint.x;
        Camera.main.transform.position = newCameraPos;

        Vector3 vectorBetweenPlayers = OBJ02.position - OBJ01.position;
        middlePoint = OBJ01.position + 0.5f * vectorBetweenPlayers;

        distanceBetweenOBJs = vectorBetweenPlayers.magnitude;
        cameraDistance = (distanceBetweenOBJs / 2.0f / aspectRatio) / tanFov;

        Vector3 dir = (Camera.main.transform.position - middlePoint).normalized;
        Camera.main.transform.position = middlePoint + dir * (cameraDistance + DISTANCE_MARGIN);        
    }

    void LateUpdate()
    {
        Camera.main.orthographicSize = cameraDistance;
    }
}
