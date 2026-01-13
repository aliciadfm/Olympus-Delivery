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

        player.transform.SetPositionAndRotation(transform.position, transform.rotation);

        yield return new WaitForEndOfFrame();

        if (cc) cc.enabled = true;

        var pm = player.GetComponent<PlayerMovement>();
        if (pm)
        {
            pm.canMove = true;
            pm.ResetDeathState();
        }
    }
}

