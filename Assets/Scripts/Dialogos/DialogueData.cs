using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [Header("Información del NPC")]
    public string npcName;

    [TextArea(2, 6)]
    [Header("Líneas del diálogo")]
    public string[] sentences;
}
