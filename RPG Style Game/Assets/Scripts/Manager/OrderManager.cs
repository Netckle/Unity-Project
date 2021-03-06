﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private PlayerManager the_player;        // 이벤트 도중에 키입력 처리를 담당한다.
    private List<MovingObject> characters;  // 크기의 변동이 있으므로 배열은 힘들다.

    void Start()
    {
        the_player = FindObjectOfType<PlayerManager>();
    }

    // 씬에 있는 MovingObject의 정보들을 받아오기 위해 사용한다. 명령을 내리기 전에 실행해야 한다. 
    public void PreLoadCharacter()
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

    public void canMove()
    {
        the_player.do_not_move = false;
    }

    public void NotMove()
    {
        the_player.do_not_move = true;
    }

    public void SetPassingStatus(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].character_name)
            {
                characters[i].box_collider.isTrigger = false;
            }
        }
    }

    public void Move(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].character_name)
            {
                characters[i].Move(_dir);
            }
        }
    }

    public void SetTransparent(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].character_name)
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetUnTransparent(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].character_name)
            {
                characters[i].gameObject.SetActive(true);
            }
        }
    }

    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].character_name)
            {
                characters[i].animator.SetFloat("DirX", 0f);
                characters[i].animator.SetFloat("DirY", 0f);

                switch(_dir)
                {
                    case "UP":
                        characters[i].animator.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].animator.SetFloat("DirY", -1f);
                        break;
                    case "LEFT":
                        characters[i].animator.SetFloat("DirX", -1f);
                        break;
                    case "RIGHT":
                        characters[i].animator.SetFloat("DirX", 1f);
                        break;
                }
            }
        }
    }
}
