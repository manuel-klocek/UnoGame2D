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
    private readonly List<GameObject> players = new();

    // Start is called before the first frame update
    void Start()
    {
        cardStack = GameObject.FindGameObjectWithTag("cardStack").GetComponent<CardStackScript>();
        cardManager = GameObject.FindGameObjectWithTag("cardManager").GetComponent<CardManagerScript>();

        //Instantiate Mock Players
        var player1 = Instantiate(player);
        player1.GetComponent<PlayerScript>().name = "Player 1";
        var player2 = Instantiate(player);
        player2.GetComponent<PlayerScript>().name = "Player 2";
        var player3 = Instantiate(player);
        player3.GetComponent<PlayerScript>().name = "Player 3";
        var player4 = Instantiate(player);
        player4.GetComponent<PlayerScript>().name = "Player 4";

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
            /* Let user decide color */ 
        }
        else if (stackCardSymbol == "<>")
        {
            clockwiseRotation = !clockwiseRotation;
            isEvent = false;
        }
        else if (stackCardSymbol == "!!")
        {
            isEvent = false;
            NextPlayer(true);
            return;
        }
        else if (stackCardSymbol == "+2")
        {
            penaltyCardsLaying = true;
            isEvent = false;
        }
        else if (stackCardSymbol == "+4")
        {
            /* Let user decide color */
            penaltyCardsLaying = true;
            isEvent = false;
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
            print(currentPlayer.name + " recieved 1 penalty card");
        }
        currentPlayer.GetComponent<PlayerScript>().AddMultipleCardGOs(list);
        penaltyCardsLaying = false;
    }

    public void NextPlayer(bool skipPlayer = false, int? explicitPlayerIndex = null)
    {
        int nextPlayerIndex;
        if (explicitPlayerIndex != null) nextPlayerIndex = (int) explicitPlayerIndex;
        else nextPlayerIndex = GetNextPlayerIndex();

        SetCurrentPlayer(players[nextPlayerIndex]);

        if (skipPlayer) NextPlayer();

        CheckForEvents();
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
        foreach(var player in playerGOs) players.Add(player);
    }
}