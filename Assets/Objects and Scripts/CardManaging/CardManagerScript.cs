using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManagerScript : MonoBehaviour
{
    public GameObject card;
    public Text cardDescripion;
    private CardStackScript cardStack;
    private TakeStackScript takeStack;
    public CardGeneratorScript cardGenerator;
    private List<GameObject> existingCardGOs;
    private readonly List<PlayerScript> players = new();

    // Start is called before the first frame update
    void Start()
    {
        cardGenerator = GameObject.FindGameObjectWithTag("cardGenerator").GetComponent<CardGeneratorScript>();
        cardStack = GameObject.FindGameObjectWithTag("cardStack").GetComponent<CardStackScript>();
        takeStack = GameObject.FindGameObjectWithTag("takeStack").GetComponent<TakeStackScript>();
        UpdateExistingCards();
    }

    void Update()
    {
        UpdateExistingCards();
    }

    public void GenerateDecks(List<GameObject> players)
    {
        this.players.Clear();
        players.ForEach(player => this.players.Add(player.GetComponent<PlayerScript>()));

        var decks = cardGenerator.GenerateDecks(players.Count);


        for(int i = 0; i < decks.Count; i++)
        {
            players[i].GetComponent<PlayerScript>().AddMultipleCardGOs(decks[i]);
            Debug.Log("Deck for " + players[i].name + " created");
        }
    }

    public GameObject GenerateSingleCardGO()
    {
        return cardGenerator.GenerateCard(existingCardGOs);
    }

    public void GenerateCardStack()
    {
        GameObject stackCardGO;
        while(true)
        {
            stackCardGO = cardGenerator.GenerateCard(existingCardGOs);
            var stackCard = stackCardGO.GetComponent<Card>();
            if(stackCard.symbol != "+2" &&
                stackCard.symbol != "+4" &&
                stackCard.symbol != "<>" &&
                stackCard.symbol != "!!" &&
                stackCard.symbol != "==")
            {
                break;
            }
            else
            {
                Destroy(stackCardGO);
            }
        }
        cardStack.SetFirstCard(stackCardGO);
    }

    private void UpdateExistingCards()
    {
        var newCardState = new List<GameObject>();
        players.ForEach(player => newCardState.AddRange(player.GetDeck()));
        newCardState.AddRange(cardStack.GetCardStack());
        var takeCard = takeStack.GetTakeCard();
        if (takeCard != null) newCardState.Add(takeCard);

        existingCardGOs = newCardState;
    }

    private void GetPlayers()
    {
        players.Clear();
        var goList = GameObject.FindGameObjectsWithTag("Player");
        foreach (var go in goList)
        {
            players.Add(go.GetComponent<PlayerScript>());
        }
    }

    public void SetCardsUnvisibleOf(PlayerScript player) => player.GetDeck().ForEach(card => { if (!card.CompareTag("stackCard")) card.SetActive(false); });

    public void SetCardsVisibleOf(PlayerScript player) => player.GetDeck().ForEach(card => card.SetActive(true));

    public void RearrangeCards()
    {
        //delete old cards from Player Deck
        var cardStackCard = cardStack.GetLastCard();
        foreach(var player in players)
        {
            foreach(var card in player.GetDeck())
            {
                if (cardStackCard != card) continue;
                player.RemoveCard(card);
                return;
            }
        }
    }

    public void SetCardDescription(string color, string symbol) => cardDescripion.text = color + " " + symbol;
}