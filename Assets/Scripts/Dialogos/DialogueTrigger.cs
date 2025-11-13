using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData;
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
            DialogueManager.Instance.StartDialogue(dialogueData);
            pressEIndicator.SetActive(false);
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
