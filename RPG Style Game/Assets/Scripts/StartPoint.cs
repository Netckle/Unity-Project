using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string start_point;

    private PlayerManager the_player;
    private CameraManager the_camera;

    void Start()
    {
        the_camera = FindObjectOfType<CameraManager>();
        the_player = FindObjectOfType<PlayerManager>();

        if (start_point == the_player.current_map_name)
        {
            the_camera.transform.position = new Vector3(this.transform.position.x, this.transform.transform.position.y, -10);
            the_player.transform.position = this.transform.position;
        }
    } 
}
