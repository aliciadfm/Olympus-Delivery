using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Conjunto de diálogos en orden")]
    public DialogueData[] dialogueSequence;
    private int dialogueIndex = 0;

    [Header("Detección mediante Raycast")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer; // para filtrar solo NPCs si quieres

    [Header("UI de interacción")]
    public GameObject pressEIndicator;

    [Header("Cabeza del NPC")]
    public HeadLookAtPlayer headLookAtPlayer;

    private Transform playerCamera;

    // Nueva bandera para habilitar/deshabilitar interacción sin desactivar el GameObject
    private bool interactionEnabled = true;

    void Start()
    {
        // Se asume que la cámara del jugador tiene el tag MainCamera
        playerCamera = Camera.main?.transform;

        if (pressEIndicator != null)
            pressEIndicator.SetActive(false);
    }

    void Update()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main?.transform;
            if (playerCamera == null) return;
        }

        // Protect against DialogueManager being missing in scene
        bool dialogueSystemAvailable = DialogueManager.Instance != null;

        bool canInteract = interactionEnabled && IsPlayerLookingAtNPC() && dialogueSystemAvailable && !DialogueManager.Instance.IsDialogueActive;

        if (canInteract)
        {
            if (pressEIndicator != null && !pressEIndicator.activeSelf)
                pressEIndicator.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
                StartDialogue();
        }
        else
        {
            if (pressEIndicator != null && pressEIndicator.activeSelf)
                pressEIndicator.SetActive(false);
        }
    }

    private bool IsPlayerLookingAtNPC()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            // The raycast might hit a child collider (e.g. a head or body collider), so check IsChildOf
            Transform hitTransform = hit.collider != null ? hit.collider.transform : hit.transform;

            if (hitTransform == transform || hitTransform.IsChildOf(transform))
                return true;
        }

        return false;
    }

    private void StartDialogue()
    {
        if (dialogueSequence == null || dialogueSequence.Length == 0) {
            if (headLookAtPlayer != null)
            {
                headLookAtPlayer.EndDialogue();
            }
            return;
        }

        DialogueData currentDialogue = dialogueSequence[dialogueIndex];
        // Pass this specific NPC gameObject so DialogueManager knows which NPC started the dialogue
        DialogueManager.Instance.StartDialogue(currentDialogue, gameObject);

        if (headLookAtPlayer != null)
        {
            headLookAtPlayer.StartDialogue();
        }

        dialogueIndex++;

        if (dialogueIndex >= dialogueSequence.Length)
            dialogueIndex = dialogueSequence.Length - 1;
    }

    // Método público para activar/desactivar la interacción sin desactivar el GameObject
    public void SetInteractionEnabled(bool enabled)
    {
        interactionEnabled = enabled;
        if (!interactionEnabled && pressEIndicator != null && pressEIndicator.activeSelf)
            pressEIndicator.SetActive(false);
    }
}