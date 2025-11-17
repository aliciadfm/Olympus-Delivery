using UnityEngine;

public class UnlockDashZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AbilityManager.Instance.Unlock(AbilityType.Dash);
        }
    }
}
