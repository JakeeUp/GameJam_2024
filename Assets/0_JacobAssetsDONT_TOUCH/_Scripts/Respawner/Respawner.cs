using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Respawner : MonoBehaviour
{
    public void RespawnPlayer(GameObject player, GameObject respawnPoint, float delay)
    {
        StartCoroutine(RespawnAfterDelay(player, respawnPoint, delay));
    }

    private IEnumerator RespawnAfterDelay(GameObject player, GameObject respawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.transform.position;
            Debug.Log("player respawn");

            player.SetActive(true); // Reactivate the player
        }
    }
}
