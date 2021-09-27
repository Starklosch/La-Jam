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

    bool m_holdCard, m_equiped;

    bool HoldCard
    {
        get => m_holdCard;
        set
        {
            m_holdCard = value;

            anim.SetBool(animHoldCard, value);
        }
    }

    bool Equiped
    {
        get => m_equiped;
        set
        {
            m_equiped = value;

            anim.SetBool(animEquiped, value);
        }
    }

    public void PlayCard()
    {
        anim.SetTrigger(animPlayCard);
    }

    public void Attack()
    {
        anim.SetTrigger(animAttack);
    }

    void PlayCardStart()
    {
        HoldCard = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        animHoldCard = Animator.StringToHash("HoldCard");
        animPlayCard = Animator.StringToHash("PlayCard");
        animEquiped = Animator.StringToHash("Equiped");
        animAttack = Animator.StringToHash("Attack");
    }

}
