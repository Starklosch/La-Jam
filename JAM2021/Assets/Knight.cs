using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Enemy
{
    int animAttack1, animAttack2;

    public override void Start()
    {
        base.Start();

        animAttack1 = Animator.StringToHash("AttackNormal");
        animAttack2 = Animator.StringToHash("AttackHeavy");
    }

    public override void Attack()
    {
        canAttack = false;

        if (canAnim)
        {
            int ani = Random.Range(0, 2);
            switch (ani)
            {
                case 0:
                    anim.SetTrigger(animAttack1);
                    break;
                case 1:
                    anim.SetTrigger(animAttack2);
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
        Heavy,
        Normal
    }
}
