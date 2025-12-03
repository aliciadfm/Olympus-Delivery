using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [Tooltip("Identificador del actor que habla. Coincide con DialogueActor.actorId")]
    public string speakerId;

    [TextArea(2, 6)]
    public string text;

    [Tooltip("Retrato opcional (override) para esta línea")]
    public Sprite portrait;

    // Más campos posibles: voice clip, delay, emotion, etc.
}
