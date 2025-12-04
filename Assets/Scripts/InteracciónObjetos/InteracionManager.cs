using System;
using UnityEngine;

public class InteracionManager : MonoBehaviour
{
    public int index = 0;
    public int maxIndex = 1;
    public AnuncioManager anuncioManager;
    public string anuncio = "";
    private string anuncio2 = " Basura recogida: ";
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
        if (index == maxIndex)
        {
            anuncioManager.MostrarAnuncio(anuncio);
        }
        else
        {
            anuncioManager.MostrarAnuncio(anuncio2 + index + " de " + maxIndex);
        }
    }
}