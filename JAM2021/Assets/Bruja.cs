using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruja : Enemy
{

    int animSpell1;
    int animSpell2;

    public override void Start()
    {
        base.Start();

        animSpell1 = Animator.StringToHash("Spell1");
        animSpell2 = Animator.StringToHash("Spell2");
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
                    anim.SetTrigger(animSpell1);
                    break;
                case 1:
                    anim.SetTrigger(animSpell2);
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

    public override void Update()
    {
        base.Update();
    }

    public enum AttackType
    {
        Spell1,
        Spell2
    }
}
