using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeComponent : MonoBehaviour
{
    AIController controller;
   // public Enemy enemyScript;
    public float animationDelay = 2.0f; // Duration of the animation

    private void Awake()
    {
       // enemyScript= GetComponent<Enemy>();
        controller = GetComponent<AIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && controller != null)
        {
            StartCoroutine(TriggerEnemyResponse());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && controller != null)
        {
            //controller.StopChasingPlayer();
            StopAllCoroutines(); // Stop the coroutine if the player leaves the cone
        }
    }

    IEnumerator TriggerEnemyResponse()
    {
        controller.PlaySpottedAnimation(); // Play the animation
        yield return new WaitForSeconds(animationDelay); // Wait for the animation to finish
        //controller.ResumeMovement(); // Resume enemy movement
        //enemyScript.StartChasingPlayer(); // Start chasing the player
    }
}
