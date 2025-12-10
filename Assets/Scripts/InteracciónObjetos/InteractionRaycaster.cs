using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InteractionRaycaster : MonoBehaviour
{
    public float distancia = 3f;

    private InteracionManager interacionManager;
    private Camera camara;
    private TMP_Text indicatorText;
    private GameObject objetoActual;

    private const string preferredIndicatorName = "TextInteracturar";

    void Start()
    {
        camara = Camera.main;
        interacionManager = FindAnyObjectByType<InteracionManager>();
        FindIndicatorText();
    }

    void Update()
    {
        RaycastHit hit;
        objetoActual = null;

        if (Physics.Raycast(camara.transform.position, camara.transform.forward, out hit, distancia))
        {
            if (hit.collider.CompareTag("interactuable"))
            {
                objetoActual = hit.collider.gameObject;
                indicatorText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    objetoActual.SetActive(false);
                    indicatorText.gameObject.SetActive(false);
                    interacionManager.AumentarIndex();
                }
                return;
            }
            if (hit.collider.CompareTag("antorcha"))
            {
                objetoActual = hit.collider.gameObject;
                indicatorText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Transform parent = objetoActual.transform.parent;
                    GameObject pCube361 = parent.Find("pCube361").gameObject;

                    if (pCube361.activeSelf == true)
                    {
                        indicatorText.gameObject.SetActive(false);
                        return;
                    }

                    pCube361.SetActive(true);
                    objetoActual.tag = "Untagged";
                    indicatorText.gameObject.SetActive(false);
                    interacionManager.AumentarDoubleIndex();
                }
                return;
            }
            if (hit.collider.CompareTag("basura") && interacionManager.todasBasuraRecogida)
            {
                objetoActual = hit.collider.gameObject;
                indicatorText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Transform parent = objetoActual.transform.parent;
                    parent.Find("Trash box").gameObject.tag = "Untagged";
                    parent.Find("Trash cap").gameObject.tag = "Untagged";
                    indicatorText.gameObject.SetActive(false);
                    interacionManager.BasuraRecogida();
                    objetoActual.tag = "Untagged";
                }
                return;
            }
        }

        if (indicatorText != null)
            indicatorText.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        indicatorText = null;
        FindIndicatorText();
    }

    private void FindIndicatorText()
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);

        if (allTexts == null || allTexts.Length == 0)
            return;

        foreach (var t in allTexts)
        {
            if (string.Equals(t.gameObject.name, preferredIndicatorName,
                System.StringComparison.OrdinalIgnoreCase))
            {
                indicatorText = t;
                break;
            }
        }

        if (indicatorText == null)
        {
            foreach (var t in allTexts)
            {
                string nameLower = t.gameObject.name.ToLower();
                if (nameLower.Contains("textinteract"))
                {
                    indicatorText = t;
                    break;
                }
            }
        }

        if (indicatorText == null)
            indicatorText = allTexts[0];

        if (indicatorText != null)
            indicatorText.gameObject.SetActive(false);
    }
}