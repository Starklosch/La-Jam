using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected int health;
    protected Slider healthSlider;
    float currentStunDuration;
    bool hasPoison;
    bool hasStun;
    public virtual void Start()
    {
        healthSlider = transform.Find("CanvasEnemy").Find("HealthBar").GetComponent<Slider>();
    }

    public virtual void Update()
    {
        if (hasStun)
        {
            currentStunDuration -= Time.deltaTime;
            if (currentStunDuration <= 0)
            {
                hasStun = false;
                Debug.Log("FREE OF STUN");
            }
        }
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

        if (health <= 0) return true;

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
    void PoisonDamage()
    {
        TakeDamage(1);
    }

    void StopPoison()
    {
        CancelInvoke("PoisonDamage");
        hasPoison = false;
    }

    public void Poison(float duration)
    {
        StopPoison();

        Debug.Log("POISONED");
        hasPoison = true;
        InvokeRepeating("PoisonDamage", 0f, 1f);
        Invoke("StopPoison", duration);
    }

    public void Stun(float duration)
    {
        hasStun = true;
        currentStunDuration = duration;
        Debug.Log("STUNNED");
    }
}
