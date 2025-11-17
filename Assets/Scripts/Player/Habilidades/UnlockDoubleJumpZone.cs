using UnityEngine;

public class UnlockDoubleJumpZone : MonoBehaviour
{
    public AnuncioManager anuncioManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AbilityManager.Instance.Unlock(AbilityType.DoubleJump);
            anuncioManager.MostrarAnuncio("¡Habilidad Desbloqueada: Doble Salto!");
        }
    }
}
