using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class AnuncioManager : MonoBehaviour
{
    public static AnuncioManager Instance;

    public GameObject anuncioUI;
    public TextMeshProUGUI anuncioText;

    public float fadeDuration = 0.5f;
    public float tiempoVisible = 3f;

    private Coroutine anuncioCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        anuncioCoroutine = null;
        RebindUI();
    }

    private void RebindUI()
    {
        anuncioCoroutine = null;

        anuncioUI = GameObject.FindWithTag("AnuncioUI");

        if (anuncioUI == null)
            return;

        anuncioText = anuncioUI.GetComponentInChildren<TextMeshProUGUI>(true);
        anuncioUI.SetActive(false);
    }

    public void MostrarAnuncio(string mensaje)
    {
        if (!anuncioUI || !anuncioText)
            return;

        if (anuncioCoroutine != null)
            StopCoroutine(anuncioCoroutine);

        anuncioCoroutine = StartCoroutine(MostrarAnuncioCoroutine(mensaje));
    }

    private IEnumerator MostrarAnuncioCoroutine(string mensaje)
    {
        if (!anuncioText || !anuncioUI)
            yield break;

        anuncioText.text = mensaje;
        anuncioUI.SetActive(true);

        yield return FadeText(0f, 1f, fadeDuration);
        yield return new WaitForSeconds(tiempoVisible);
        yield return FadeText(1f, 0f, fadeDuration);

        if (anuncioUI)
            anuncioUI.SetActive(false);
    }

    private IEnumerator FadeText(float start, float end, float duration)
    {
        float elapsed = 0f;
        Color c = anuncioText.color;

        while (elapsed < duration)
        {
            if (!anuncioText)
                yield break;

            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(start, end, elapsed / duration);
            anuncioText.color = c;
            yield return null;
        }

        c.a = end;
        anuncioText.color = c;
    }
}
