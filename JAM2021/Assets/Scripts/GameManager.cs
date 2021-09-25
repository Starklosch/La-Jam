using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    GameObject canvasInstance;
    CardsSelectorDisplay cardsSD;

    public void setCanvas()
    {
        canvasInstance = GameObject.Find("Canvas");
        cardsSD = canvasInstance.transform.GetChild(2).GetComponent<CardsSelectorDisplay>();
    }

    Queue<Cards> deck = new Queue<Cards>();
    Cards[] hand = new Cards[3];
    int cursorIndex = 0;

    public enum Cards
    {
        Sword,
        Hammer,
        Teleport,
        Poison,
        Speed,
        Heal,
        None
    }

    private void Start()
    {
        deck.Enqueue(Cards.Sword);
        deck.Enqueue(Cards.Teleport);
        deck.Enqueue(Cards.Heal);
        hand[0] = Cards.Sword;
        hand[1] = Cards.Teleport;
        hand[2] = Cards.Heal;

        setCanvas();
    }

    public void AddToDeck(Cards card)
    {
        deck.Enqueue(card);
    }

    public Cards RemoveFromDeck()
    {
        if (deck.Count > 0)
            return deck.Dequeue();
        else return Cards.None;
    }

    public void MoveCursor(int m)
    {
        cursorIndex -= m;
        if (cursorIndex < 0) cursorIndex = 2;
        else if (cursorIndex > 2) cursorIndex = 0;

        cardsSD.UpdateCursorUI(cursorIndex);
    }
}
