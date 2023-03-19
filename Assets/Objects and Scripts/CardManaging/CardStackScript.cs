using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStackScript : MonoBehaviour
{
    private CardManagerScript cardManager;
    private readonly List<GameObject> cardStack = new();
    private GameManagerScript gameManager;

    void Start()
    {
        cardManager = GameObject.FindGameObjectWithTag("cardManager").GetComponent<CardManagerScript>();
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManagerScript>();
        if(cardStack.Count == 0) SetFirstCard(cardManager.GenerateSingleCardGO());
    }

    void Update()
    {
        ReformatCardStack();
    }

    public void SetFirstCard(GameObject cardGO)
    {
        cardStack.Add(cardGO);
    }

    public List<GameObject> GetCardStack()
    {
        return cardStack;
    }

    public void AddCardGO(GameObject cardGO)
    {
        var oldStackCard = cardStack[^1].GetComponent<Card>();
        if(IsSameOrFirstActionCard(cardGO)) return;
        cardStack.ForEach(cardGO => Destroy(cardGO));
        cardStack.Clear();
        cardStack.Add(cardGO);
        Debug.Log("CardStack cleared!");
        Debug.Log(cardGO.GetComponent<Card>().color + " " + cardGO.GetComponent<Card>().symbol + " added to CardStack");
    }

    public GameObject GetLastCard()
    {
        return cardStack[^1];
    }

    public void ReformatCardStack()
    {
        foreach (var card in cardStack)
        {
            card.transform.position = new Vector2(0, 0);
        }
    }

    private bool IsSameOrFirstActionCard(GameObject cardGO)
    {
        if (cardStack[^1].GetComponent<Card>().symbol == cardGO.GetComponent<Card>().symbol &&
            (cardStack[^1].GetComponent<Card>().symbol == "+2" || cardStack[^1].GetComponent<Card>().symbol == "+4"))
        {
            cardStack.Add(cardGO);
            Debug.Log(cardGO.GetComponent<Card>().color + " " + cardGO.GetComponent<Card>().symbol + " added to CardStack");
            return true;
        }
        return false;
    }
}
