using UnityEngine;
using UnityEngine.SceneManagement;

public class IrAlMenu : MonoBehaviour
{
    [SerializeField] private string nombreDelMenu = "MainMenu"; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(nombreDelMenu);
        }
    }
}