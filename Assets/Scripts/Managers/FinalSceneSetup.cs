using UnityEngine;

public class FinalSceneSetup : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.canMove = true;

        CameraMovement cm = FindAnyObjectByType<CameraMovement>();
        if (cm != null) cm.canMove = true;

        Time.timeScale = 1f;
    }
}