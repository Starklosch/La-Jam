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
    GameObject hit;
    bool hasWeapon;
    Transform hand;
    //Espera entre golpeo y golpeo
    [Tooltip("Espera entre golpeo y golpeo")]
    public float hitRatio = 0.4f;
    //Tiempo que aparece el cubo hit
    float cooldownHit= 0.1f;
    float lastTime = 0f;

    //Weapon stats
    int currentDamage;
    int currentDurability;
    void Start()
    {
        currentDamage = 0;
        hasWeapon = false;
        hand = transform.Find("Main Camera").Find("PlayerHand");
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
        switch ((WeaponType)c)
        {
            case WeaponType.Sword:
                currentDamage = 1;
                currentDurability = 5;
                GameManager.Instance.GetCanvas().SetWDurabilitySliderMax(currentDurability);
                GameManager.Instance.GetCanvas().SetWDurabilitySliderValue(currentDurability);
                break;
            case WeaponType.Axe:
                currentDamage = 2;
                currentDurability = 5;
                GameManager.Instance.GetCanvas().SetWDurabilitySliderMax(currentDurability);
                GameManager.Instance.GetCanvas().SetWDurabilitySliderValue(currentDurability);
                break;
            case WeaponType.Hammer:
                currentDamage = 3;
                currentDurability = 5;
                GameManager.Instance.GetCanvas().SetWDurabilitySliderMax(currentDurability);
                GameManager.Instance.GetCanvas().SetWDurabilitySliderValue(currentDurability);
                break;
        }
            
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
        if (hitRatio > Time.time - lastTime) return;

        hit.SetActive(true);

        lastTime = Time.time;
    }

    public void UseDurability()
    {
        currentDurability--;
        GameManager.Instance.GetCanvas().SetWDurabilitySliderValue(currentDurability);
        if (currentDurability <= 0) DisableWeapon();
    }
}
