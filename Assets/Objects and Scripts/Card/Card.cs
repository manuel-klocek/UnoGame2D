using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string color = "";
    public string symbol = "";
    private CardStackScript cardStack;
    private TakeStackScript takeStack;
    private CardManagerScript cardManager;
    private RulesScript rules;
    private GameManagerScript gameManager;

    void Start()
    {
        cardStack = GameObject.FindGameObjectWithTag("cardStack").GetComponent<CardStackScript>();
        takeStack = GameObject.FindGameObjectWithTag("takeStack").GetComponent<TakeStackScript>();
        cardManager = GameObject.FindGameObjectWithTag("cardManager").GetComponent<CardManagerScript>();
        rules = GameObject.FindGameObjectWithTag("rules").GetComponent<RulesScript>();
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManagerScript>();
    }

    public void SetColorAndSymbol(string color, string symbol)
    {
        this.color = color;
        this.symbol = symbol;
    }

    private void OnMouseUp()
    {
        if (cardStack.GetCardStack().Contains(gameObject)) return;
        else if (takeStack.GetTakeCard() == gameObject)
        {
            if (gameManager.penaltyCardsLaying) ExecutePenalty();
            else
            {
                if (rules.Evaluate(gameObject, takeStack.GetTakeCard())) { /*TODO create animation for take and play or hold*/ gameManager.currentPlayer.GetComponent<PlayerScript>().AddCardGO(takeStack.UseTakeCard()); }
                else gameManager.currentPlayer.GetComponent<PlayerScript>().AddCardGO(takeStack.UseTakeCard());
            }
        }
        else if (!rules.Evaluate(gameObject, cardStack.GetLastCard())) { /*TODO implement card shake animation*/ return; }
        else
        {
            gameObject.tag = "stackCard";
            cardStack.AddCardGO(gameObject);
            //Event Handler calls NextPlayer on Action Cards
            if (CheckForEventCard())
            {
                gameManager.CheckForEvents();
                return;
            }
        }

        //TODO remove old cards from player deck
        gameManager.NextPlayer();
    }

    private void OnMouseOver()
    {
        if (takeStack.GetTakeCard() != gameObject) cardManager.SetCardDescription(color, symbol);
    }

    private void ExecutePenalty()
    {
        int penaltySum = 0;
        foreach (var cardGO in cardStack.GetCardStack())
        {
            var card = cardGO.GetComponent<Card>();
            if (card.symbol == "+2") penaltySum += 2;
            else if (card.symbol == "+4") penaltySum += 4;
        }

        gameManager.AddPenaltyCards(penaltySum);
    }

    private bool CheckForEventCard()
    {
        var eventSymbolCollection = new List<string> { "!!", "<>", "+2", "+4", "==" };
        if (!eventSymbolCollection.Contains(symbol)) return false;
        gameManager.isEvent = true;
        return true;
    }
}