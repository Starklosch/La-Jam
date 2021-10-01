using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    [SerializeField] ScriptableCard[] cardsScriptables;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        if (cardsScriptables.Length <= 0)
            cardsScriptables = Resources.FindObjectsOfTypeAll<ScriptableCard>();

        cardsData = new Dictionary<Cards, ScriptableCard>();

        Debug.Log(cardsScriptables.Length);
        foreach (var item in cardsScriptables)
        {
            cardsData[item.id] = item;
        }
    }

    public void SceneSwitch(string name)
    {
        SceneManager.LoadScene(name);
    }

    bool isPaused = false;
    public bool isGamePaused()
    {
        return isPaused;
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        SetTimeRunning(isPaused);
        UIManagerInstance.SetPauseMenuUI(isPaused);
    }

    public void SetTimeRunning(bool a)
    {
        Time.timeScale = (a) ? 0 : 1;
        Cursor.lockState = a ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = a;
    }

    UIManager UIManagerInstance;
    PlayerController playerInstance;

    public void SetCanvas(UIManager mI)
    {
        UIManagerInstance = mI;
    }

    public UIManager GetCanvas()
    {
        return UIManagerInstance;
    }
    public void SetPlayer(PlayerController pGO)
    {
        playerInstance = pGO;
    }

    Queue<Cards> deck = new Queue<Cards>();
    List<Cards> discarded = new List<Cards>();
    Cards[] hand = new Cards[3];
    int cursorIndex = 0;

    //public int CursorIndex
    //{
    //    get => cursorIndex;
    //    set
    //    {
    //        if (cursorIndex != value)
    //            CursorIndexChanged?.Invoke(value);

    //        cursorIndex = value;
    //    }
    //}

    public delegate void IntHandler(int number);

    public event IntHandler CursorIndexChanged;

    Dictionary<Cards, ScriptableCard> cardsData = null;

    public IReadOnlyDictionary<Cards, ScriptableCard> CardsData
    {
        get
        {
            return cardsData;
        }
    }

    public enum CardType
    {
        Undefined,
        Weapon,
        Spell,
        Support
    }

    public enum Cards
    {
        //Melee weapons
        Sword,
        Axe,
        Hammer,
        //Spells
        Poison,
        Stun,
        FireBall,
        //Support
        Speed,
        Jump,
        Heal,

        None
    }

    private void Start()
    {
        hand[0] = Cards.Sword;
        hand[1] = Cards.Hammer;
        hand[2] = Cards.Speed;

        UpdateAllCardImages();
        CursorIndexChanged?.Invoke(cursorIndex);
    }

    Cards chestCardHolding = Cards.None;
    Weapons weaponsComponent;
    Spells spellsComponent;
    Buffs buffsComponent;

    Weapons WeaponsComponent
    {
        get
        {
            if (weaponsComponent == null)
                weaponsComponent = playerInstance.GetComponent<Weapons>();

            return weaponsComponent;
        }
    }

    Spells SpellsComponent
    {
        get
        {
            if (spellsComponent == null)
                spellsComponent = playerInstance.GetComponent<Spells>();

            return spellsComponent;
        }
    }

    Buffs BuffsComponent
    {
        get
        {
            if (buffsComponent == null)
                buffsComponent = playerInstance.GetComponent<Buffs>();

            return buffsComponent;
        }
    }

    public PlayerController PlayerInstance { get => playerInstance; }

    public void AcceptChest()
    {
        if(chestCardHolding != Cards.None)
        {
            AddToDeck(chestCardHolding);
            chestCardHolding = Cards.None;
        }
        SetTimeRunning(false);
    }

    public void DiscardChest()
    {
        chestCardHolding = Cards.None;
        SetTimeRunning(false);
    }

    public void OpenChest()
    {
        SetTimeRunning(true);
        //Bad luck prevention
        if (deck.Count < 1)
        {
            chestCardHolding = (Cards)Random.Range(0, (int)Cards.None);
            UIManagerInstance.ShowChestPanel(0, chestCardHolding);
        }
        else
        {
            Chest.ChestType rndChest = (Chest.ChestType)Random.Range(0, (int)Chest.ChestType.None);
            switch (rndChest)
            {
                case Chest.ChestType.Reward:
                    chestCardHolding = (Cards)Random.Range(0, (int)Cards.None);
                    UIManagerInstance.ShowChestPanel(rndChest, chestCardHolding);
                    break;
                case Chest.ChestType.Trap:
                    UIManagerInstance.ShowChestPanel(rndChest, Cards.None, deck.Dequeue());
                    break;
                case Chest.ChestType.ForcedTrade:
                    chestCardHolding = (Cards)Random.Range(0, (int)Cards.None);
                    UIManagerInstance.ShowChestPanel(rndChest, chestCardHolding, deck.Dequeue());
                    break;
            }
        }
    }

    public void AddToDeck(Cards card)
    {
        deck.Enqueue(card);
        int getEmptyIndex = AnyEmptySlot();
        if(getEmptyIndex != -1)
        {
            hand[getEmptyIndex] = RemoveFromDeck();
            UIManagerInstance.UpdateCardImage(getEmptyIndex, hand[getEmptyIndex]);
        }
        UIManagerInstance.UpdateDeckCount(deck.Count);
    }

    public Cards RemoveFromDeck()
    {
        if (deck.Count > 0)
        {
            UIManagerInstance.UpdateDeckCount(deck.Count-1);
            return deck.Dequeue();
        }
        else return Cards.None;
    }

    bool EmptyHand()
    {
        return (hand[0]==Cards.None && hand[1] == Cards.None && hand[2] == Cards.None);
    }

    int AnyEmptySlot()
    {
        int i = 0;
        while (i < hand.Length)
        {
            if (hand[i] == Cards.None)
            {
                return i;
            }
            i++;
        }
        return -1;
    }

    void ShuffleDiscarded()
    {
        UIManagerInstance.PlayShuffledTextAnimationUI();
        for (int i = 0; i < discarded.Count; i++)
        {
            Cards temp = discarded[i];
            int randomIndex = Random.Range(i, discarded.Count);
            discarded[i] = discarded[randomIndex];
            discarded[randomIndex] = temp;
        }
    }

    void MoveDiscardedToDeck()
    {
        foreach (var c in discarded)
        {
            AddToDeck(c);
        }
        discarded.Clear();
    }

    public Cards GetCardSelected()
    {
        return hand[cursorIndex];
    }

    public void UseCard()
    {
        if (GetCardSelected() == Cards.None) return;
        switch (hand[cursorIndex])
        {
            case Cards.Sword:
                //Las de tipo arma instanciaran/activaran un objeto desde GameManager y se comunicaran con un componente Weapon del jugador para indicar que tiene x arma.
                SpellsComponent.DisableSpell();
                WeaponsComponent.EnableWeapon(Cards.Sword);
                break;
            case Cards.Axe:
                SpellsComponent.DisableSpell();
                WeaponsComponent.EnableWeapon(Cards.Axe);
                break;
            case Cards.Hammer:
                SpellsComponent.DisableSpell();
                WeaponsComponent.EnableWeapon(Cards.Hammer);
                break;

            case Cards.Poison:
                //Las de tipo hechizo se comunican con un componente ThrowSpell del jugador, para lanzar la carta y realizar el efecto.
                WeaponsComponent.DisableWeapon();
                SpellsComponent.ActivateSpell(Cards.Poison);
                break;
            case Cards.Stun:
                WeaponsComponent.DisableWeapon();
                SpellsComponent.ActivateSpell(Cards.Stun);
                break;
            case Cards.FireBall:
                WeaponsComponent.DisableWeapon();
                SpellsComponent.ActivateSpell(Cards.FireBall);
                //SpellsComponent.FireBall(playerInstance.GetComponent<PlayerController>());
                break;

            case Cards.Speed:
                //Las de tipo soporte llaman a metodos del componente Support del jugador para aplicar sus efectos.
                BuffsComponent.ActivateBuff(Cards.Speed);
                break;
            case Cards.Jump:
                BuffsComponent.ActivateBuff(Cards.Jump);
                break;
            case Cards.Heal:
                BuffsComponent.ActivateBuff(Cards.Heal);
                break;
        }

        discarded.Add(hand[cursorIndex]);
        hand[cursorIndex] = Cards.None;
        UIManagerInstance.UpdateCardImage(cursorIndex, hand[cursorIndex]);
        //CD de uso empieza
        //Animacion de descarte

        if (deck.Count == 0 && EmptyHand())
        {
            //Animacion de barajar y mover a deck

            ShuffleDiscarded();
            MoveDiscardedToDeck();

            UpdateAllCardImages();
        }
        else if (deck.Count>0)
        {
            //Animacion de robar si hay cartas en deck
            hand[cursorIndex] = RemoveFromDeck();
            UIManagerInstance.UpdateCardImage(cursorIndex, hand[cursorIndex]);
        }

        CursorIndexChanged?.Invoke(cursorIndex);
    }

    public void UpdateAllCardImages() 
    {
        for (int i = 0; i < 3; i++)
        {
            UIManagerInstance.UpdateCardImage(i, hand[i]);
        }
    }

    public void MoveCursor(int m)
    {
        cursorIndex -= m;
        if (cursorIndex < 0) cursorIndex = 2;
        else if (cursorIndex > 2) cursorIndex = 0;

        UIManagerInstance.UpdateCursor(cursorIndex);

        CursorIndexChanged?.Invoke(cursorIndex);
    }

    public void SetCursor(int i)
    {
        cursorIndex = i;
        UIManagerInstance.UpdateCursor(cursorIndex);

        CursorIndexChanged?.Invoke(cursorIndex);
    }

    public void ShowEKey()
    {
        if (!UIManagerInstance) return;

        UIManagerInstance.ShowE();
    }
    public void HideEKey()
    {
        if (!UIManagerInstance) return;
        UIManagerInstance.HideE();
    }

    public bool IsEKeyActive()
    {
        if (!UIManagerInstance) return false;
        return UIManagerInstance.IsEActive();
    }
}
