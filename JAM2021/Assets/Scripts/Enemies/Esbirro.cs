using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Esbirro : Enemy
{

    public override void Attack()
    {
        base.Attack();

        AttackAnim();
    }

    protected void AttackEnd()
    {
        Debug.Log(name + " attacked");

        if (Physics.CheckSphere(nav.destination, attackRadius, playerMask))
            player.Mana.Harm(damage);

        attackCooldown = Time.deltaTime + attackTime;
        canAttack = true;
        IsStopped = false;
    }

    public override void Update()
    {
        base.Update();

    }

    public override bool TakeDamage(int d)
    {
        //Sonido de daño esbirro
        if (base.TakeDamage(d))
        {
            Die();
        }
        return false;
    }

    public override void Die()
    {
        //Sonido de muerte esbirro
        base.Die();
    }

}
