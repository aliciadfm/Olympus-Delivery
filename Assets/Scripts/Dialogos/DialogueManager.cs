using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueUI;
    public TMP_Text npcNameText;
    public TMP_Text dialogueText;

    private string[] sentences;
    private int index = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private string currentSentence = "";
    private Coroutine typingCoroutine;
    public bool IsDialogueActive => isDialogueActive;
    private PlayerMovement playerMovement;
    private CameraMovement cameraMovement;
    private GameObject engagedNPC; // the NPC currently engaged in dialogue
    public float typingSpeed = 0.05f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogueUI.SetActive(false);
    }

    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        cameraMovement = FindAnyObjectByType<CameraMovement>();
    }

    // Accept an optional engagedNPC so the trigger can pass which NPC started the dialogue
    public void StartDialogue(DialogueData dialogue, GameObject engagedNPC = null)
    {
        if (isDialogueActive) return;

        if (playerMovement == null)
            playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (cameraMovement == null)
            cameraMovement = FindAnyObjectByType<CameraMovement>();

        if (playerMovement != null)
            playerMovement.canMove = false;
        if (cameraMovement != null)
            cameraMovement.canMove = false;

        // Store the NPC that started the dialogue so we can disable only interaction (not visibility)
        this.engagedNPC = engagedNPC;
        if (this.engagedNPC != null)
        {
            // En lugar de desactivar todo el GameObject, deshabilitamos sólo la interacción del DialogueTrigger
            DialogueTrigger trigger = this.engagedNPC.GetComponent<DialogueTrigger>();
            if (trigger != null)
            {
                trigger.SetInteractionEnabled(false);
            }
            else
            {
                // Si no hay DialogueTrigger directamente en el root, buscamos en hijos (por si el trigger está en un hijo)
                trigger = this.engagedNPC.GetComponentInChildren<DialogueTrigger>();
                if (trigger != null)
                    trigger.SetInteractionEnabled(false);
            }
        }

        isDialogueActive = true;
        dialogueUI.SetActive(true);
        npcNameText.text = dialogue.npcName;

        sentences = dialogue.sentences;
        index = 0;
        ShowNextSentence();
    }

    public void ShowNextSentence()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (index < sentences.Length)
        {
            currentSentence = sentences[index];
            typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
            index++;
        }
        else
        {
            StartCoroutine(EndDialogueSmooth());
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private IEnumerator EndDialogueSmooth()
{
    isDialogueActive = false;
    dialogueUI.SetActive(false);

    if (engagedNPC != null)
    {
        // 🔹 AVISAMOS AL NPC DE QUE EL DIÁLOGO TERMINÓ
        HeadLookAtPlayer lookAt = engagedNPC.GetComponent<HeadLookAtPlayer>();
        if (lookAt == null)
            lookAt = engagedNPC.GetComponentInChildren<HeadLookAtPlayer>();

        if (lookAt != null)
            lookAt.EndDialogue();

        DialogueTrigger trigger = engagedNPC.GetComponent<DialogueTrigger>();
        if (trigger == null)
            trigger = engagedNPC.GetComponentInChildren<DialogueTrigger>();

        if (trigger != null)
            trigger.SetInteractionEnabled(true);

        engagedNPC = null;
    }

    yield return null;

    if (playerMovement != null)
        playerMovement.canMove = true;
    if (cameraMovement != null)
        cameraMovement.canMove = true;
}


    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                ShowNextSentence();
            }
        }

        if (playerMovement == null)
            playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (cameraMovement == null)
            cameraMovement = FindAnyObjectByType<CameraMovement>();
    }
}