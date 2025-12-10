using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private enum QuestionPhase
    {
        ShowingQuestionText,
        ShowingOptions,
        ShowingWrongAnswer
    }

    private QuestionPhase questionPhase;

    [Header("UI ROOT")]
    public GameObject dialogueUI;

    [Header("UI TEXT")]
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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        typingCoroutine = null;
        isDialogueActive = false;
        RebindUI();
    }

    private void RebindUI()
    {
        dialogueUI = GameObject.FindWithTag("DialogueUi");
        if (dialogueUI == null) return;

        npcNameText = dialogueUI.transform.Find("NPCNameDialog").GetComponent<TMP_Text>();
        dialogueText = dialogueUI.transform.Find("DialogueText").GetComponent<TMP_Text>();
        pressSpaceTip = dialogueUI.transform.Find("PulsaEspacio").GetComponent<TMP_Text>();
        optionsPanel = dialogueUI.transform.Find("OptionsPanel").gameObject;
        optionTexts = optionsPanel.GetComponentsInChildren<TMP_Text>(true);

        dialogueUI.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void StartDialogue(DialogueData dialogue, GameObject engagedNPC = null)
    {
        if (isDialogueActive || dialogueUI == null) return;

        playerMovement ??= FindAnyObjectByType<PlayerMovement>();
        cameraMovement ??= FindAnyObjectByType<CameraMovement>();

        if (playerMovement) playerMovement.canMove = false;
        if (cameraMovement) cameraMovement.canMove = false;
        LockCursor();

        this.engagedNPC = engagedNPC;

        if (engagedNPC != null)
        {
            var trigger = engagedNPC.GetComponentInChildren<DialogueTrigger>();
            if (trigger != null)
                trigger.SetInteractionEnabled(false);
        }

        isDialogueActive = true;
        dialogueUI.SetActive(true);

        npcNameText.text = dialogue.npcName;
        nodes = dialogue.nodes;
        index = 0;

        ShowNextNode();
    }

    private void ShowNextNode()
    {
        if (index >= nodes.Length)
        {
            StartCoroutine(EndDialogueSmooth());
            return;
        }

        currentNode = nodes[index];

        optionsPanel.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        pressSpaceTip.gameObject.SetActive(true);

        currentSentence = currentNode.sentence;
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));

        if (currentNode.type == DialogueNodeType.Sentence)
            index++;
        else
            questionPhase = QuestionPhase.ShowingQuestionText;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void ShowOptions(DialogueOption[] options)
    {
        optionsPanel.SetActive(true);

        for (int i = 0; i < optionTexts.Length; i++)
        {
            bool active = i < options.Length;
            optionTexts[i].transform.parent.gameObject.SetActive(active);

            if (!active) continue;

            optionTexts[i].text = options[i].optionText;

            int captured = i;
            var btn = optionTexts[i].GetComponentInParent<UnityEngine.UI.Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => SelectOption(captured));
        }
    }

    private void SelectOption(int optionIndex)
    {
        LockCursor();

        DialogueOption option = currentNode.options[optionIndex];
        optionsPanel.SetActive(false);

        if (option.isCorrect)
        {
            index++;
            ShowNextNode();
        }
        else
        {
            StopTyping();
            currentSentence = option.responseIfWrong;
            questionPhase = QuestionPhase.ShowingWrongAnswer;

            dialogueText.gameObject.SetActive(true);
            pressSpaceTip.gameObject.SetActive(true);

            typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
        }
    }

    void StopTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        isTyping = false;
    }

    private IEnumerator EndDialogueSmooth()
    {
        yield return null;

        isDialogueActive = false;
        dialogueUI.SetActive(false);

        if (engagedNPC != null)
        {
            var trigger = engagedNPC.GetComponentInChildren<DialogueTrigger>();
            if (trigger) trigger.SetInteractionEnabled(true);

            engagedNPC = null;
        }

        LockCursor();

        if (playerMovement) playerMovement.canMove = true;
        if (cameraMovement) cameraMovement.canMove = true;
    }

    private void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopTyping();
                dialogueText.text = currentSentence;
            }
            else if (currentNode.type == DialogueNodeType.Sentence)
            {
                ShowNextNode();
            }
            else if (currentNode.type == DialogueNodeType.Question &&
                     questionPhase == QuestionPhase.ShowingQuestionText)
            {
                dialogueText.gameObject.SetActive(false);
                pressSpaceTip.gameObject.SetActive(false);

                ShowOptions(currentNode.options);
                UnlockCursor();

                questionPhase = QuestionPhase.ShowingOptions;
            }
            else if (questionPhase == QuestionPhase.ShowingWrongAnswer)
            {
                // ✅ Volvemos a la pregunta
                questionPhase = QuestionPhase.ShowingQuestionText;
                ShowNextNode();
            }
        }
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