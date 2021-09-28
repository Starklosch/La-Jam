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

    protected override void AttackEnd()
    {
        base.AttackEnd();

        player.Mana.Harm(damage);
    }

    public override void Update()
    {
        base.Update();

        if (player != null)
        {
            var pDistance = Vector3.Distance(player.transform.position, transform.position);
            if (canAttack)
            {
                if (pDistance < attackDistance)
                {
                    if (Time.time > attackCooldown)
                        Attack();
                }
                else if (pDistance < chaseDistance)
                {
                    Chase();
                }
            }
        }
    }

    public override void Chase()
    {
        nav.SetDestination(player.transform.position);
    }
}
