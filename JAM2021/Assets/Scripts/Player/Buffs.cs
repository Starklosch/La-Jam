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
    bool hasBuff, hasSprintBuff, hasJumpBuff;
    float currentTime;
    PlayerController playerController;

    void Start()
    {
        currentType = BuffType.None;
        hasBuff = false;
        playerController = GetComponent<PlayerController>();
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
                hasSprintBuff = true;
                ActivateDurationBar(GameManager.Instance.CardsData[c].duration);
                break;
            case GameManager.Cards.Jump:
                currentType = BuffType.Jump;
                hasJumpBuff = true;
                ActivateDurationBar(GameManager.Instance.CardsData[c].duration);
                break;
            case GameManager.Cards.Heal:
                currentType = BuffType.Heal;
                GetComponent<ManaSystem>().Heal(GameManager.Instance.CardsData[c].damage);
                break;
        }
    }
    public void DisableBuff()
    {
        GameManager.Instance.GetCanvas().ShowBuffDurationBar(false);
        currentType = BuffType.None;
        hasBuff = false;
        hasSprintBuff = false;
        hasJumpBuff = false;
    }

    void ActivateDurationBar(float maxSecs)
    {
        hasBuff = true;
        currentTime = maxSecs;
        GameManager.Instance.GetCanvas().ShowBuffDurationBar(true);
        GameManager.Instance.GetCanvas().SetBuffDurationSliderMax(currentTime);
        GameManager.Instance.GetCanvas().SetBuffDurationSliderValue(currentTime);
    }
    public bool HasSprintBuff()
    {
        return hasSprintBuff;
    }
    public bool HasJumpBuff()
    {
        return hasJumpBuff;
    }
}
