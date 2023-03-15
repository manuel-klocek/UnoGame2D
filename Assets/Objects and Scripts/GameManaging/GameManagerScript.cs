using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManagerScript : MonoBehaviour
{
    public GameObject player;
    public GameObject currentPlayer;
    public Text currentPlayerName;
    public bool penaltyCardsLaying = false;
    public bool isEvent = false;
    private bool clockwiseRotation = true;
    private CardStackScript cardStack;
    private CardManagerScript cardManager;
    private ArtificialIntelligenceService ai;
    private readonly List<GameObject> players = new();

    // Start is called before the first frame update
    void Start()
    {
        cardStack = GameObject.FindGameObjectWithTag("cardStack").GetComponent<CardStackScript>();
        cardManager = GameObject.FindGameObjectWithTag("cardManager").GetComponent<CardManagerScript>();
        ai = GameObject.FindGameObjectWithTag("aiManager").GetComponent<ArtificialIntelligenceService>();

        //Instantiate Mock Players
        var player1 = Instantiate(player);
        player1.GetComponent<PlayerScript>().name = "Player 1";
        var player2 = Instantiate(player);
        player2.GetComponent<PlayerScript>().name = "Player 2";
        var bot1 = Instantiate(player);
        bot1.name = "Bot 1";
        bot1.tag = "bot";
        var bot2 = Instantiate(player);
        bot2.name = "Bot 2";
        bot2.tag = "bot";

        GetPlayers();

        cardManager.GenerateDecks(players);
        cardManager.GenerateCardStack();

        SetCurrentPlayer(player1);
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerName.text = currentPlayer.name;
    }

    private void SetCurrentPlayer(GameObject nextPlayer)
    {
        currentPlayer = nextPlayer;
        Debug.Log(currentPlayer.name + " is now playing...");
        foreach(var playerGO in players)
        {
            if (playerGO == nextPlayer)
            {
                var player = playerGO.GetComponent<PlayerScript>();
                cardManager.SetCardsVisibleOf(player);
                player.SortDeck();
                player.ReformatDeck();
                continue;
            }
            cardManager.SetCardsUnvisibleOf(playerGO.GetComponent<PlayerScript>());
        }
    }

    public void CheckForEvents()
    {
        var stackCardSymbol = cardStack.GetLastCard().GetComponent<Card>().symbol;
        var eventSymbolCollection = new List<string> { "!!", "<>", "+2", "+4", "==" };
        if (!eventSymbolCollection.Contains(stackCardSymbol)) return;
        if (!isEvent) return;
        if (stackCardSymbol == "==")
        {
            if (currentPlayer.CompareTag("bot")) ai.determineColor(currentPlayer.GetComponent<PlayerScript>().GetDeck());
            else { /* Let user decide color */ }
        }
        else if (stackCardSymbol == "<>")
        {
            clockwiseRotation = !clockwiseRotation;
            isEvent = false;
        }
        else if (stackCardSymbol == "!!")
        {
            NextPlayer(true);
            isEvent = false;
            return;
        }
        else if (stackCardSymbol == "+2")
        {
            if (!IsAbleToForward("+2"))
            {
                AddPenaltyCards(2);
                isEvent = false;
            }
            else
            {
                penaltyCardsLaying = true;
                isEvent = true;
            }
        }
        else if (stackCardSymbol == "+4")
        {
            if (!IsAbleToForward("+4"))
            {
                AddPenaltyCards(4);
                isEvent = false;
            }
            else
            {
                penaltyCardsLaying = true;
                isEvent = true;
            }
        }
        else penaltyCardsLaying = false;
        NextPlayer();
    }

    public void AddPenaltyCards(int cardsToTakeNum)
    {
        List<GameObject> list = new();
        for(int i = 0; i < cardsToTakeNum; i++)
        {
            list.Add(cardManager.GenerateSingleCardGO());
        }
        currentPlayer.GetComponent<PlayerScript>().AddMultipleCardGOs(list);
    }

    public void NextPlayer(bool skipPlayer = false, int? explicitPlayerIndex = null)
    {
        int nextPlayerIndex;
        if (explicitPlayerIndex != null) nextPlayerIndex = (int) explicitPlayerIndex;
        else nextPlayerIndex = GetNextPlayerIndex();

        if (skipPlayer) NextPlayer();

        SetCurrentPlayer(players[nextPlayerIndex]);

        CheckForEvents();

        if (currentPlayer.CompareTag("bot")) ai.ExecuteMove(currentPlayer, cardStack.GetLastCard());
    }

    private int GetNextPlayerIndex()
    {
        var currentPlayerIndex = players.FindIndex(player => player == currentPlayer);
        if (clockwiseRotation)
        {
            if (currentPlayerIndex == players.Count - 1) return 0;
            else return ++currentPlayerIndex;
        }
        else
        {
            if (currentPlayerIndex == 0) return players.Count - 1;
            else return --currentPlayerIndex;
        }
    }

    private void GetPlayers()
    {
        players.Clear();
        var playerGOs = GameObject.FindGameObjectsWithTag("Player");
        var botGOs = GameObject.FindGameObjectsWithTag("bot");
        foreach(var player in playerGOs) players.Add(player);
        foreach (var bot in botGOs) players.Add(bot);
    }

    private bool IsAbleToForward(string symbol)
    {
        foreach(var card in currentPlayer.GetComponent<PlayerScript>().GetDeck())
        {
            if (card.GetComponent<Card>().symbol == symbol) return true;
        }
        return false;
    }
}