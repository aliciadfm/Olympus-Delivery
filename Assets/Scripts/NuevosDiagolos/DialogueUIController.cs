using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class DialogueUIController : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_Text nameText;
    public TMP_Text contentText;
    public GameObject continueHint; // "Pulsa ESPACIO..." u otro

    [Header("Opciones")]
    public float fadeDuration = 0.12f;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        HideInstant();
    }

    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(1f));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(0f));
    }

    public void HideInstant()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator Fade(float target)
    {
        float start = canvasGroup.alpha;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = target;
        canvasGroup.interactable = target > 0.5f;
        canvasGroup.blocksRaycasts = target > 0.5f;
    }

    public void SetName(string s)
    {
        if (nameText != null) nameText.text = s;
    }

    public void SetContent(string s)
    {
        if (contentText != null) contentText.text = s;
    }

    public void SetContinueHint(bool visible)
    {
        if (continueHint != null)
            continueHint.SetActive(visible);
    }
}
