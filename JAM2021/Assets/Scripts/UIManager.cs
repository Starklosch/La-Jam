using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    CardsSelectorDisplay cardsSelectorDisplay;
    Transform weaponDurabilityBar;
    Transform buffDurationBar;
    Transform eKey;
    Transform chestPanels;
    Transform pauseMenu;
    Text deckCountText;
    Animator shuffledText;
    void Start()
    {
        cardsSelectorDisplay = GetComponentInChildren<CardsSelectorDisplay>();
        eKey = transform.Find("PlayerUI").Find("KeyE");
        chestPanels = transform.Find("ChestPanels");
        GameManager.Instance.SetCanvas(this);
        weaponDurabilityBar = transform.Find("WeaponDurabilityBar");
        buffDurationBar = transform.Find("BuffDurationBar");
        deckCountText = transform.Find("PlayerUI").Find("CardsDeck").Find("Num").GetComponent<Text>();
        shuffledText = transform.Find("PlayerUI").Find("CardsDeck").Find("ShuffledText").GetComponent<Animator>();
        pauseMenu = transform.Find("PauseMenu");

        GameManager.Instance.UpdateAllCardImages();
    }
    //Durability of weapon
    public void ShowDurabilityBar(bool b)
    {
        weaponDurabilityBar.gameObject.SetActive(b);
    }

    public void SetWDurabilitySliderMax(int i)
    {
        weaponDurabilityBar.GetComponent<Slider>().maxValue = i;
    }

    public void SetWDurabilitySliderValue(int i)
    {
        weaponDurabilityBar.GetComponent<Slider>().value = i;
    }
    //Buff duration
    public void ShowBuffDurationBar(bool b)
    {
        buffDurationBar.gameObject.SetActive(b);
    }

    public void SetBuffDurationSliderMax(float i)
    {
        buffDurationBar.GetComponent<Slider>().maxValue = i;
    }

    public void SetBuffDurationSliderValue(float i)
    {
        buffDurationBar.GetComponent<Slider>().value = i;
    }

    public void UpdateCursor(int i)
    {
        cardsSelectorDisplay.UpdateCursorUI(i);
    }

    public void UpdateCardImage(int i, GameManager.Cards c)
    {
        if(c!=GameManager.Cards.None) cardsSelectorDisplay.UpdateCardImageUI(i,GameManager.Instance.CardsData[c].image);
        else cardsSelectorDisplay.UpdateCardImageUI(i, null);
    }

    public void UpdateDeckCount(int i)
    {
        deckCountText.text = i.ToString();
    }

    public void PlayShuffledTextAnimationUI()
    {
        shuffledText.Play("FadeInOutText", -1, 0.0f);
    }

    public void SetPauseMenuUI(bool b)
    {
        pauseMenu.gameObject.SetActive(b);
    }

    public void ResumeGameUI()
    {
        GameManager.Instance.TogglePauseMenu();
    }

    public void ExitGameUI()
    {
        Application.Quit();
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

    public void ShowChestPanel(Chest.ChestType type, GameManager.Cards newCard, GameManager.Cards deckCard = GameManager.Cards.None)
    {
        switch (type)
        {
            case Chest.ChestType.Reward:
                chestPanels.Find("NewCardPanel").gameObject.SetActive(true);
                //Se muestra la newCard en la imagen
                chestPanels.Find("NewCardPanel").Find("Card").GetComponent<Image>().sprite = GameManager.Instance.CardsData[newCard].image;
                chestPanels.Find("NewCardPanel").Find("CardDescription").GetComponent<Text>().text = GameManager.Instance.CardsData[newCard].description;
                break;

            case Chest.ChestType.Trap:
                chestPanels.Find("DiscardCardPanel").gameObject.SetActive(true);
                //Se muestra la carta descartada deckCard del deck
                chestPanels.Find("DiscardCardPanel").Find("Card").GetComponent<Image>().sprite = GameManager.Instance.CardsData[deckCard].image;
                break;

            case Chest.ChestType.ForcedTrade:
                chestPanels.Find("TradeCardPanel").gameObject.SetActive(true);
                //Se muestra la carta descartada deckCard y la nueva carta newCard
                chestPanels.Find("TradeCardPanel").Find("Card1").GetComponent<Image>().sprite = GameManager.Instance.CardsData[deckCard].image;
                chestPanels.Find("TradeCardPanel").Find("Card2").GetComponent<Image>().sprite = GameManager.Instance.CardsData[newCard].image;
                chestPanels.Find("TradeCardPanel").Find("CardDescription").GetComponent<Text>().text = GameManager.Instance.CardsData[newCard].description;
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
