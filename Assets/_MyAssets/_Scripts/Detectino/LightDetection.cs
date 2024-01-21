using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    RagdollController ragdollController;
    private void Awake()
    {
        ragdollController = FindObjectOfType<RagdollController>();
    }
    private void Start()
    {
       
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player has been spotted!");
            if (ragdollController != null)
            {
                ragdollController.TurnOnRagDoll();
            }
           
        }
    }

  
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player is no longer in the light.");
        }
    }
}
