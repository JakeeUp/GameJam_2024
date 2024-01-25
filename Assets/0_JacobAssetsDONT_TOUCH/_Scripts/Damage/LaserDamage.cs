using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public float pushForce = 500f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RagdollController ragdollController = other.GetComponent<RagdollController>();
            if (ragdollController != null)
            {
                Vector3 forceDirection = -transform.forward;
                ragdollController.TurnOnRagDollWithForce(forceDirection, pushForce);
            }
        }
    }
}
