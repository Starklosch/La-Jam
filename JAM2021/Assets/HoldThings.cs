using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldThings : MonoBehaviour
{
    public GameObject cardHand;
    public GameObject weaponHand;

    public Card card;

    // Start is called before the first frame update
    void Start()
    {
        card.transform.SetParent(cardHand.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
