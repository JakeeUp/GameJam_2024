using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
     public float pushForce = 500f;  // Force magnitude
    public float grabDuration = 0.5f;  // Duration of the grab before the push
    public Enemy enemyScript;  // Reference to the Enemy script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RagdollController ragdollController = other.GetComponent<RagdollController>();
            if (ragdollController != null)
            {
                StartCoroutine(GrabAndPushPlayer(ragdollController, other.transform.position - transform.position));
                
                if (enemyScript != null)
                {
                    enemyScript.HitPlayer = true; // Set the boolean flag
                }
            }
        }
    }

    private IEnumerator GrabAndPushPlayer(RagdollController ragdollController, Vector3 pushDirection)
    {
        // Optional: Trigger any grab animations or effects here

        // Wait for the grab duration
        yield return new WaitForSeconds(grabDuration);

        // Apply the push force
        pushDirection.y = 0; // Keep the push horizontal
        ragdollController.TurnOnRagDollWithForce(pushDirection.normalized, pushForce);
    }
}
