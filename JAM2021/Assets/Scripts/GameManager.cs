using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


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

    public void SetPause(bool a)
    {
        Time.timeScale = (a) ? 0 : 1;
        Cursor.lockState = a ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = a;
    }

    UIManager UIManagerInstance;
    GameObject playerInstance;

    public void SetCanvas(UIManager mI)
    {
        UIManagerInstance = mI;
    }

    public UIManager GetCanvas()
    {
        return UIManagerInstance;
    }
    public void SetPlayer(GameObject pGO)
    {
        playerInstance = pGO;
    }

    Queue<Cards> deck = new Queue<Cards>();
    List<Cards> discarded = new List<Cards>();
    Cards[] hand = new Cards[3];
    int cursorIndex = 0;

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
        hand[1] = Cards.Poison;
        hand[2] = Cards.Speed;
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

    public void AcceptChest()
    {
        if(chestCardHolding != Cards.None)
        {
            deck.Enqueue(chestCardHolding);
            chestCardHolding = Cards.None;
        }
        SetPause(false);
    }

    public void DiscardChest()
    {
        chestCardHolding = Cards.None;
        SetPause(false);
    }

    public void OpenChest()
    {
        SetPause(true);
        //Bad luck prevention
        if (deck.Count < 1)
        {
            chestCardHolding = (Cards)Random.Range(0, (int)Cards.None);
            UIManagerInstance.ShowChestPanel(0, chestCardHolding);
        }
        else
        {
            int randomChest = Random.Range(0, (int)Chest.ChestType.None);
            switch ((Chest.ChestType)randomChest)
            {
                case Chest.ChestType.Reward:
                    chestCardHolding = (Cards)Random.Range(0, (int)Cards.None);
                    UIManagerInstance.ShowChestPanel(0, chestCardHolding);
                    break;
                case Chest.ChestType.Trap:
                    UIManagerInstance.ShowChestPanel(0, Cards.None, deck.Dequeue());
                    break;
                case Chest.ChestType.ForcedTrade:
                    chestCardHolding = (Cards)Random.Range(0, (int)Cards.None);
                    UIManagerInstance.ShowChestPanel(0, chestCardHolding, deck.Dequeue());
                    break;
            }
        }
    }

    public void AddToDeck(Cards card)
    {
        deck.Enqueue(card);
    }

    public Cards RemoveFromDeck()
    {
        if (deck.Count > 0)
            return deck.Dequeue();
        else return Cards.None;
    }

    bool EmptyHand()
    {
        return (hand[0]==Cards.None && hand[1] == Cards.None && hand[2] == Cards.None);
    }

    void ShuffleDiscarded()
    {
        //O(n/2)
        for (int i = 0; i < discarded.Count/2; i++)
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
        if (GetCardSelected() == Cards.None /*|| CD*/) return;
        switch (hand[cursorIndex])
        {
            case Cards.Sword:
                //Las de tipo arma instanciaran/activaran un objeto desde GameManager y se comunicaran con un componente Weapon del jugador para indicar que tiene x arma.
                SpellsComponent.DisableSpell();
                WeaponsComponent.ActivateWeapon(Cards.Sword);
                break;
            case Cards.Axe:
                SpellsComponent.DisableSpell();
                WeaponsComponent.ActivateWeapon(Cards.Axe);
                break;
            case Cards.Hammer:
                SpellsComponent.DisableSpell();
                WeaponsComponent.ActivateWeapon(Cards.Hammer);
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
        //CD de uso empieza
        //Animacion de descarte

        if (deck.Count == 0 && EmptyHand())
        {
            //Animacion de barajar y mover a deck

            ShuffleDiscarded();
            MoveDiscardedToDeck();
            //Poner 3 en mano
            for(int i = 0; i < 3; i++)
            {
                hand[i] = RemoveFromDeck();
            }
        }
        else if (deck.Count>0)
        {
            //Animacion de robar si hay cartas en deck
            hand[cursorIndex] = RemoveFromDeck();
        }
    }

    public void MoveCursor(int m)
    {
        cursorIndex -= m;
        if (cursorIndex < 0) cursorIndex = 2;
        else if (cursorIndex > 2) cursorIndex = 0;

        UIManagerInstance.UpdateCursor(cursorIndex);
    }

    public void SetCursor(int i)
    {
        cursorIndex = i;
        UIManagerInstance.UpdateCursor(cursorIndex);
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
