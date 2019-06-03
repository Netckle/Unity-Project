using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMiniSlime : MonoBehaviour
{   
    public GameObject prefab;
    public float power;
    public float yOffset;

    public List<GameObject> miniSlimes = new List<GameObject>();

    private BossSlime mini;

    void Start()
    {
        mini = GetComponent<BossSlime>();

        // Make mini slimes.
        for (int i = 0; i < 3; ++i)
        {
            GameObject slime = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            slime.SetActive(false);
            miniSlimes.Add(slime);
        }
    }

    public void SpawnMiniMonster(int max)
    {
        StartCoroutine(CorSpawnMiniMonster(max));
    }

    IEnumerator CorSpawnMiniMonster(int max)
    {
        for (int i = 0; i < max; ++i)
        {          
            miniSlimes[i].SetActive(true);
            miniSlimes[i].transform.position = this.transform.position + new Vector3(0, yOffset, 0);
            miniSlimes[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.normalized.x, transform.localScale.normalized.y) * power, ForceMode2D.Impulse);
            
            yield return new WaitForSeconds(1.0F);
        }
    }

    void Update()
    {
        for(int i = 0; i < miniSlimes.Count; ++i) 
        {
            if (miniSlimes[i].activeSelf)
            {
                mini.canAttack = false;
                return;
            }
        }
        mini.canAttack = true;
    }
}
