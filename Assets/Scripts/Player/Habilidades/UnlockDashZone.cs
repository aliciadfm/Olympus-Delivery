using UnityEngine;

public class UnlockDashZone : MonoBehaviour
{
    public AnuncioManager anuncioManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AbilityManager.Instance.Unlock(AbilityType.Dash);
            anuncioManager.MostrarAnuncio("¡Habilidad Desbloqueada: Dash!");
        }
    }
}
