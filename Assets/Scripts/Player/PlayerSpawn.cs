using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Header("Prefab del Jugador (solo se usa la 1ª vez)")]
    public GameObject playerPrefab;
    void Start()
    {
        GameObject existing = GameObject.FindGameObjectWithTag("Player");

        if (existing == null)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, transform.rotation);
            player.tag = "Player";
            DontDestroyOnLoad(player);
        }
        else
        {
            var cc = existing.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;
            existing.transform.SetPositionAndRotation(transform.position, transform.rotation);
            if (cc) cc.enabled = true;
        }
    }
}
