using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class SmoothBlinkText : MonoBehaviour
{
    [Header("Parpadeo")]
    [Tooltip("Tiempo en segundos que tarda un ciclo completo de opacidad (subida y bajada).")]
    public float blinkSpeed = 2f; 

    private TMP_Text tmpText;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        float alpha = (Mathf.Sin(Time.time * Mathf.PI * 2f / Mathf.Max(blinkSpeed, 0.01f)) + 1f) / 2f;

        Color c = tmpText.color;
        c.a = alpha;
        tmpText.color = c;
    }
}