using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[System.Serializable]
public class CardData
{
    public string   cardName;
    public string   cardType;
    public int      cardLevel;

    public CardData(string _cardName, string _cardType, int _cardLevel)
    {
        cardName = _cardName;
        cardType = _cardType;
        cardLevel = _cardLevel;
    }
}

public struct CardDataContainer
{
    public List<CardData> cardData;

    public CardDataContainer(List<CardData> _cardData)
    {
        cardData = _cardData;
    }
}

public class Card : MonoBehaviour
{
    [HideInInspector]
    public CardData data;

    public string   cardName;
    public string   cardType;
    public int      cardLevel;

    public Text name;
    public Text level;

    void Start()
    {
        data = new CardData(cardName, cardType, cardLevel);

        name.text = cardName;
        level.text = "레벨 " + cardLevel.ToString();
    }
}
