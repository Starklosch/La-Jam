using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsSelectorDisplay : MonoBehaviour
{
    [SerializeField] GameObject cursorOb;

    RectTransform cursorTransform;

    void Start()
    {
        //cursorOb = transform.GetChild(3).gameObject;

        cursorTransform = cursorOb.GetComponent<RectTransform>();
    }

    public void UpdateCursorUI(int index)
    {
        if (cursorOb != null)
        {
            cursorOb.transform.SetParent(transform.GetChild(index));
            cursorTransform.anchoredPosition = Vector3.zero;
            //cursorOb.transform.position = new Vector3(transform.GetChild(index).position.x, cursorOb.transform.position.y, 0);
        }
    }
}
