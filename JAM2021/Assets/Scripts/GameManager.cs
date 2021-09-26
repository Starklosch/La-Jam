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
        cardsSD = canvasInstance.GetComponentInChildren<CardsSelectorDisplay>();
    }

    int typeWeapon = 0, typeSpell = 0, typeSupport = 0;
    Queue<Cards> deck = new Queue<Cards>();
    List<Cards> discarded = new List<Cards>();
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

    public void OpenChest()
    {
        //Random card
        //Dependiendo si se acepta o no se añade al deck
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

    bool EmptyHand()
    {
        return (hand[0]==Cards.None && hand[1] == Cards.None && hand[2] == Cards.None);
    }

    void ShuffleDiscarded()
    {
        //O(n/2)
        for (int i = 0; i < discarded.Count/2; i++)
        {
            Cards temp = discarded[i];
            int randomIndex = Random.Range(i, discarded.Count);
            discarded[i] = discarded[randomIndex];
            discarded[randomIndex] = temp;
        }
    }

    void MoveDiscardedToDeck()
    {
        foreach (var c in discarded)
        {
            deck.Enqueue(c);
        }
        discarded.Clear();
    }

    public void UseCard()
    {
        if (hand[cursorIndex] == Cards.None /*|| CD*/) return;
        switch (hand[cursorIndex])
        {
            case Cards.Sword:
                //Las de tipo arma instanciaran un objeto desde GameManager y se comunicaran con un componente Weapon del jugador para indicar que tiene x arma.
                break;
            case Cards.Hammer:
                break;
            case Cards.Teleport:
                //Las de tipo hechizo se comunican con un componente ThrowSpell del jugador, para lanzar la carta y realizar el efecto.
                break;
            case Cards.Poison:
                break;
            case Cards.Speed:
                //Las de tipo soporte llaman a metodos del componente Support del jugador para aplicar sus efectos.
                break;
            case Cards.Heal:
                break;
        }

        discarded.Add(hand[cursorIndex]);
        hand[cursorIndex] = Cards.None;
        //CD de uso empieza
        //Animacion de descarte

        if (deck.Count == 0 && EmptyHand())
        {
            //Animacion de barajar y mover a deck

            ShuffleDiscarded();
            MoveDiscardedToDeck();
            for(int i = 0; i < 3; i++)
            {
                hand[i] = RemoveFromDeck();
            }
        }
        else if (deck.Count>0)
        {
            //Animacion de robar si hay cartas en deck
            hand[cursorIndex] = RemoveFromDeck();
        }
    }

    public void MoveCursor(int m)
    {
        cursorIndex -= m;
        if (cursorIndex < 0) cursorIndex = 2;
        else if (cursorIndex > 2) cursorIndex = 0;

        cardsSD.UpdateCursorUI(cursorIndex);
    }

    public void SetCursor(int i)
    {
        cursorIndex = i;
        cardsSD.UpdateCursorUI(cursorIndex);
    }
}
