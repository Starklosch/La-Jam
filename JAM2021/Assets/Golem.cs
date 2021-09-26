using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    [Tooltip("Vida máxima del enemigo.")]
    public int maxHealth = 10;

    public override void Start()
    {
        base.Start();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        health = maxHealth;
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
}
