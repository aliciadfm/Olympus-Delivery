using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; //TextMeshPro para mensajes de error

public class LogicaThunder : MonoBehaviour
{
    [System.Serializable]
    public struct PreguntaTinder {
        public Texture foto;
        public bool esCorazonLoCorrecto;
        public string mensajeError;
    }

    public PreguntaTinder[] niveles;
    public MeshRenderer planoFoto;
    public GameObject panelError;
    public TextMeshProUGUI textoError;

    private int indiceActual = 0;
    private bool juegoBloqueado = false;

    void Start() {
        ActualizarInterfaz();
        panelError.SetActive(false);
    }

    public void ValidarDecision(bool eligioCorazon) {
        if (juegoBloqueado) return;

        if (eligioCorazon == niveles[indiceActual].esCorazonLoCorrecto) {
            Avanzar();
        } else {
            MostrarError();
        }
    }

    void Avanzar() {
    	indiceActual++;
    
    	if (indiceActual < niveles.Length) {
        	ActualizarInterfaz();
    	}
    	else {
        	Debug.Log("¡Minijuego superado!");
        	CargarEscenaFinal();
    	}
	}

    void CargarEscenaFinal() {
    	SceneManager.LoadScene("FinJuego");
    }

    void MostrarError() {
        juegoBloqueado = true;
        textoError.text = niveles[indiceActual].mensajeError;
        panelError.SetActive(true);
        Invoke("Reiniciar", 4f);
    }

    void ActualizarInterfaz() {
        planoFoto.material.mainTexture = niveles[indiceActual].foto;
    }

    void Reiniciar() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}