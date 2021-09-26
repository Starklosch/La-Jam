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
    [SerializeField] Vector2Int renderTextureSize = new Vector2Int(4096, 4096);

    [SerializeField] Font font;
    [SerializeField] int fontSize;
    [SerializeField] FontStyle fontStyle;

    [SerializeField] LayerMask cullingMask;

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

    public Vector2Int TextureSize
    {
        get => renderTextureSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 0 1 2
        // 3 4 5
        // 6 7 8
        if (cullingMask == 0)
            cullingMask = LayerMask.GetMask("Text");

        if (gameObject.TryGetComponent(out AudioListener audio))
            Destroy(audio);

        if (cam == null)
            cam = gameObject.AddComponent<Camera>();

        if (renderTexture == null)
            renderTexture = new RenderTexture(renderTextureSize.x, renderTextureSize.y, 0);
        else
            renderTextureSize = new Vector2Int(renderTextureSize.x, renderTextureSize.y);

        //Debug.Log("Start");

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.targetTexture = renderTexture;
        cam.orthographic = true;
        cam.cullingMask = cullingMask;

        Invalidate();
    }

    public void Invalidate()
    {
        // Set up
        cam.orthographicSize = size;
        textMesh.text = text;
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

    public Texture2D GetTexture2D()
    {
        cam.Render();

        RenderTexture.active = renderTexture;

        var tex = new Texture2D(renderTextureSize.x, renderTextureSize.y);
        tex.ReadPixels(new Rect(0, 0, renderTextureSize.x, renderTextureSize.y), 0, 0);
        tex.Apply();

        return tex;
    }

    public void StoreToTexture2D(Texture2D tex)
    {
        cam.Render();

        RenderTexture.active = renderTexture;
        //Debug.Log("Store");

        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();
    }
}
