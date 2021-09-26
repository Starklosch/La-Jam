using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    bool isOpen;

    void Start()
    {
        isOpen = false;
    }
    
    public void Open()
    {
        if (isOpen)
        {
            //Mensaje de que ya ha sido abierto o no hacer nada
            return;
        }
        else
        {
            isOpen = true;

        }
    }
}
