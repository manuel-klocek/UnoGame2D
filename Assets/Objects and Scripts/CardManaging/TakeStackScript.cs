using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeStackScript : MonoBehaviour
{
    private GameObject takeCard;
    private CardManagerScript cardManager;

    void Start()
    {
        cardManager = GameObject.FindGameObjectWithTag("cardManager").GetComponent<CardManagerScript>();
        SetTakeCard();
    }

    public GameObject GetTakeCard()
    {
        return takeCard;
    }

    private void ArrangeTakeStack()
    {
        takeCard.transform.position = new Vector2(-5, 1.5f);
    }

    private void SetTakeCard()
    {
        if (takeCard == null)
        {
            takeCard = cardManager.GenerateSingleCardGO();
            takeCard.tag = "takeCard";
        }
        Debug.Log("TakeCard generated and set!");
        ArrangeTakeStack();
    }

    public GameObject UseTakeCard()
    {
        var card = takeCard;
        takeCard = null;
        SetTakeCard();
        Debug.Log(card.GetComponent<Card>().color + " " + card.GetComponent<Card>().symbol + " got added to Player");
        return card;
    }
}
