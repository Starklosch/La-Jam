using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected int health;
    protected Slider healthSlider;
    public virtual void Start()
    {
        healthSlider = transform.Find("CanvasEnemy").Find("HealthBar").GetComponent<Slider>();
    }

    public virtual void Heal(int h)
    {
        health += h;

        if (health > healthSlider.maxValue) health = (int)healthSlider.maxValue;

        UpdateHealthBarHUD();
    }

    public virtual bool TakeDamage(int d)
    {
        health -= d;

        UpdateHealthBarHUD();

        if (health < 0) return true;

        return false;
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    void UpdateHealthBarHUD()
    {
        healthSlider.value = health;
    }
}
