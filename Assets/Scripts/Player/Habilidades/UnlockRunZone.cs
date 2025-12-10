using UnityEngine;

public class UnlockRunZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        AbilityManager.Instance.Unlock(AbilityType.Run);

        if (AnuncioManager.Instance != null)
        {
            AnuncioManager.Instance.MostrarAnuncio(
                "¡Habilidad Desbloqueada: Correr!"
            );
        }
    }
}