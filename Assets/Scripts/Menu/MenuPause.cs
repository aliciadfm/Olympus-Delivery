using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public GameObject pauseCanvas;         
    public string mainMenuScene = "MenuPrincipal";

    public MonoBehaviour[] scriptsToDisable;

    bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))  
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

        foreach (var s in scriptsToDisable)
            if (s != null) s.enabled = false;

        Time.timeScale = 0f;                
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        foreach (var s in scriptsToDisable)
            if (s != null) s.enabled = true;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}