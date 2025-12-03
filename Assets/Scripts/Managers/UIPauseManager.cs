using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    public GameObject mainPanel;     
    public GameObject creditsPanel;  

    public void OpenCredits()
    {
        if (mainPanel) mainPanel.SetActive(false);
        if (creditsPanel) creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel) creditsPanel.SetActive(false);
        if (mainPanel) mainPanel.SetActive(true);
    }
}

