using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderText : MonoBehaviour
{
    [SerializeField] string text;
    [SerializeField] float size = 1;

    [SerializeField] Camera cam;
    [SerializeField] TextMesh textMesh;
    [SerializeField] TextAnchor textPositioning;
    [SerializeField] TextAlignment textAlignment;

    [SerializeField] RenderTexture renderTexture;
    [SerializeField] Font font;
    [SerializeField] int fontSize;
    [SerializeField] FontStyle fontStyle;

    public FontStyle FontStyle
    {
        get => FontStyle;
        set
        {
            FontStyle = value;
        }
    }

    public string Text
    {
        get => text;
        set
        {
            text = value;
            Invalidate();
        }
    }
    public TextAnchor TextPositioning
    {
        get => textPositioning;
        set
        {
            textPositioning = value;
            Invalidate();
        }
    }

    public int FontSize
    {
        get => fontSize;
        set
        {
            fontSize = value;
            Invalidate();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 0 1 2
        // 3 4 5
        // 6 7 8

        Invalidate();
    }

    public void Invalidate()
    {
        // Set up
        cam.orthographicSize = size;
        textMesh.anchor = TextPositioning;
        textMesh.alignment = textAlignment;
        textMesh.font = font;
        textMesh.fontSize = FontSize;
        textMesh.fontStyle = fontStyle;

        // Positioning
        float x = 0, y = 0;

        var row = (int)TextPositioning / 3;
        var column = (int)TextPositioning % 3;

        if (row == 0)
            y = size;
        else if (row == 2)
            y = -size;

        if (column == 0)
            x = -size;
        else if (column == 2)
            x = size;

        textMesh.transform.localPosition = new Vector3(x, y, 2);
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
    }

}
