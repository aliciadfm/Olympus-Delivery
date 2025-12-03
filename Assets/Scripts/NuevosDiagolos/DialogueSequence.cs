using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    [Header("ID interno (opcional)")]
    public string sequenceId;

    [Header("Líneas del diálogo (ordenadas)")]
    public DialogueLine[] lines;
}
