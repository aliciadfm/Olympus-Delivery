using UnityEngine;

public class UnlockDashZone : MonoBehaviour
{
    [SerializeField] private GameObject objetoAOcultar;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        AbilityManager.Instance.Unlock(AbilityType.Dash);

        if (AnuncioManager.Instance != null)
        {
            AnuncioManager.Instance.MostrarAnuncio("¡Habilidad Desbloqueada: Dash!");
        }

        if (objetoAOcultar != null)
        {
            objetoAOcultar.SetActive(false);
        }
    }
}