using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Animator doorAnimator;

    private void Start()
    {
        // Get the Animator component attached to the door
        doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the trigger
        if (other.CompareTag("Player"))
        {
            // Set the parameter to open the door
            doorAnimator.SetBool("Open", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player has exited the trigger area
        if (other.CompareTag("Player"))
        {
            // Set the parameter to close the door
            doorAnimator.SetBool("Open", false);
        }
    }
}
