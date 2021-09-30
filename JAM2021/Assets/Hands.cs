using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    Animator anim;

    int animHoldCard = 0;
    int animPlayCard = 0;
    int animEquiped = 0;
    int animAttack = 0;
    int animCastSpell = 0;

    //bool m_holdCard, m_equiped;

    public bool HoldCard
    {
        get => anim.GetBool(animHoldCard);
        set => anim.SetBool(animHoldCard, value);
    }

    public bool Equiped
    {
        get => anim.GetBool(animEquiped);
        set => anim.SetBool(animEquiped, value);
    }

    public void PlayCard()
    {
        anim.SetTrigger(animPlayCard);
    }

    public void Attack()
    {
        anim.SetTrigger(animAttack);
    }

    public void Equip()
    {
        if (!Equiped)
            Equiped = true;
    }

    public void CastSpell()
    {
        anim.SetTrigger(animCastSpell);
    }

    public void ShowWeapon()
    {
        Debug.Log("Show");
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        animHoldCard = Animator.StringToHash("HoldCard");
        animPlayCard = Animator.StringToHash("PlayCard");
        animEquiped = Animator.StringToHash("Equiped");
        animAttack = Animator.StringToHash("Attack");
        animCastSpell = Animator.StringToHash("CastSpell");

        HoldCard = true;
    }

}
