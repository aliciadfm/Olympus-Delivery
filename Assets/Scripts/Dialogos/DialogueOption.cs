using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public bool isCorrect;

    [TextArea(2, 4)]
    public string responseIfWrong; // lo que dice el NPC si fallas
}
