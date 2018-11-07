using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp3 : MonoBehaviour 
{
	private Temp2 player_inform;
	private bool isAttacked = false;
	private bool isGrabbed 	= false;

	public CameraShake cameraShake;

	void Start()
	{
		player_inform = GetComponentInParent<Temp2>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			Debug.Log("isAttacked : " + isAttacked);
			isAttacked = true;
		}
		else if (Input.GetKeyUp(KeyCode.O))
		{
			isAttacked = false;
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			isGrabbed = true;
		}
		else if (Input.GetKeyUp(KeyCode.P))
		{
			isGrabbed = false;
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (!isAttacked && !isGrabbed)
			return;

		if (isAttacked && collider.gameObject.tag == "Box")
		{
			Debug.Log("Box와 충돌함");

			Temp boxData = collider.gameObject.GetComponent<Temp>();			
			
			if (boxData.box_dir != player_inform.playerDir)
			{
				cameraShake.StartCoroutine("Shake");
				boxData.OnHit(TOUNCHED_OBJ.PLAYER, player_inform.playerDir);
			}
		}
	}
}
