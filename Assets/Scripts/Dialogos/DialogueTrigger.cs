using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Conjuntos de diálogos en orden")]
    public DialogueData[] dialogueSequence;
    private int dialogueIndex = 0;

    [Header("UI de interacción")]
    public GameObject pressEIndicator;

    private bool playerInRange = false;

    void Start()
    {
        if (pressEIndicator != null)
            pressEIndicator.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueSequence.Length == 0)
            {
                return;
            }

            DialogueData currentDialogue = dialogueSequence[dialogueIndex];

            Debug.Log($"Iniciando diálogo {dialogueIndex + 1} con {name}");

            DialogueManager.Instance.StartDialogue(currentDialogue);
            pressEIndicator.SetActive(false);

            dialogueIndex++;

            if (dialogueIndex >= dialogueSequence.Length)
                dialogueIndex = dialogueSequence.Length - 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (pressEIndicator != null)
                pressEIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (pressEIndicator != null)
                pressEIndicator.SetActive(false);
        }
    }
}
