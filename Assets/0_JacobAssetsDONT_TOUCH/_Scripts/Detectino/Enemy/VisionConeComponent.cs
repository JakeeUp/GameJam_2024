using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeComponent : MonoBehaviour
{
    public Enemy enemyScript;
    public float animationDelay = 2.0f; // Duration of the animation

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyScript != null)
        {
            StartCoroutine(TriggerEnemyResponse());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemyScript != null)
        {
            enemyScript.StopChasingPlayer();
            StopAllCoroutines(); // Stop the coroutine if the player leaves the cone
        }
    }

    IEnumerator TriggerEnemyResponse()
    {
        enemyScript.PlaySpottedAnimation(); // Play the animation
        yield return new WaitForSeconds(animationDelay); // Wait for the animation to finish
        enemyScript.ResumeMovement(); // Resume enemy movement
        enemyScript.StartChasingPlayer(); // Start chasing the player
    }
}
