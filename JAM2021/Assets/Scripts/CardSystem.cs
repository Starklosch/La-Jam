using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    Card[] inHand = new Card[3];

    List<Card> deck;

    public Vector3 force;

    public GameObject testCard;

    public float throwHeight = 1;

    public float maxMana;

    float mana;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = transform.position + Vector3.up * throwHeight;
            var direction = Vector3.Normalize(Camera.main.transform.forward);

            var card = Instantiate(testCard, position, Quaternion.identity);
            var bh = card.GetComponent<CardBehaviour>();
            bh.Force = direction;
        }
    }


    public Card TakeCard(int slot)
    {
        if (inHand[slot] != null)
            return null;

        while (true)
        {
            var rndCard = Random.Range(0, deck.Count - 1);
            var card = deck[rndCard];

            if (!card.InHand)
                continue;

            card.InHand = true;
            inHand[slot] = card;
            return card;
        }
    }

    public void AddCard(Card c)
    {
        deck.Add(c);
    }

    public void RemoveCard(Card c)
    {
        deck.Remove(c);
    }
}
