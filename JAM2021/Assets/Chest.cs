using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    bool isOpen_;

    void Start()
    {
        isOpen_ = false;
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
            isOpen_ = true;
            GameManager.Instance.HideEKey();
            GameManager.Instance.OpenChest();
        }
    }

    public bool isOpen()
    {
        return isOpen_;
    }
}
