using UnityEngine;

public class UnlockRunZone : MonoBehaviour
{
    public AnuncioManager anuncioManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AbilityManager.Instance.Unlock(AbilityType.Run);
            anuncioManager.MostrarAnuncio("¡Habilidad Desbloqueada: Correr!");
        }
    }
}