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
    private Coroutine typingCoroutine;
    private PlayerMovement playerMovement;

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
    }

    public void StartDialogue(DialogueData dialogue)
    {
        if (isDialogueActive) return;

        if (playerMovement == null)
            playerMovement = FindAnyObjectByType<PlayerMovement>();

        if (playerMovement != null)
            playerMovement.canMove = false;

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
            typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
            index++;
        }
        else
        {
            StartCoroutine(EndDialogueSmooth());
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator EndDialogueSmooth()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);

        // 🔹 Esperar un frame antes de reactivar el movimiento
        yield return null;

        if (playerMovement != null)
            playerMovement.canMove = true;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
            ShowNextSentence();

        if (playerMovement == null)
            playerMovement = FindAnyObjectByType<PlayerMovement>();
    }
}