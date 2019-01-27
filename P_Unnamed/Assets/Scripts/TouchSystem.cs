using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSystem : MonoBehaviour
{
    private Vector3 mousePos;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("마우스 좌표 : " + Input.mousePosition);
            mousePos = Input.mousePosition;
        }
    }
}
