using UnityEngine;

public class EnfocarMinijuego : MonoBehaviour
{
    [Header("Posición de la cámara en Thunder")]
    public Vector3 posicionFija = new Vector3(0, 5, -10);
    public Vector3 rotacionFija = new Vector3(0, 0, 0);

    void Start()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = posicionFija;
            cam.transform.eulerAngles = rotacionFija;
            
            CameraMovement cm = cam.GetComponent<CameraMovement>();
            if (cm != null) cm.canMove = false;
        }
    }
}
