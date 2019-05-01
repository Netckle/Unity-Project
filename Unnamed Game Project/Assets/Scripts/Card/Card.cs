using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        data = new CardData(cardName, cardType, cardLevel);
    }
}
