using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public DialogueNodeType type;

    [TextArea(2, 6)]
    public string sentence;
    
    public DialogueOption[] options; // solo para preguntas
}

public enum DialogueNodeType
{
    Sentence,
    Question
}
