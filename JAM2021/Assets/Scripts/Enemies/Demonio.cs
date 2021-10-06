using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demonio : Enemy
{

    int animAttackL, animAttackR, animClawL, animClawR;

    public override void Start()
    {
        base.Start();

        animAttackL = Animator.StringToHash("AttackL");
        animAttackR = Animator.StringToHash("AttackR");
        animClawL = Animator.StringToHash("ClawL");
        animClawR = Animator.StringToHash("ClawR");
    }

    public override void Attack()
    {
        base.Attack();

        if (canAnim)
        {
            int ani = Random.Range(0, 4);
            switch (ani)
            {
                case 0:
                    anim.SetTrigger(animAttackL);
                    break;
                case 1:
                    anim.SetTrigger(animAttackR);
                    break;
                case 2:
                    anim.SetTrigger(animClawL);
                    break;
                case 3:
                    anim.SetTrigger(animClawR);
                    break;
            }

        }
    }

    public void AttackEnd(AttackType type)
    {
        Debug.Log(name + " used " + type);

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
        //Sonido de daño demonio
        if (base.TakeDamage(d))
        {
            Die();
        }
        return false;
    }

    public override void Die()
    {
        //Sonido de muerte demonio
        base.Die();
    }

    public enum AttackType
    {
        SimpleL,
        SimpleR,
        ClawL,
        ClawR
    }
}
