using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum WeaponType
    {
        //**Same order as Cards Enum***
        Sword,
        Axe,
        Hammer,

        None
    }

    GameObject currentWeapon = null;
    Transform hand;
    void Start()
    {
        hand = transform.Find("Main Camera").Find("PlayerHand");
    }
    
    public void ActivateWeapon(GameManager.Cards c)
    {
        DisableWeapon();

        Debug.Log(hand);
        if (!hand) return;
        currentWeapon = hand.GetChild((int)c).gameObject;
        currentWeapon.SetActive(true);
    }

    public void DisableWeapon()
    {
        if (currentWeapon != null) currentWeapon.SetActive(false);
        currentWeapon = null;
    }
}
