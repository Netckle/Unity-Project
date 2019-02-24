using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    static public DataBaseManager instance;

    private PlayerStat the_player_stat;

    private FloatingTextTrigger floating_text_trigger;

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches; 

    public List<Item> itemList = new List<Item>();

#region Singleton
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
#endregion Singleton
    
    void Start()
    {
        the_player_stat = FindObjectOfType<PlayerStat>();
        floating_text_trigger = FindObjectOfType<FloatingTextTrigger>();

        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 체워주는 마법의 물약", Item.ItemType.USE));
    }
    
    public void UseItem(int _item_id)
    {
        switch(_item_id)
        {
            case 10001:
                if (the_player_stat.hp >= the_player_stat.current_hp + 50)
                    the_player_stat.current_hp += 50;
                else
                    the_player_stat.current_hp += the_player_stat.hp;

                floating_text_trigger.TriggerFloatingText(the_player_stat.transform.position, 60, 50.ToString(), Color.green, 25);
                break;
            case 10002:
                if (the_player_stat.mp >= the_player_stat.current_mp + 50)
                    the_player_stat.current_mp += 15;
                else
                    the_player_stat.current_mp += the_player_stat.mp;

                floating_text_trigger.TriggerFloatingText(the_player_stat.transform.position, 60, 50.ToString(), Color.blue, 25);
                break;
        }
    }
}
