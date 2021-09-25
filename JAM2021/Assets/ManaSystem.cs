using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    [SerializeField] float maxMana = 100;
    [SerializeField] float manaFillSpeed = 10;

    [SerializeField] Slider manaSlider;
    [SerializeField] TextMeshProUGUI manaText;

    float mana = 0;

    public float Mana { get => mana; }
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
        manaSlider.maxValue = maxMana;
        mana = maxMana;
        manaText.text = mana.ToString("0,0");
        manaSlider.value = mana;
    }

    // Update is called once per frame
    void Update()
    {
        if (mana < maxMana)
        {
            mana = Mathf.Clamp(mana + manaFillSpeed * Time.deltaTime, 0, maxMana);
            manaSlider.value = mana;
            manaText.text = mana.ToString("0,0");
        }
    }

    public void UseMana(float mana)
    {
        if (mana > this.mana)
        {
            Debug.LogError("Not enough mana.");
            return;
        }

        this.mana -= mana;
    }
}
