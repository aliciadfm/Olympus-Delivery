using UnityEngine;

public class UnlockDoubleJumpZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        AbilityManager.Instance.Unlock(AbilityType.DoubleJump);

        if (AnuncioManager.Instance != null)
        {
            AnuncioManager.Instance.MostrarAnuncio("¡Habilidad Desbloqueada: Doble Salto!");
        }

        gameObject.SetActive(false);
    }
}
