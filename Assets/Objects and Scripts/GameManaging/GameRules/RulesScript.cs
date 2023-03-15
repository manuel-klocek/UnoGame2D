using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesScript : MonoBehaviour
{
    public bool Evaluate(GameObject cardGO, GameObject lastPlacedCardGO)
    {
        var lastPlacedCard = lastPlacedCardGO.GetComponent<Card>();
        var card = cardGO.GetComponent<Card>();
        if (card.symbol == "+2" && lastPlacedCard.symbol == "+2" ||
            card.color == "black") return true;
        else if (card.symbol == lastPlacedCard.symbol ||
                 card.color == lastPlacedCard.color) return true;
        else return false;
    }
}