using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    Animator anim;

    int animHoldCard = 0;
    int animPlayCard = 0;
    int animEquiped = 0;
    int animEquipAgain = 0;
    int animAttack = 0;
    int animCastSpell = 0;

    //bool m_holdCard, m_equiped;
    //public event Action WeaponShown;
    Queue<Action> weaponTasks;
    Queue<Action> cardTasks;

    bool wait;

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

    public void PlayCard(Action action = null)
    {
        anim.SetTrigger(animPlayCard);

        if (action != null)
            cardTasks.Enqueue(action);
    }

    public void Attack()
    {
        anim.SetTrigger(animAttack);
    }

    public void Equip(Action action = null)
    {
        // Equip again
        if (Equiped)
            anim.SetTrigger(animEquipAgain);
        else
            Equiped = true;

        if (action != null)
            weaponTasks.Enqueue(action);
    }

    public void Unequip(Action action = null)
    {
        if (!Equiped)
            return;

        Equiped = false;

        if (action != null)
            weaponTasks.Enqueue(action);
    }

    public void CastSpell()
    {
        anim.SetTrigger(animCastSpell);
    }

    public void ToggleWeapon()
    {
        if (weaponTasks.Count > 0)
            weaponTasks.Dequeue()?.Invoke();
    }

    public void ToggleCard()
    {
        if (cardTasks.Count > 0)
            cardTasks.Dequeue()?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        animHoldCard = Animator.StringToHash("HoldCard");
        animPlayCard = Animator.StringToHash("PlayCard");
        animEquiped = Animator.StringToHash("Equiped");
        animEquipAgain = Animator.StringToHash("EquipAgain");
        animAttack = Animator.StringToHash("Attack");
        animCastSpell = Animator.StringToHash("CastSpell");

        //anim.get

        weaponTasks = new Queue<Action>();
        cardTasks = new Queue<Action>();
    }

}
