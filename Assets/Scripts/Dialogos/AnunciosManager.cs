using UnityEngine;
using TMPro;
using System.Collections;

public class AnuncioManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject anuncioUI; 
    public TextMeshProUGUI anuncioText;

    [Header("Settings")]
    public float fadeDuration = 0.5f;
    public float tiempoVisible = 3f;

    private Coroutine anuncioCoroutine;

    private void Awake()
    {
        anuncioUI.SetActive(false);
    }

    public void MostrarAnuncio(string mensaje)
    {
        if (anuncioCoroutine != null)
            StopCoroutine(anuncioCoroutine);

        anuncioCoroutine = StartCoroutine(MostrarAnuncioCoroutine(mensaje));
    }

    private IEnumerator MostrarAnuncioCoroutine(string mensaje)
    {
        anuncioText.text = mensaje;
        anuncioUI.SetActive(true);

        yield return StartCoroutine(FadeText(0f, 1f, fadeDuration));

        yield return new WaitForSeconds(tiempoVisible);

        yield return StartCoroutine(FadeText(1f, 0f, fadeDuration));

        anuncioUI.SetActive(false);
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = anuncioText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            anuncioText.color = color;
            yield return null;
        }

        color.a = endAlpha;
        anuncioText.color = color;
    }
}
