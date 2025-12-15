using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMiraManager : MonoBehaviour
{
    public static UIMiraManager Instance;

    [Header("Busca la mira en cada escena")]
    [SerializeField] private string crosshairTag = "CrosshairUi";

    [SerializeField] private GameObject crosshairRoot;
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
        RebindCrosshair();
    }

    private void RebindCrosshair()
    {
        // Si la mira existe en cada escena como un objeto nuevo:
        var go = GameObject.FindWithTag(crosshairTag);
        if (go != null) crosshairRoot = go;

        // Estado por defecto al cargar escena (ajusta a tu gusto):
        if (crosshairRoot != null) crosshairRoot.SetActive(true);
    }

    public void SetVisible(bool visible)
    {
        if (crosshairRoot != null)
            crosshairRoot.SetActive(visible);
    }
}