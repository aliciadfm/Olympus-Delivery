using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Dialogue/DialogueTriggerPro")]
public class DialogueTriggerPro : MonoBehaviour
{
    [Header("Secuencia a reproducir")]
    public DialogueSequence sequence;

    [Header("Detección")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer = ~0; // por defecto todo
    public bool useRaycastLook = true; // si false usa proximidad simple

    [Header("Opcional")]
    public bool autoStartOnEnter = false; // iniciar diálogo al entrar en rango
    public bool showIndicator = true;

    [Header("Bindings (si quieres asignar manualmente)")]
    [Tooltip("Si dejas vacío, el trigger mapeará automáticamente DialogueActor en la escena por actorId")]
    public DialogueActor[] manualBindings;

    private Transform playerCamera;
    private DialogueActor[] localActors; // actores en children (útil en NPCs compound)

    void Start()
    {
        if (Camera.main != null)
            playerCamera = Camera.main.transform;

        // recopilamos actores hijos (por si el NPC tiene varios)
        localActors = GetComponentsInChildren<DialogueActor>(true);
    }

    void Update()
    {
        if (playerCamera == null) return;

        bool inRange = false;
        Vector3 indicatorPos = transform.position;

        if (useRaycastLook)
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
            {
                DialogueTriggerPro hitTrigger = hit.transform.GetComponentInParent<DialogueTriggerPro>();
                if (hitTrigger == this)
                {
                    inRange = true;
                    // posicion del indicador: si hay actor local toma su offset
                    var actor = FirstLocalActor();
                    indicatorPos = actor != null ? actor.transform.position + actor.indicatorWorldOffset : transform.position + Vector3.up * 2f;
                }
            }
        }
        else
        {
            // proximidad esférica
            var camPos = playerCamera.position;
            if (Vector3.Distance(camPos, transform.position) <= interactionDistance)
            {
                inRange = true;
                var actor = FirstLocalActor();
                indicatorPos = actor != null ? actor.transform.position + actor.indicatorWorldOffset : transform.position + Vector3.up * 2f;
            }
        }

        // Manejo del indicador global
        if (showIndicator && InteractionUI.Instance != null)
        {
            if (inRange && !DialogueManagerPro.Instance.IsDialogueActive)
                InteractionUI.Instance.ShowAtWorldPosition(indicatorPos);
            else
                InteractionUI.Instance.Hide();
        }

        // Auto-start (opcional)
        if (inRange && autoStartOnEnter && !DialogueManagerPro.Instance.IsDialogueActive)
            TriggerDialogue();
        else if (inRange && Input.GetKeyDown(KeyCode.E) && !DialogueManagerPro.Instance.IsDialogueActive)
            TriggerDialogue();
    }

    private DialogueActor FirstLocalActor()
    {
        if (localActors != null && localActors.Length > 0) return localActors[0];
        return null;
    }

    private void TriggerDialogue()
    {
        if (sequence == null) return;

        // Construir bindings: si manualBindings no vacío, usarlo; si no, buscar todos los DialogueActor en escena.
        Dictionary<string, DialogueActor> map = new Dictionary<string, DialogueActor>();

        if (manualBindings != null && manualBindings.Length > 0)
        {
            foreach (var a in manualBindings)
            {
                if (a == null) continue;
                if (string.IsNullOrEmpty(a.actorId)) continue;
                if (!map.ContainsKey(a.actorId))
                    map.Add(a.actorId, a);
            }
        }
        else
        {
            // intentamos mapear actores presentes en la escena por actorId
            DialogueActor[] all = FindObjectsOfType<DialogueActor>();
            foreach (var a in all)
            {
                if (a == null) continue;
                if (string.IsNullOrEmpty(a.actorId)) continue;
                if (!map.ContainsKey(a.actorId))
                    map.Add(a.actorId, a);
            }
        }

        // Llamamos al manager central
        if (DialogueManagerPro.Instance != null)
            DialogueManagerPro.Instance.StartDialogue(sequence, map);

        // Oculta indicador al iniciar
        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();
    }
}
