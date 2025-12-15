using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public GameObject pauseCanvas;
    public string mainMenuScene = "MenuPrincipal";
    public MonoBehaviour[] scriptsToDisable;

    [SerializeField] private UIMiraManager miraManager;

    bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);
        if (miraManager != null)
            miraManager.SetVisible(false);

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
        if (miraManager != null)
            miraManager.SetVisible(true);

        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            Destroy(player);

        SceneManager.LoadScene(mainMenuScene);
    }
}