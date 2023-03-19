using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesScript : MonoBehaviour
{
    private GameManagerScript gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManagerScript>();
    }

    public bool Evaluate(GameObject cardGO, GameObject lastPlacedCardGO)
    {
        var lastPlacedCard = lastPlacedCardGO.GetComponent<Card>();
        var card = cardGO.GetComponent<Card>();
        if (card.color == "black") return lastPlacedCard.color == "black" ? false : true;
        else if ((card.symbol == lastPlacedCard.symbol || card.color == lastPlacedCard.color) &&
                 !gameManager.penaltyCardsLaying) return true;
        else if (card.symbol == "+2" && lastPlacedCard.symbol == "+2") return true;
        else return false;
    }
}