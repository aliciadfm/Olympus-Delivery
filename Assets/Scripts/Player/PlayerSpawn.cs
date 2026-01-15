using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            player = Instantiate(playerPrefab);
            player.tag = "Player";
            DontDestroyOnLoad(player);
        }
        
        var cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;

        var pm = player.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.ResetDeathState();
            pm.canMove = true;
        }

        yield return new WaitForSeconds(0.1f);

        if (cc) cc.enabled = true;
        
        Debug.Log("Jugador posicionado correctamente en el Mundo 2.");
    }
}
