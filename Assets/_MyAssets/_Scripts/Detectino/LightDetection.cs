using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    private void Start()
    {
        // Ensure the player's collider is set to trigger if needed
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider the player touched is on the "Light" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player has been spotted!");
            // You can add additional functionality here, like handling the player getting caught
        }
    }

    // Optional: Log a message when the player exits the light
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player is no longer in the light.");
        }
    }
}
