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

    public float typingSpeed = 0.05f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogueUI.SetActive(false);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        if (isDialogueActive) return;

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
            EndDialogue();
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

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Q))
            ShowNextSentence();
    }
}