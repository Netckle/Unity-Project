using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int item_id;
    public int count;
    public string pick_up_sound;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.Play(pick_up_sound);
            Inventory.instance.GetAnItem(item_id, count);
            Destroy(this.gameObject);
        }
    }
}
