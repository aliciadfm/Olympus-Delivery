using System;
using UnityEngine;

public class InteracionManager : MonoBehaviour
{
    public int index = 0;
    private int maxIndex = 8;

    private int doubleIndex= 0;
    private int doubleMaxIndex = 6;
    public AnuncioManager anuncioManager;
    public GameObject portal;
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
        else if (index < maxIndex)
        {
            anuncioManager.MostrarAnuncio(anuncio2 + index + "  de " + maxIndex);
        }
    }

    public void AumentarDoubleIndex()
    {
        doubleIndex++;
        if (doubleIndex < doubleMaxIndex)
        {
            anuncioManager.MostrarAnuncio("Antorchas enencdidads: " + doubleIndex + " de " + doubleMaxIndex);
        } else if (doubleIndex == doubleMaxIndex)
        {
            portal.SetActive(true);
            anuncioManager.MostrarAnuncio("Todas las antorchas han sido encendidas. El portal ha sido activado.");
        } 
    }
}