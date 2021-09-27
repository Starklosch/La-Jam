using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esbirro : Enemy
{

    public override void Attack()
    {
        base.Attack();

    }

    public override void AttackEnd()
    {
        base.AttackEnd();
    }

    public override void Update()
    {
        base.Update();

        if (player != null)
        {
            if (canAttack && Time.time > attackCooldown && Vector3.Distance(player.transform.position, transform.position) < 5)
            {
                Attack();
            }
        }
    }
}
