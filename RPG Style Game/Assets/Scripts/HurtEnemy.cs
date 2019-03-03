using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    public string attack_sound;

    private PlayerStat the_player_state;
    private FloatingTextTrigger floating_text_trigger;

    void Start()
    {
        the_player_state = FindObjectOfType<PlayerStat>();
        floating_text_trigger = FindObjectOfType<FloatingTextTrigger>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            int damage = collision.gameObject.GetComponent<EnemyStat>().DamagedByPlayer(the_player_state.attack);
            AudioManager.instance.Play(attack_sound);

            floating_text_trigger.TriggerFloatingText(collision.gameObject.transform.position, 100, damage.ToString(), Color.white, 25);
        }
    }
}
