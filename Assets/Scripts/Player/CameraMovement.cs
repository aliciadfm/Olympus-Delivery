using UnityEngine;

public class CameraMovement : MonoBehaviour
{
     [Header("Referencias")]
    public Transform playerBody;

    [Header("Sensibilidad")]
    public float mouseSensitivity = 180f; // grados/seg

    private float xRotation = 0f;

    public bool canMove = true; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!canMove) return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
