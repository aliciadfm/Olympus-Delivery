using UnityEngine;

public class LimpiezaEscenaFinal : MonoBehaviour
{
    void Awake() // Usamos Awake para que sea lo primero que ocurra
    {
        // 1. Buscamos al clon que viene de la escena anterior y lo borramos
        GameObject clon = GameObject.Find("Player(Clone)");
        if (clon != null) 
        {
            Destroy(clon);
            Debug.Log("Clon antiguo eliminado con éxito.");
        }

        // 2. Aseguramos que el ratón vuelva a modo primera persona
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        // 3. Reconectamos el DialogueManager con la nueva interfaz
        // Buscamos el Manager que viene por DontDestroyOnLoad
        DialogueManager dm = FindAnyObjectByType<DialogueManager>();
        if (dm != null)
        {
            // Forzamos la búsqueda de la DialogueUI de esta escena
            dm.Invoke("RebindUI", 0.2f); 
        }
    }
}