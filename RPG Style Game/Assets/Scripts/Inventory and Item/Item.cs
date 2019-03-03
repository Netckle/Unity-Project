using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public int item_id; 
    public string item_name; 
    public string item_description; 
    public int item_count; 
    public Sprite item_icon; 
    public ItemType item_type;

    public enum ItemType
    {
        USE, EQUIP, QUEST, ETC
    }

    public Item(int _item_id, string _item_name, string _item_description, ItemType _item_type, int _item_count = 1)
    {
        item_id = _item_id;
        item_name = _item_name;
        item_description = _item_description;
        item_type = _item_type;
        item_count = _item_count;
        item_icon = Resources.Load("ItemIcon/" + _item_id.ToString(), typeof(Sprite)) as Sprite;
    }
}
