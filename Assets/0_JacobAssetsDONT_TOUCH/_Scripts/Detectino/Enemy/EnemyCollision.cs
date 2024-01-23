using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public float pushForce = 500f;  // Force magnitude
    public Enemy enemyScript;       // Reference to the Enemy script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RagdollController ragdollController = other.GetComponent<RagdollController>();
            if (ragdollController != null)
            {
                Vector3 forceDirection = other.transform.position - transform.position;
                forceDirection.y = 0;
                ragdollController.TurnOnRagDoll(forceDirection.normalized, pushForce);

                if (enemyScript != null)
                {
                    enemyScript.HitPlayer = true; // Set the boolean flag
                }
            }
        }
    }
}
