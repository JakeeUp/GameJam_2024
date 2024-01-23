using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeComponent : MonoBehaviour
{
    public Enemy enemyScript; // Public variable to assign the enemy

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyScript != null)
        {
            enemyScript.StartChasingPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemyScript != null)
        {
            enemyScript.StopChasingPlayer();
        }
    }
}
