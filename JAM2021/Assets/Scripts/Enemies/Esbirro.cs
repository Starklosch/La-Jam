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
        player.Mana.Harm(damage);

        attackCooldown = Time.deltaTime + attackTime;
        canAttack = true;
        IsStopped = false;
    }

    public override void Update()
    {
        base.Update();

    }

}
