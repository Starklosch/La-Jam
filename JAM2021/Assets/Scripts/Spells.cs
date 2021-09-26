using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    public enum SpellType
    {
        //**Same order as Cards Enum***
        Poison,
        Stun,
        FireBall,

        None
    }
    //**Same order as SpellType order
    public GameObject[] prefabs;
    Transform hand;
    bool hasSpell;
    SpellType currentType;

    void Start()
    {
        currentType = SpellType.None;
        hasSpell = false;
        hand = transform.Find("Main Camera").Find("PlayerHand");
    }

    public bool HasSpellInHand()
    {
        return hasSpell;
    }

    public void ShootSpell()
    {
        Instantiate(prefabs[(int)currentType], hand.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*2, ForceMode.Impulse);
    }

    public void ActivateSpell(GameManager.Cards c)
    {
        DisableSpell();

        switch (c)
        {
            case GameManager.Cards.Poison:
                currentType = SpellType.Poison;
                break;
            case GameManager.Cards.Stun:
                currentType = SpellType.Stun;
                break;
            case GameManager.Cards.FireBall:
                currentType = SpellType.FireBall;
                break;
        }
        hasSpell = true;
    }

    public void DisableSpell()
    {
        currentType = SpellType.None;
        hasSpell = false;
    }
}
