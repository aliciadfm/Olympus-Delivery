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

    void Start()
    {
        // Se asume que la cámara del jugador tiene el tag MainCamera
        playerCamera = Camera.main.transform;

        if (pressEIndicator != null)
            pressEIndicator.SetActive(false);
    }



    void Update()
    {
        if (playerCamera == null) return;

        bool canInteract = IsPlayerLookingAtNPC() && !DialogueManager.Instance.IsDialogueActive;

        if (canInteract)
        {
            if (!pressEIndicator.activeSelf)
                pressEIndicator.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
                StartDialogue();
        }
        else
        {
            if (pressEIndicator.activeSelf)
                pressEIndicator.SetActive(false);
        }
    }

    private bool IsPlayerLookingAtNPC()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            return hit.transform == transform;
        }

        return false;
    }

    private void StartDialogue()
    {
        if (dialogueSequence.Length == 0) {
            if (headLookAtPlayer != null) 
            {
                headLookAtPlayer.EndDialogue();
            }
            return;
        }

        DialogueData currentDialogue = dialogueSequence[dialogueIndex];
        DialogueManager.Instance.StartDialogue(currentDialogue);

        if (headLookAtPlayer != null) 
        {
            headLookAtPlayer.StartDialogue();
        }

        dialogueIndex++;

        if (dialogueIndex >= dialogueSequence.Length)
            dialogueIndex = dialogueSequence.Length - 1;
    }
}
