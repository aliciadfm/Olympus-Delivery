using System;
using UnityEngine;
public class InteracionManager : MonoBehaviour
{
    public int indiceBasura = 0;
    private int numBasuraTotal = 8;
    private int indiceAntorcas= 0;
    private int numAntorchasTotal = 6;
    public bool todasBasuraRecogida = false;
    public bool todasAntorchasEncendidas = false;
    public AnuncioManager anuncioManager;
    public GameObject portal;
    public GameObject zeus1;
    public GameObject zeus2;

    public void AumentarIndex()
    {
        indiceBasura++;
        if (indiceBasura == numBasuraTotal)
        {
            todasBasuraRecogida = true;
            anuncioManager.MostrarAnuncio("¡Has recogido toda la basura! Ahora debes tirarla en la papelera.");
        }
        else if (indiceBasura < numBasuraTotal)
        {
            anuncioManager.MostrarAnuncio(" Basura recogida: " + indiceBasura + " de " + numBasuraTotal);
        }
    }

    public void AumentarDoubleIndex()
    {
        indiceAntorcas++;
        if (indiceAntorcas < numAntorchasTotal)
        {
            anuncioManager.MostrarAnuncio("Antorchas encendidas: " + indiceAntorcas + " de " + numAntorchasTotal);
        } else if (indiceAntorcas == numAntorchasTotal)
        {
            portal.SetActive(true);
            todasAntorchasEncendidas = true;
            anuncioManager.MostrarAnuncio("Todas las antorchas han sido encendidas. El portal ha sido activado.");
        } 
    }

    public void BasuraRecogida()
    {
        anuncioManager.MostrarAnuncio("Has completado la misión de Zeus, puedes volver a hablar con él.");
        zeus1.SetActive(false);
        zeus2.SetActive(true);
    }
}