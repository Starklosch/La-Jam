using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsSelectorDisplay : MonoBehaviour
{
    GameObject cursorOb;
    void Start()
    {
        cursorOb = transform.GetChild(3).gameObject;
    }

    public void UpdateCursorUI(int index)
    {
        if (cursorOb != null)
        {
            cursorOb.transform.position = new Vector3(transform.GetChild(index).position.x, cursorOb.transform.position.y, 0);
        }
    }
}
