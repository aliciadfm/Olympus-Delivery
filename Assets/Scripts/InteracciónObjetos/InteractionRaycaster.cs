using TMPro;
using UnityEngine;

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

        if (interacionManager == null)
            interacionManager = FindAnyObjectByType<InteracionManager>();

        if (indicatorText == null)
        {
            // Buscamos todos los TMP_Text (incluyendo inactivos)
            TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);

            if (allTexts != null && allTexts.Length > 0)
            {
                // 1) Primero intentamos encontrar por nombre exacto preferido (case-insensitive)
                foreach (var t in allTexts)
                {
                    if (string.Equals(t.gameObject.name, preferredIndicatorName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        indicatorText = t;
                        break;
                    }
                }

                // 2) Si no encontramos por nombre exacto, aplicamos heurística por palabras clave
                if (indicatorText == null)
                {
                    foreach (var t in allTexts)
                    {
                        string nameLower = t.gameObject.name.ToLower();
                        if (nameLower.Contains("textinteract") || nameLower.Contains("textinteracturar"))
                        {
                            indicatorText = t;
                            break;
                        }
                    }
                }

                // 3) Fallback: si sólo hay uno o no hubo coincidencias, tomamos el primero
                if (indicatorText == null)
                    indicatorText = allTexts[0];
            }
        }

        if (indicatorText != null)
            indicatorText.gameObject.SetActive(false);
        else
            Debug.LogWarning("[InteractionRaycaster] No se encontró ningún TMP_Text para usar como indicador. Crea uno llamado '" + preferredIndicatorName + "' o asígnalo manualmente.");
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
        }

        indicatorText.gameObject.SetActive(false);
    }
}