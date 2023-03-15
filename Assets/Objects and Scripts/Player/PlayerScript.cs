using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerScript : MonoBehaviour
{
    public readonly float SPACE_BETWEEN_CARDS = 1;
    private readonly float VERTICAL_DISTANCE_OF_CARDS = -3;
    private List<GameObject> deck = new List<GameObject>();


    public void AddMultipleCardGOs(List<GameObject> cardGOs)
    {
        deck.AddRange(cardGOs);
    }

    public void AddCardGO(GameObject cardGO)
    {
        deck.Add(cardGO);
    }

    public List<GameObject> GetDeck()
    {
        return deck;
    }

    public GameObject RemoveCardByIndex(int index)
    {
        var cardGO = deck[index];
        deck.Remove(cardGO);
        return cardGO;
    }

    public GameObject RemoveCard(GameObject cardGO)
    {
        deck.Remove(cardGO);
        return cardGO;
    }

    public void SortDeck()
    {
        var colors = new List<string> { "black", "green", "blue", "yellow", "red" };

        var sortedDeck = new List<GameObject>();
        foreach (var color in colors)
        {
            foreach (var cardGO in deck)
            {
                if (cardGO.GetComponent<Card>().color == color)
                {
                    sortedDeck.Add(cardGO);
                }
            }
        }
        deck = sortedDeck;
    }

    //TODO frontend
    public void ReformatDeck()
    {
        float middleCardNum = deck.Count / 2;
        if (deck.Count % 2 != 0)
        {
            float x;
            float cardWidth = deck[0].GetComponent<BoxCollider2D>().size.x;

            for (int i = 0; i < deck.Count; i++)
            {
                if (i < middleCardNum)
                {
                    x = 0 - SPACE_BETWEEN_CARDS * (middleCardNum - i);
                }
                else if (i == middleCardNum)
                {
                    x = 0;
                }
                else
                {
                    x = 0 + SPACE_BETWEEN_CARDS * (i - middleCardNum);
                }

                deck[i].transform.position = new Vector2(x, VERTICAL_DISTANCE_OF_CARDS);
            }
        }
        else
        {
            float x;
            for (int i = 0; i < deck.Count; i++)
            {
                if (middleCardNum < 1) x = 0;
                else if (i == middleCardNum - 1)
                {
                    x = 0 - SPACE_BETWEEN_CARDS / 2;
                }
                else if (i == middleCardNum)
                {
                    x = 0 + SPACE_BETWEEN_CARDS / 2;
                }
                else if (i < middleCardNum)
                {
                    x = 0 - SPACE_BETWEEN_CARDS * (middleCardNum - i - 0.5f);
                }
                else
                {
                    x = 0 + SPACE_BETWEEN_CARDS * (i - middleCardNum + 0.5f);
                }

                deck[i].transform.position = new Vector2(x, VERTICAL_DISTANCE_OF_CARDS);
            }
        }
    }
}
