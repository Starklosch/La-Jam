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


    public float throwHeight = 1;
    public float maxMana = 100;
    public float manaFillSpeed = 10;

    public Slider manaSlider;
    public TextMeshProUGUI manaText;

    public float cardCost = 10;

    float mana = 0;

    private void Start()
    {
        manaSlider.maxValue = maxMana;
        mana = maxMana;
        manaText.text = mana.ToString();
        manaSlider.value = mana;
    }

    private void Update()
    {
        // Mana
        if (mana < 100)
        {
            mana = Mathf.Clamp(mana + manaFillSpeed * Time.deltaTime, 0, maxMana);
            manaSlider.value = mana;
            manaText.text = mana.ToString("0,0");
        }

        // Usar carta
        if (Input.GetMouseButtonDown(0) && mana > cardCost)
        {
            mana -= cardCost;

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
