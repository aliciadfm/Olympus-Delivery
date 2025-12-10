using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData[] dialogueSequence;
    private int dialogueIndex = 0;
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public GameObject pressEIndicator;
    private Transform playerCamera;
    private bool interactionEnabled = true;

    private void Start()
    {
        playerCamera = Camera.main?.transform;
        if (pressEIndicator) pressEIndicator.SetActive(false);
    }

    private void Update()
    {
        if (!interactionEnabled || DialogueManager.Instance == null) return;
        if (DialogueManager.Instance.IsDialogueActive) return;

        if (IsPlayerLooking())
        {
            if (pressEIndicator) pressEIndicator.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
                StartDialogue();
        }
        else
        {
            if (pressEIndicator) pressEIndicator.SetActive(false);
        }
    }

    private bool IsPlayerLooking()
    {
        if (!playerCamera) return false;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            return hit.transform == transform || hit.transform.IsChildOf(transform);
        }
        return false;
    }

    private void StartDialogue()
    {
        if (dialogueSequence.Length == 0) return;

        DialogueManager.Instance.StartDialogue(dialogueSequence[dialogueIndex], gameObject);

        dialogueIndex = Mathf.Min(dialogueIndex + 1, dialogueSequence.Length - 1);
    }

    public void SetInteractionEnabled(bool enabled)
    {
        interactionEnabled = enabled;
        if (!enabled && pressEIndicator)
            pressEIndicator.SetActive(false);
    }
}