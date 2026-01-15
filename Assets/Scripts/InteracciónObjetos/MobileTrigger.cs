using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileTrigger : MonoBehaviour
{
    public string escenaTinder = "Thunder";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm == null) pm = other.GetComponentInParent<PlayerMovement>();
            
            CameraMovement cm = FindAnyObjectByType<CameraMovement>();

            if (pm != null) pm.canMove = false;
            if (cm != null) cm.canMove = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("Jugador fijado. Cargando minijuego...");

            SceneManager.LoadScene(escenaTinder);
        }
    }
}