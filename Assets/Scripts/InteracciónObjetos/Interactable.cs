using UnityEngine;

[DisallowMultipleComponent]
public class Interactable : MonoBehaviour
{
    [Header("Configuración del hint")]
    [Tooltip("Texto que aparecerá cuando el jugador mire el objeto. Si está vacío se mostrará el hint por defecto del raycaster.")]
    public string hintText = "Pulsa E para interactuar";

    [Header("Comportamiento por defecto")]
    [Tooltip("Si es true, al interactuar el GameObject se desactivará (gameObject.SetActive(false)).")]
    public bool disableOnInteract = true;

    /// <summary>
    /// Método que se ejecuta al interactuar. Puedes sobreescribirlo en subclases para comportamiento personalizado.
    /// </summary>
    public virtual void Interact()
    {
        if (disableOnInteract)
            gameObject.SetActive(false);
    }
}
