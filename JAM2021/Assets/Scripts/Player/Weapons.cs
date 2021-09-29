using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
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
    
    public void ActivateWeapon(GameManager.Cards c)
    {
        DisableWeapon();

        if (!hand) return;
        currentWeapon = hand.GetChild((int)c).gameObject;
        currentWeapon.SetActive(true);
        hasWeapon = true;
        GameManager.Instance.GetCanvas().ShowDurabilityBar(true);

        currentDamage = GameManager.Instance.CardsData[c].damage;
        currentDurability = GameManager.Instance.CardsData[c].duration;
        GameManager.Instance.GetCanvas().SetWDurabilitySliderMax((int)currentDurability);
        GameManager.Instance.GetCanvas().SetWDurabilitySliderValue((int)currentDurability);
    }

    public void DisableWeapon()
    {
        if (currentWeapon != null) currentWeapon.SetActive(false);
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
        Debug.Log("a");
        if (hitRatio > Time.time - lastTime) return;

        hit.SetActive(true);

        Debug.Log("Pegar");

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
