using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public int hp;
    public int current_hp;
    public int attack;
    public int defense;
    public int exp;

    void Start()
    {
        current_hp = hp;
    }

    public int DamagedByPlayer(int _player_attack)
    {
        int player_attack = _player_attack;
        int damage;

        if (defense >= player_attack)
            damage = 1;
        else
            damage = player_attack - defense;

        current_hp -= damage;

        if (current_hp <= 0)
        {
            Destroy(this.gameObject);
            PlayerStat.instance.current_exp += exp;
        }

        return damage;
    }
}
