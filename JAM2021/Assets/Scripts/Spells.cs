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
        var proj = Instantiate(prefabs[(int)currentType], hand.position, Quaternion.identity).GetComponent<Projectile>();
        proj.direction = Camera.main.transform.forward;

        switch (currentType)
        {
            case SpellType.Poison:
                proj.Collision += (sender, args) =>
                {
                    Enemy e = args.Collision.gameObject.GetComponent<Enemy>();
                    if (e) e.Poison();
                    Projectile projComponent = (Projectile)sender;
                    Destroy(projComponent.gameObject);
                };
                break;
            case SpellType.Stun:
                proj.Collision += (sender, args) =>
                {
                    Enemy e = args.Collision.gameObject.GetComponent<Enemy>();
                    if (e) e.Stun();
                    Projectile projComponent = (Projectile)sender;
                    Destroy(projComponent.gameObject);
                };
                break;
            case SpellType.FireBall:
                proj.Collision += (sender, args) =>
                {
                    Enemy e = args.Collision.gameObject.GetComponent<Enemy>();
                    if (e) e.TakeDamage(3);
                    Projectile projComponent = (Projectile)sender;
                    Destroy(projComponent.gameObject);
                };
                break;
        }

        DisableSpell();
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
