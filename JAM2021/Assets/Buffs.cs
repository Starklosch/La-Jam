using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    public enum BuffType
    {
        //**Same order as Cards Enum***
        Speed,
        Jump,
        Heal,

        None
    }

    BuffType currentType;
    bool hasBuff;
    float currentTime;

    void Start()
    {
        currentType = BuffType.None;
        hasBuff = false;
    }

    void Update()
    {
        if (hasBuff)
        {
            if(currentTime <= 0) DisableBuff();
            else
            {
                GameManager.Instance.GetCanvas().SetBuffDurationSliderValue(currentTime);
                currentTime -= Time.deltaTime;
            }
        }
    }

    public void ActivateBuff(GameManager.Cards c)
    {
        if(hasBuff) DisableBuff();

        switch (c)
        {
            case GameManager.Cards.Speed:
                currentType = BuffType.Speed;
                
                ActivateDurationBar(5);
                break;
            case GameManager.Cards.Jump:
                currentType = BuffType.Jump;
                
                ActivateDurationBar(5);
                break;
            case GameManager.Cards.Heal:
                currentType = BuffType.Heal;
                GetComponent<ManaSystem>().Heal(5);
                break;
        }
    }
    public void DisableBuff()
    {
        GameManager.Instance.GetCanvas().ShowBuffDurationBar(false);
        currentType = BuffType.None;
        hasBuff = false;
    }

    void ActivateDurationBar(float maxSecs)
    {
        hasBuff = true;
        currentTime = maxSecs;
        GameManager.Instance.GetCanvas().ShowBuffDurationBar(true);
        GameManager.Instance.GetCanvas().SetBuffDurationSliderMax(currentTime);
        GameManager.Instance.GetCanvas().SetBuffDurationSliderValue(currentTime);
    }
}
