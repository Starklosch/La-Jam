using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [Tooltip("Referencia a la mano del jugador")]
    public Hands handAnimation;

    GameObject currentWeapon = null;
    GameObject hit;
    bool hasWeapon;
    public Transform hand;
    //Espera entre golpeo y golpeo
    [Tooltip("Espera entre golpeo y golpeo")]
    public float hitRatio = 0.4f;
    //Tiempo que aparece el cubo hit
    float cooldownHit= 0.1f;
    float lastTime = 0f;

    //Weapon stats
    float currentDamage;
    float currentDurability;

    Action pendingTask;

    void Start()
    {
        currentDamage = 0;
        hasWeapon = false;
        hit = transform.Find("MeleeHit").gameObject;
    }

    void Update()
    {
        if (hit.activeInHierarchy && cooldownHit < Time.time - lastTime) hit.SetActive(false);
    }
    
    public void EnableWeapon(GameManager.Cards c)
    {
        if (!hand) return;

        Action enableAction = delegate ()
        {
            currentWeapon = hand.GetChild((int)c).gameObject;
            currentWeapon.SetActive(true);
            hasWeapon = true;
            GameManager.Instance.GetCanvas().ShowDurabilityBar(true);

            currentDamage = GameManager.Instance.CardsData[c].damage;
            currentDurability = GameManager.Instance.CardsData[c].duration;
            GameManager.Instance.GetCanvas().SetWDurabilitySliderMax((int)currentDurability);
            GameManager.Instance.GetCanvas().SetWDurabilitySliderValue((int)currentDurability);
        };

        //if (currentWeapon != null)
        //{
        GameManager.Instance.PlayerInstance.Hands.Equip(delegate ()
        {
            if (currentWeapon != null)
                DisableWeaponAction();

            enableAction();
        });
        //}
        //else
        //{
        //    GameManager.Instance.PlayerInstance.Hands.Equip(enableAction);
        //}

    }

    public void DisableWeapon(/*Action doAfter = null*/)
    {
        if (currentWeapon == null) return;

        GameManager.Instance.PlayerInstance.Hands.Unequip(DisableWeaponAction);
    }

    void DisableWeaponAction()
    {
        currentWeapon.SetActive(false);
        currentWeapon = null;
        hasWeapon = false;
        currentDamage = 0;
        GameManager.Instance.GetCanvas().ShowDurabilityBar(false);
    }

    public bool HasWeaponInHand()
    {
        return hasWeapon;
    }

    public void UseWeapon()
    {
        if (hitRatio > Time.time - lastTime) return;

        hit.SetActive(true);

        lastTime = Time.time;
    }

    public void UseDurability()
    {
        currentDurability--;
        GameManager.Instance.GetCanvas().SetWDurabilitySliderValue((int)currentDurability);
        if (currentDurability <= 0) DisableWeapon();
    }

    public float GetCurrentDamage()
    {
        return currentDamage;
    }
}
