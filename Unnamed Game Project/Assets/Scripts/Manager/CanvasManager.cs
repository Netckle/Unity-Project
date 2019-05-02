using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HUDTYPE
{
    CARD, QUEST, INVENTORY, STATUS
}

public class CanvasManager : MonoBehaviour
{
    public GameObject[] huds = new GameObject[4];

    public void ControllPanel(HUDTYPE type, bool isOpen)
    {
        int index = 0;

        switch (type)
        {
            case HUDTYPE.CARD:
                index = 0; 
                break;
            case HUDTYPE.QUEST:
                index = 1;
                break;
            case HUDTYPE.INVENTORY:
                index = 2;
                break;
            case HUDTYPE.STATUS:
                index = 3;
                break;
        }
        huds[index].SetActive(isOpen);
    }

    public void ControllAllPanel(bool isOpen)
    {
        foreach (GameObject hud in huds)
        {
            hud.SetActive(isOpen);
        }
    }
}
