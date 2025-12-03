using UnityEngine;
using UnityEngine.UI;

// Este componente es el indicador global "Pulsa E". Úsalo en una Canvas (Screen Space - Overlay o Camera).
[AddComponentMenu("Dialogue/InteractionUI")]
public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance { get; private set; }

    [Tooltip("RectTransform del objeto que muestra 'Pulsa E' (puede ser texto o icono).")]
    public RectTransform indicatorRect;

    [Tooltip("Cámara usada para proyectar posiciones (si null usa Camera.main).")]
    public Camera targetCamera;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (indicatorRect != null)
            indicatorRect.gameObject.SetActive(false);

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    /// <summary> Muestra el indicador en la pantalla en la posición proyectada desde worldPos. </summary>
    public void ShowAtWorldPosition(Vector3 worldPos)
    {
        if (indicatorRect == null || targetCamera == null) return;

        Vector3 screenPos = targetCamera.WorldToScreenPoint(worldPos);

        // Si está detrás de la cámara lo ocultamos
        if (screenPos.z <= 0f)
        {
            Hide();
            return;
        }

        indicatorRect.gameObject.SetActive(true);
        indicatorRect.position = screenPos;
    }

    /// <summary> Muestra el indicador en una posición fija (centrado, por ejemplo). </summary>
    public void ShowFixed(Vector2 screenPosition)
    {
        if (indicatorRect == null) return;
        indicatorRect.gameObject.SetActive(true);
        indicatorRect.position = screenPosition;
    }

    public void Hide()
    {
        if (indicatorRect == null) return;
        indicatorRect.gameObject.SetActive(false);
    }
}
