using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private PlayerManager   the_player;  // 플레이어가 바라보고 있는 방향.
    private Vector2         vector;

    private Quaternion      rotation;   // 회전(각도)을 담당하는 Vector4 x y z w

	void Start () 
    {
        the_player = FindObjectOfType<PlayerManager>();
	}
	
	void Update () 
    {
        this.transform.position = the_player.transform.position;

        vector.Set(the_player.animator.GetFloat("DirX"), the_player.animator.GetFloat("DirY"));

        if(vector.x == 1.0F)
        {
            rotation = Quaternion.Euler(0, 0, 90);
            this.transform.rotation = rotation;
        }
        else if (vector.x == -1.0F)
        {
            rotation = Quaternion.Euler(0, 0, -90);
            this.transform.rotation = rotation;
        }
        else if (vector.y == 1.0F)
        {
            rotation = Quaternion.Euler(0, 0, 180);
            this.transform.rotation = rotation;
        }
        else if (vector.y == -1.0F)
        {
            rotation = Quaternion.Euler(0, 0, 0);
            this.transform.rotation = rotation;
        }
    }
}
