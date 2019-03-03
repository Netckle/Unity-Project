using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text item_name_text;
    public Text item_count_text;
    public GameObject selected_item;
    
    public void AddItem(Item _item)
    {
        item_name_text.text = _item.item_name;
        icon.sprite = _item.item_icon;

        if (Item.ItemType.USE == _item.item_type)
        {
            if (_item.item_count > 0)
                item_count_text.text = "x " + _item.item_count.ToString();
            else
                item_count_text.text = "";
        }
    }

    public void RemoveItem()
    {
        item_name_text.text = "";
        item_count_text.text = "";
        icon.sprite = null;
    }
}
