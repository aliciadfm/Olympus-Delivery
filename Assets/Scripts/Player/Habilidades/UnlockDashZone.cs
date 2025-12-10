using UnityEngine;

public class UnlockDashZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        AbilityManager.Instance.Unlock(AbilityType.Dash);

        if (AnuncioManager.Instance != null)
        {
            AnuncioManager.Instance.MostrarAnuncio("¡Habilidad Desbloqueada: Dash!");
        }
    }
}
