using UnityEngine;

public class UnlockDoubleJumpZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AbilityManager.Instance.Unlock(AbilityType.DoubleJump);
        }
    }
}
