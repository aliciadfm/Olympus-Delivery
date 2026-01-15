using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador pasa por encima del checkpoint
        if (other.CompareTag("Player"))
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm == null) pm = other.GetComponentInParent<PlayerMovement>();

            if (pm != null)
            {
                // Actualizamos el punto de respawn del jugador al de este objeto
                pm.SetRespawnPoint(this.transform);
                Debug.Log("Checkpoint alcanzado: " + gameObject.name);
            }
        }
    }
}
