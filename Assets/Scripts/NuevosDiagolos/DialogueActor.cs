using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Dialogue/DialogueActor")]
public class DialogueActor : MonoBehaviour
{
    [Tooltip("ID único del actor. Ej: 'hermes', 'vendedor', 'player'")]
    public string actorId;

    [Tooltip("Nombre que aparecerá en la UI (si no se indica otro en la línea)")]
    public string displayName;

    [Header("Opcional: offset para el indicador 'Pulsa E'")]
    public Vector3 indicatorWorldOffset = Vector3.up * 2f;

    // Puedes expandir con audio, animador, etc.
}
