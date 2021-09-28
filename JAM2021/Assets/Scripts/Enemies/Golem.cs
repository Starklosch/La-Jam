using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    [Tooltip("Vida máxima del enemigo.")]
    public int maxHealth = 10;

    int animAttackMelee;
    int animAttackRange;

    public override void Start()
    {
        base.Start();
        
        animAttackMelee = Animator.StringToHash("AttackMelee");
        animAttackRange = Animator.StringToHash("AttackRange");
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

    public override void Heal(int h)
    {
        base.Heal(h);
    }

    public override bool TakeDamage(int d)
    {
        //Sonido de daño golem
        if (base.TakeDamage(d))
        {
            Die();
        }
        return false;
    }

    public override void Die()
    {
        //Sonido de muerte golem
        base.Die();
    }

    public override void Attack()
    {
        base.Attack();

        if (canAnim)
        {
            int ani = Random.Range(0, 2);
            switch (ani)
            {
                case 0:
                    anim.SetTrigger(animAttackMelee);
                    break;
                case 1:
                    anim.SetTrigger(animAttackRange);
                    break;
                default:
                    break;
            }

        }
    }

    public void AttackEnd(AttackType type)
    {
        AttackEnd();

        Debug.Log(name + " used " + type);
        player.Mana.Harm(damage);
    }

    public override void Chase()
    {
        Debug.Log(nav);
        nav.SetDestination(player.transform.position);
    }

    public enum AttackType
    {
        Melee,
        Range
    }
}
