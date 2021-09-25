using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSystem : MonoBehaviour
{
    Cards[] inHand = new Cards[3];

    List<Cards> deck;

    public GameObject testCard;

    public ManaSystem manaSystem;

    public float throwHeight = 1;

    public float cardCost = 10;


    private void Start()
    {
        if (manaSystem == null && !TryGetComponent(out manaSystem))
        {
            Debug.LogError("manaSystem is null");
        }
    }

    private void Update()
    {

        // Usar carta
        if (Input.GetMouseButtonDown(0) && manaSystem.Mana > cardCost)
        {
            manaSystem.UseMana(cardCost);

            var position = transform.position + Vector3.up * throwHeight;
            var direction = Vector3.Normalize(Camera.main.transform.forward);

            var card = Instantiate(testCard, position, Quaternion.identity);
            var bh = card.GetComponent<CardBehaviour>();
            bh.Force = direction;
        }
    }


    public Cards TakeCard(int slot)
    {
        while (true)
        {
            var rndCard = Random.Range(0, deck.Count - 1);
            var card = deck[rndCard];

            inHand[slot] = card;
            return card;
        }
    }

    public void AddCard(Cards c)
    {
        deck.Add(c);
    }

    public void RemoveCard(Cards c)
    {
        deck.Remove(c);
    }

    public enum Cards
    {
        Null,
        Teleport
    }
}
