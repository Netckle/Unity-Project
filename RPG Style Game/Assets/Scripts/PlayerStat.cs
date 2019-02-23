using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int characterLevel;
    public int[] needExp;
    public int currentExp;

    public int hp;
    public int currentHp;
    public int mp;
    public int currentMp;

    public int atk;
    public int def;

    public string dmgSound;

    public GameObject prefabs_Floating_text;
    public GameObject parent; // Canvas

    void Start()
    {
        instance = this;
    }

    public void Hit(int _enemyAtk)
    {
        int dmg;

        if (def >= _enemyAtk)
            dmg = 1;
        else
            dmg = _enemyAtk - def;

        currentHp -= dmg;

        if (currentHp <= 0)
            Debug.Log("체력 0 미만, 게임오버");

        AudioManager.instance.Play(dmgSound);

        Vector3 vector = this.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = dmg.ToString();
        clone.GetComponent<FloatingText>().text.color = Color.red;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);

        StopAllCoroutines();
        StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    void Update()
    {
        if (currentExp >= needExp[characterLevel])
        {
            characterLevel++;
            hp += characterLevel * 2;
            mp += characterLevel + 2;

            currentHp = hp;
            currentMp = mp;
            atk++;
            def++;
        }
    }
}
