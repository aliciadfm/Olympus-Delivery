using System;
using UnityEngine;

public class InteracionManager : MonoBehaviour
{
    public int index = 0;
    public const int maxIndex = 1;
    public AnuncioManager anuncioManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AumentarIndex()
    {
        index++;
        if(index >= maxIndex)
        {
            anuncioManager.MostrarAnuncio("Has completado la interaccion con todos los objetos");
        }
    }
}
