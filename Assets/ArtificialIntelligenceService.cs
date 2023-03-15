using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligenceService : MonoBehaviour
{
    private RulesScript rules;
    private TakeStackScript takeStack;

    void Start()
    {
        rules = GameObject.FindGameObjectWithTag("rules").GetComponent<RulesScript>();
        takeStack = GameObject.FindGameObjectWithTag("takeStack").GetComponent<TakeStackScript>();
    }

    public void ExecuteMove(GameObject player, GameObject stackCard)
    {
        var card = determineMove(player, stackCard);

        if (card == null) takeStack.GetTakeCard().GetComponent<Card>().AiCallingToUseCard();
        else card.GetComponent<Card>().AiCallingToUseCard();
    }

    public GameObject determineMove(GameObject player, GameObject stackCard)
    {
        if (!player.CompareTag("bot")) return null;
        var hand = player.GetComponent<PlayerScript>().GetDeck();
        var allowedCards = new List<GameObject>();

        hand.ForEach(cardGO =>
        {
            if (rules.Evaluate(cardGO, stackCard)) allowedCards.Add(cardGO);
        });
        if (allowedCards.Count == 0) return null;
        return GetBestChoices(allowedCards)[0];
    }

    private List<GameObject> GetBestChoices(List<GameObject> allowedCards)
    {
        var sameColorCards = new List<GameObject>();
        string[] colors = { "blue", "green", "yellow", "red" };

        int storageNumber = 0;
        string determinedColor = "black";
        foreach (var color in colors)
        {
            int number = 0;
            foreach (var card in allowedCards)
            {
                if (card.GetComponent<Card>().color == color) number++;
            }
            if (storageNumber < number)
            {
                storageNumber = number;
                determinedColor = color;
            }
        }

        allowedCards.ForEach(card =>
        {
            if (card.GetComponent<Card>().color == determinedColor) sameColorCards.Add(card);
        });

        return sameColorCards;
    }

    public string determineColor(List<GameObject> cards)
    {
        string[] colors = { "blue", "green", "yellow", "red" };
        var storageNumber = 0;
        var determinedColor = "";
        foreach (var color in colors)
        {
            var number = 0;
            foreach (var card in cards)
            {
                if (card.GetComponent<Card>().color == color) number++;
            }

            if (storageNumber < number)
            {
                storageNumber = number;
                determinedColor = color;
            }
        }
        if (determinedColor == "") return colors[Random.Range(1, 4)];

        return determinedColor;
    }
}