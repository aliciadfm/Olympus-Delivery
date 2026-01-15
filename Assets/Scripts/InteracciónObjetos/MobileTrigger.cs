using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MobileTrigger : MonoBehaviour
{
    public string escenaTinder = "Thunder";
    public GameObject textoPrompt;
    private bool jugadorCerca = false;

    void Start()
    {
        if (textoPrompt != null) textoPrompt.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (textoPrompt != null) textoPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (textoPrompt != null) textoPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            Teletransportar();
        }
    }

    void Teletransportar()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(escenaTinder);
    }
}