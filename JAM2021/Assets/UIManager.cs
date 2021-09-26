using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    CardsSelectorDisplay cardsSelectorDisplay;
    Transform weaponDurabilityBar;
    Transform eKey;
    Transform chestPanels;
    void Start()
    {
        cardsSelectorDisplay = GetComponentInChildren<CardsSelectorDisplay>();
        eKey = transform.Find("PlayerUI").Find("KeyE");
        chestPanels = transform.Find("ChestPanels");
        GameManager.Instance.SetCanvas(this);
        weaponDurabilityBar = transform.Find("WeaponDurabilityBar");
    }

    public void SetWDurabilitySliderMax(int i)
    {
        weaponDurabilityBar.GetComponent<Slider>().maxValue = i;
    }

    public void SetWDurabilitySliderValue(int i)
    {
        weaponDurabilityBar.GetComponent<Slider>().value = i;
    }

    public void UpdateCursor(int i)
    {
        cardsSelectorDisplay.UpdateCursorUI(i);
    }

    public void ShowE()
    {
        eKey.GetComponent<Text>().enabled = true;
    }
    public void HideE()
    {
        eKey.GetComponent<Text>().enabled = false;
    }

    public bool IsEActive()
    {
        return eKey.GetComponent<Text>().IsActive();
    }

    public void ShowChestPanel(int i, GameManager.Cards newCard, GameManager.Cards deckCard = GameManager.Cards.None)
    {
        switch ((Chest.ChestType)i)
        {
            case Chest.ChestType.Reward:
                chestPanels.Find("NewCardPanel").gameObject.SetActive(true);
                //Se muestra la newCard en la imagen
                break;
            case Chest.ChestType.ForcedTrade:
                chestPanels.Find("TradeCardPanel").gameObject.SetActive(true);
                //Se muestra la carta descartada deckCard y la nueva carta newCard
                break;
            case Chest.ChestType.Trap:
                chestPanels.Find("DiscardCardPanel").gameObject.SetActive(true);
                //Se muestra la carta descartada deckCard del deck
                break;
        }
    }
    public void AcceptChestUI()
    {
        foreach(Transform panel in chestPanels){
            if (panel.gameObject.activeInHierarchy) panel.gameObject.SetActive(false);
        }
        GameManager.Instance.AcceptChest();
    }
    public void DiscardChestUI()
    {
        foreach (Transform panel in chestPanels)
        {
            if (panel.gameObject.activeInHierarchy) panel.gameObject.SetActive(false);
        }
        GameManager.Instance.DiscardChest();
    }
}
