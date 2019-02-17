using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private PlayerManager       thePlayer;
    private List<MovingObject>  characters;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    public void PreLoadCharacterInform()
    {
        characters = ToList();
    }

    public List<MovingObject> ToList()
    {
        List<MovingObject> tempList = new List<MovingObject>();
        MovingObject[] temp = FindObjectsOfType<MovingObject>();

        for (int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }
        return tempList;
    }

    public void SetCanMove(bool _canMove)
    {
        thePlayer.canMove = _canMove;
    }

    public void Move(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].Move(_dir);
            }
        }
    }

    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].animator.SetFloat("DirX", 0F);
                characters[i].animator.SetFloat("DirY", 0F);

                switch (_dir)
                {
                    case "UP":
                        characters[i].animator.SetFloat("DirY", 1.0F);
                        break;
                    case "DOWN":
                        characters[i].animator.SetFloat("DirY", -1.0F);
                        break;
                    case "LEFT":
                        characters[i].animator.SetFloat("DirX", -1.0F);
                        break;
                    case "RIGHT":
                        characters[i].animator.SetFloat("DirX", 1.0F);
                        break;
                }
            }
        }
    }

    public void SetPassingStatus(string _name, bool _isTrigger)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].boxCollider.isTrigger = _isTrigger;
            }
        }
    }

    public void SetActiveMode(string _name, bool _active)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(_active);
            }
        }
    }
}
