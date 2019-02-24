using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_level;
    public int[] need_exp;
    public int current_exp;

    public int hp;
    public int current_hp;
    public int mp;
    public int current_mp;

    public int attack;
    public int defense;

    public string damaged_sound;

    private FloatingTextTrigger floating_text_trigger;

    void Start()
    {
        instance = this;
        floating_text_trigger = FindObjectOfType<FloatingTextTrigger>();
    }

    public void DamagedByEnemy(int _enemy_attack)
    {
        int damage;

        if (defense >= _enemy_attack)
            damage = 1;
        else
            damage = _enemy_attack - defense;

        current_hp -= damage;

        if (current_hp <= 0)
            Debug.Log("체력이 0 미만입니다. 게임오버입니다.");

        AudioManager.instance.Play(damaged_sound);

        floating_text_trigger.TriggerFloatingText(this.transform.position, 60, damage.ToString(), Color.red, 25);
        
        StopAllCoroutines();
        StartCoroutine(HitCoroutine(3));
    }

    IEnumerator HitCoroutine(int repeat_count)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0f;

        for (int i = 0; i < repeat_count; i++)
        {
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.1f);

            switch (color.a)
            {
                case 0f:
                    color.a = 1f;
                    break;
                case 1f:
                    color.a = 0f;
                    break;
            }
        }

        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    void Update()
    {
        if (current_exp >= need_exp[character_level])
        {
            character_level++;
            hp += character_level * 2;
            mp += character_level + 2;

            current_hp = hp;
            current_mp = mp;
            attack++;
            defense++;
        }
    }
}
