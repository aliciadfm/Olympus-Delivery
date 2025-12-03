using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[AddComponentMenu("Dialogue/DialogueManagerPro")]
public class DialogueManagerPro : MonoBehaviour
{
    public static DialogueManagerPro Instance { get; private set; }

    [Header("UI Controller (prefab o de la escena)")]
    public DialogueUIController uiController;

    [Header("Opciones de escritura")]
    public float typingSpeed = 0.03f;

    [Header("Referencias opcionales que se bloquearán durante diálogos")]
    public MonoBehaviour playerMovementLock; // por ejemplo PlayerMovement (usa canMove internamente)
    public MonoBehaviour cameraMovementLock; // CameraMovement

    private Coroutine typingCoroutine;
    private bool isDialogueActive = false;
    public bool IsDialogueActive => isDialogueActive;

    private DialogueSequence currentSequence;
    private Dictionary<string, DialogueActor> bindings = new Dictionary<string, DialogueActor>();
    private int currentIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Inicia un diálogo con la secuencia y un mapeo actorId -> DialogueActor (puede ser vacío).
    /// </summary>
    public void StartDialogue(DialogueSequence seq, Dictionary<string, DialogueActor> actorBindings)
    {
        if (seq == null) return;
        if (isDialogueActive) return;

        currentSequence = seq;
        bindings = actorBindings ?? new Dictionary<string, DialogueActor>();
        currentIndex = 0;

        LockControls(true);
        isDialogueActive = true;

        if (uiController != null)
            uiController.Show();

        ShowLine(currentIndex);
    }

    private void LockControls(bool lockState)
    {
        // Si tienes PlayerMovement con canMove, cambia la propiedad; si no, solo desactiva el componente.
        if (playerMovementLock != null)
        {
            var field = playerMovementLock.GetType().GetField("canMove");
            if (field != null)
                field.SetValue(playerMovementLock, !lockState);
            else
                playerMovementLock.enabled = !lockState;
        }

        if (cameraMovementLock != null)
        {
            var field2 = cameraMovementLock.GetType().GetField("canMove");
            if (field2 != null)
                field2.SetValue(cameraMovementLock, !lockState);
            else
                cameraMovementLock.enabled = !lockState;
        }
    }

    private void ShowLine(int index)
    {
        if (currentSequence == null || currentSequence.lines == null || index < 0 || index >= currentSequence.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentSequence.lines[index];

        // Resolvemos el actor (si existe)
        DialogueActor actor = null;
        if (!string.IsNullOrEmpty(line.speakerId))
            bindings.TryGetValue(line.speakerId, out actor);

        string useName = actor != null ? actor.displayName : (string.IsNullOrEmpty(line.speakerId) ? "" : line.speakerId);

        if (uiController != null)
        {
            uiController.SetName(useName);
            uiController.SetContinueHint(false);
            // Inicia la escritura
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(line.text));
        }
    }

    private IEnumerator TypeText(string text)
    {
        if (uiController == null) yield break;

        uiController.SetContent("");
        int i = 0;
        while (i < text.Length)
        {
            uiController.SetContent(text.Substring(0, i + 1));
            i++;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        uiController.SetContinueHint(true);
        typingCoroutine = null;
    }

    void Update()
    {
        if (!isDialogueActive) return;

        // Avanzar con Espacio (o cancelar escritura)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (typingCoroutine != null)
            {
                // Completar texto inmediatamente
                StopCoroutine(typingCoroutine);
                if (uiController != null && currentSequence != null && currentIndex < currentSequence.lines.Length)
                    uiController.SetContent(currentSequence.lines[currentIndex].text);

                typingCoroutine = null;
                if (uiController != null)
                    uiController.SetContinueHint(true);
            }
            else
            {
                // Pasar a siguiente linea
                currentIndex++;
                if (currentSequence != null && currentIndex < currentSequence.lines.Length)
                    ShowLine(currentIndex);
                else
                    EndDialogue();
            }
        }

        // También puedes permitir Escape para salir (opcional)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        currentSequence = null;
        bindings.Clear();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (uiController != null)
        {
            uiController.SetContinueHint(false);
            uiController.Hide();
        }

        LockControls(false);
    }
}