using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGeneratorScript : MonoBehaviour
{
    public GameObject card;

    public GameObject GenerateCard(List<GameObject> existingCardGOs)
    {
        var bluePrintCards = GetBluePrints();

        existingCardGOs.ForEach(cardGO => bluePrintCards.Remove(new CardBluePrint(cardGO.GetComponent<Card>().color, cardGO.GetComponent<Card>().symbol)));

        return GetCardFromBluePrint(bluePrintCards[0]);
    }

    public List<List<GameObject>> GenerateDecks(int playerCount)
    {
        var bluePrintCards = GetBluePrints();

        int cardIndex = 0;
        var playerDecks = new List<List<GameObject>>();
        for(int i = 0; i < playerCount; i++)
        {
            var deck = new List<GameObject>();
            for(int j = 0; j < 7; j++)
            {
                deck.Add(GetCardFromBluePrint(bluePrintCards[cardIndex]));
                bluePrintCards.Remove(bluePrintCards[cardIndex]);
                cardIndex++;
            }
            playerDecks.Add(deck);
        }
        return playerDecks;
    }


    private List<CardBluePrint> Shuffle(List<CardBluePrint> cards)
    {
        var random = new System.Random();
        var newShuffledList = new List<CardBluePrint>();
        var cardsCount = cards.Count;
        for (int i = 0; i < cardsCount; i++)
        {
            var randomElementInList = random.Next(0, cards.Count);
            newShuffledList.Add(cards[randomElementInList]);
            cards.Remove(cards[randomElementInList]);
        }
        return newShuffledList;
    }

    private List<CardBluePrint> GetBluePrints()
    {
        var cards = new List<CardBluePrint>();

        string[] colors = { "blue", "green", "yellow", "red" };

        foreach (var color in colors)
        {
            //0
            cards.Add(new CardBluePrint(color, "0"));

            for (int j = 1; j < 10; j++)
            {
                //1-9 twice
                cards.Add(new CardBluePrint(color, j.ToString()));
                cards.Add(new CardBluePrint(color, j.ToString()));
            }

            //Special Cards (each twice): +2 (2 Aufnehmen); !! (Aussetzen); && (Richtungswechsel)
            cards.Add(new CardBluePrint(color, "+2"));
            cards.Add(new CardBluePrint(color, "+2"));
            cards.Add(new CardBluePrint(color, "!!"));
            cards.Add(new CardBluePrint(color, "!!"));
            cards.Add(new CardBluePrint(color, "<>"));
            cards.Add(new CardBluePrint(color, "<>"));
        }

        //Black Cards: +4 und Farbwahl (==)
        for (int i = 0; i < 4; i++)
        {
            cards.Add(new CardBluePrint("black", "+4"));
            cards.Add(new CardBluePrint("black", "=="));
        }
        return Shuffle(cards);
    }

    private GameObject GetCardFromBluePrint(CardBluePrint cardBluePrint)
    {
        var cardInstance = Instantiate(card);
        cardInstance.GetComponent<Card>().SetColorAndSymbol(cardBluePrint.color, cardBluePrint.symbol);
        cardInstance.tag = "card";
        return cardInstance;
    }
}




class CardBluePrint
{
    public string color;
    public string symbol;

    public CardBluePrint(string color, string symbol)
    {
        this.color = color;
        this.symbol = symbol;
    }
}
