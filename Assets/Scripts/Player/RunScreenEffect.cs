using UnityEngine;

public class RunScreenEffect : MonoBehaviour
{
    public Camera playerCamera;

    [Header("FOV")]
    public float normalFOV = 60f;
    public float runFOV = 75f;
    public float fovLerpSpeed = 5f;

    [Header("Tilt")]
    public float runTilt = 3f;
    public float tiltLerpSpeed = 5f;

    bool isRunning;

    void Start()
    {
        if (!playerCamera)
            playerCamera = Camera.main;

        if (playerCamera)
            playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);

        float targetFOV = isRunning ? runFOV : normalFOV;
        float targetTilt = isRunning ? runTilt : 0f;

        if (playerCamera)
        {
            playerCamera.fieldOfView =
                Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovLerpSpeed);

            Vector3 euler = playerCamera.transform.localEulerAngles;
            float currentTilt = euler.z;
            float newTilt = Mathf.LerpAngle(currentTilt, targetTilt, Time.deltaTime * tiltLerpSpeed);
            playerCamera.transform.localEulerAngles = new Vector3(euler.x, euler.y, newTilt);
        }
    }
}
