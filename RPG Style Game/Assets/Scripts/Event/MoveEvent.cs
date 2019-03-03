using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestMove
{
    public string name;
    public string direction;
}

public class MoveEvent : MonoBehaviour
{
    [SerializeField]
    public TestMove[] move;
    public string direction;

    private OrderManager the_order;

    void Start()
    {
        the_order = FindObjectOfType<OrderManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "플레이어")
        {
            the_order.PreLoadCharacter();

            for (int i = 0; i < move.Length; i++)
            {
               the_order.Move(move[i].name, move[i].direction);
            }

            the_order.Turn("NPC 1번", direction);
        }
    }
}
