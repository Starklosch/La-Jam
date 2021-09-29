using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    int animAttackMelee;
    int animAttackRange;

    public override void Start()
    {
        base.Start();
        
        animAttackMelee = Animator.StringToHash("AttackMelee");
        animAttackRange = Animator.StringToHash("AttackRange");
    }

    //public override void Heal(int h)
    //{
    //    base.Heal(h);
    //}

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
        Debug.Log(name + " used " + type);
        
        if (Physics.CheckSphere(nav.destination, 3, playerMask))
            player.Mana.Harm(damage);

        attackCooldown = Time.deltaTime + attackTime;
        canAttack = true;
        IsStopped = false;
    }

    private void OnDrawGizmos()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.red;

        if (nav != null)
            Gizmos.DrawWireSphere(nav.destination, 3);
        Gizmos.color = color;
    }

    //public override void Chase()
    //{
    //    base.Chase();
    //    //Debug.Log(nav);
    //    nav.SetDestination(player.transform.position);
    //}

    public override void Update()
    {
        base.Update();

    }

    public enum AttackType
    {
        Melee,
        Range
    }
}
