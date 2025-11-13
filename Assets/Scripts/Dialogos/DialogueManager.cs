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

    private PlayerMovement playerMovement;
    private CameraMovement cameraMovement;
    public GameObject hermes;
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

    public void StartDialogue(DialogueData dialogue)
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
        hermes.SetActive(true);

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
                // 🔹 Completar inmediatamente el texto actual
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                // 🔹 Si ya está completo, pasar al siguiente
                ShowNextSentence();
            }
        }

        if (playerMovement == null)
            playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (cameraMovement == null)
            cameraMovement = FindAnyObjectByType<CameraMovement>();
    }
}