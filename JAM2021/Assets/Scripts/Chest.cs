using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    Animator anim;

    public enum ChestType
    {
        Reward,
        Trap,
        ForcedTrade,
        None
    }

    int animOpen = 0;

    bool isOpen_;

    void Start()
    {
        isOpen_ = false;

        anim = GetComponent<Animator>();
        animOpen = Animator.StringToHash("Open");
    }
    
    public void Open()
    {
        if (isOpen_)
        {
            //Mensaje de que ya ha sido abierto o no hacer nada
            return;
        }
        else
        {
            //Cambiar modelo a cofre abierto
            isOpen_ = true;
            GameManager.Instance.HideEKey();
            GameManager.Instance.OpenChest();

            anim.SetBool(animOpen, true);
        }
    }

    public bool isOpen()
    {
        return isOpen_;
    }

}
