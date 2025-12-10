using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private enum QuestionPhase
    {
        ShowingQuestionText,
        ShowingOptions
    }
    private QuestionPhase questionPhase;

    public GameObject dialogueUI;
    public TMP_Text npcNameText;
    public TMP_Text dialogueText;
    public TMP_Text pressSpaceTip;

    [Header("UI – Opciones")]
    public GameObject optionsPanel;
    public TMP_Text[] optionTexts;
    private DialogueNode[] nodes;
    private DialogueNode currentNode;
    private int index = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private string currentSentence = "";
    private Coroutine typingCoroutine;
    public bool IsDialogueActive => isDialogueActive;
    private PlayerMovement playerMovement;
    private CameraMovement cameraMovement;
    private GameObject engagedNPC;
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

        nodes = dialogue.nodes;
        index = 0;
        ShowNextNode();
    }

    void ShowNextNode()
    {
        if (index >= nodes.Length)
        {
            StartCoroutine(EndDialogueSmooth());
            return;
        }

        currentNode = nodes[index];

        if (currentNode.type == DialogueNodeType.Sentence)
        {
            optionsPanel.SetActive(false);
            dialogueText.gameObject.SetActive(true);
            pressSpaceTip.gameObject.SetActive(true);
            currentSentence = currentNode.sentence;
            typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
            index++;
        }
        else if (currentNode.type == DialogueNodeType.Question)
        {
            optionsPanel.SetActive(false);

            dialogueText.gameObject.SetActive(true);
            pressSpaceTip.gameObject.SetActive(true);

            currentSentence = currentNode.sentence;
            questionPhase = QuestionPhase.ShowingQuestionText;

            typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
        }
    }
    void ShowOptions(DialogueOption[] options)
    {
        optionsPanel.SetActive(true);

        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].transform.parent.gameObject.SetActive(i < options.Length);
            optionTexts[i].text = options[i].optionText;

            int capturedIndex = i;
            optionTexts[i]
                .GetComponentInParent<UnityEngine.UI.Button>()
                .onClick.RemoveAllListeners();

            optionTexts[i]
                .GetComponentInParent<UnityEngine.UI.Button>()
                .onClick.AddListener(() => SelectOption(capturedIndex));
        }
    }

    void SelectOption(int optionIndex)
    {
        DialogueOption option = currentNode.options[optionIndex];
        optionsPanel.SetActive(false);

        if (option.isCorrect)
        {
            LockCursor();
            index++;
            ShowNextNode();
        }
        else
        {
            StartCoroutine(WrongAnswerRoutine(option.responseIfWrong));
        }
    }

    IEnumerator WrongAnswerRoutine(string response)
    {
        dialogueText.gameObject.SetActive(true);
        pressSpaceTip.gameObject.SetActive(true);

        dialogueText.text = "";
        yield return StartCoroutine(TypeSentence(response));

        questionPhase = QuestionPhase.ShowingQuestionText;
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
        optionsPanel.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        pressSpaceTip.gameObject.SetActive(false);
        dialogueUI.SetActive(false);

        if (engagedNPC != null)
        {
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
            else if (currentNode.type == DialogueNodeType.Sentence)
            {
                ShowNextNode();
            }
            else if (currentNode.type == DialogueNodeType.Question
                    && questionPhase == QuestionPhase.ShowingQuestionText)
            {
                // Cambio de fase
                dialogueText.gameObject.SetActive(false);
                pressSpaceTip.gameObject.SetActive(false);

                optionsPanel.SetActive(true);
                ShowOptions(currentNode.options);
                UnlockCursor();
                questionPhase = QuestionPhase.ShowingOptions;
            }
        }

        if (playerMovement == null)
            playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (cameraMovement == null)
            cameraMovement = FindAnyObjectByType<CameraMovement>();
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}