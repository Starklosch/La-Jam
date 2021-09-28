using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class CardsSelectorDisplay : MonoBehaviour
{
    [SerializeField] GameObject cursorOb;

    RectTransform cursorTransform;

    Image[] childImages;

    void Start()
    {
        //cursorOb = transform.GetChild(3).gameObject;

        cursorTransform = cursorOb.GetComponent<RectTransform>();

        childImages = new Image[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            childImages[i] = transform.GetChild(i).GetComponent<Image>();
        }
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
    public void UpdateCardImageUI(int index, Sprite s)
    {
        if (s != null)
        {
            childImages[index].sprite = s;
            var tempColor = Color.white;
            tempColor.a = 1f;
            childImages[index].color = tempColor;
        }
        else
        {
            childImages[index].sprite = null;
            var tempColor = Color.black;
            tempColor.a = 0.5f;
            childImages[index].color = tempColor;
        }
    }
}
