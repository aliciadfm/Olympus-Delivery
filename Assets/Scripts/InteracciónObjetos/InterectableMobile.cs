using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableMobile : MonoBehaviour
{
    public string escenaTinder = "Thunder";

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.SetActive(false); 
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        SceneManager.LoadScene(escenaTinder);
    }
}
