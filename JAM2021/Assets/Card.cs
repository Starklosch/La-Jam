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

    public Material m_marco;
    public Material m_secundario;
    public Material m_imagen;
    public Material m_texto;
    public Material m_esquinas;

    public TextMesh tm;

    Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
        ren.sharedMaterials[0] = m_marco;
        ren.sharedMaterials[1] = m_imagen;
        ren.sharedMaterials[2] = m_secundario;
        ren.sharedMaterials[3] = m_esquinas;
        ren.sharedMaterials[4] = m_texto;

        tm = gameObject.AddComponent<TextMesh>();
        //tm.text = "Hola";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
