using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    [SerializeField] float baseHealth = 100;

    [SerializeField] float maxMana = 100;
    [SerializeField] float manaFillSpeed = 10;

    [SerializeField] Slider manaSlider;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI healthText;

    [Min(0)]
    float m_health = 0, m_mana = 0;

    public float Health
    {
        get => m_health;
        private set
        {
            m_health = Mathf.Clamp(value, 0, baseHealth);
            healthSlider.value = value;
            healthText.text = value.ToString("0,0");
        }
    }

    public float Mana
    {
        get => m_mana;
        private set
        {
            m_mana = Mathf.Clamp(value, 0, maxMana);
            manaSlider.value = value;
            manaText.text = value.ToString("0,0");
        }
    }

    public float MaxMana
    {
        get => maxMana;
        set
        {
            maxMana = value;
            manaSlider.maxValue = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start full health and mana
        Health = baseHealth;
        Mana = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mana < maxMana)
        {
            Mana += manaFillSpeed * Time.deltaTime;
        }
    }

    public bool UseMana(float mana)
    {
        if (mana > Mana)
        {
            Debug.LogError("Not enough mana.");
            return false;
        }

        Mana -= mana;
        return true;
    }

    public void Heal(float healing)
    {
        if (Health < baseHealth)
            Health += healing;
    }

    public void Harm(float damage)
    {
        if (Health > 0)
            Health -= damage;
    }
}
