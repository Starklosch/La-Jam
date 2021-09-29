using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    //public Material m_imagen;
    //public Material m_texto;

    //public TextMesh tm;
    public RenderText renderText;

    [SerializeField] Texture2D image;
    [SerializeField] Vector2 imageOffset;
    [SerializeField] string text;
    [SerializeField] string textReferenceName;
    [SerializeField] string imageReferenceName;
    [SerializeField] string imageOffsetReferenceName;

    public string Text
    {
        get => text;
        set
        {
            text = value;
            renderText.Text = text;
            renderText.StoreToTexture2D(savedTex);

            ren.sharedMaterials[4].SetTexture(textReference, savedTex);
        }
    }

    public Texture2D Image
    {
        get => image;
        set
        {
            image = value;

            //ren.sharedMaterials[3].mainTexture = image;
            ren.sharedMaterials[3].SetTexture(imageReference, image);
        }
    }

    public Vector2 ImageOffset
    {
        get => imageOffset;
        set
        {
            imageOffset = value;

            ren.sharedMaterials[3].SetVector(imageOffsetReference, imageOffset);
        }
    }

    int textReference = 0;
    int imageOffsetReference = 0;
    int imageReference = 0;

    Renderer ren;
    Texture2D savedTex;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();

        textReference = Shader.PropertyToID(textReferenceName);
        imageOffsetReference = Shader.PropertyToID(imageOffsetReferenceName);
        imageReference = Shader.PropertyToID(imageReferenceName);

        if (renderText == null && (renderText = FindObjectOfType<RenderText>()) == null)
            Debug.LogWarning("Missing RenderText!");

        var size = renderText.TextureSize;
        savedTex = new Texture2D(size.x, size.y);

        Text = text;
        Image = image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
