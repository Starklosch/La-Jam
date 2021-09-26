using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Color marco;
    public Color secundario;
    public Color imagen;
    public Color texto;
    public Color esquinas;

    public TextMesh tm;

    Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
        ren.sharedMaterials[0].color = marco;
        ren.sharedMaterials[1].color = imagen;
        ren.sharedMaterials[2].color = secundario;
        ren.sharedMaterials[3].color = esquinas;
        ren.sharedMaterials[4].color = texto;

        tm = gameObject.AddComponent<TextMesh>();
        //tm.text = "Hola";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
